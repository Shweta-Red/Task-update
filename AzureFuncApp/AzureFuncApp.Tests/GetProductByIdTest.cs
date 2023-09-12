using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AzureFuncApp;
using ProductBL.Models;
using ProductBL;
using Microsoft.AspNetCore.Http.Internal;

namespace AzureFuncApp.Tests
{
    public class GetProductByIdTests
    {
        [Fact]
        public async void TestGetProductById()
        {
            // Arrange
            var mockMain = new Mock<IMain>();
            var productId = "test-id";
            var expectedProduct = new Product { Id = productId, Name = "Test Product" };

            // Set up mock behavior
            mockMain.Setup(main => main.GetProductByIdAsync(productId))
                    .ReturnsAsync(expectedProduct);

            // Create a request with the route parameters
            var context = new DefaultHttpContext();
            ((dynamic)context.Request).RouteValues.Add("id", productId); // Set the "id" route parameter

            var request = new DefaultHttpRequest(context);

            // Act
            var response = (ObjectResult)await GetProductById_Func.Run(request, mockMain.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            var responseBody = (Product)response.Value;
            Assert.Equal(expectedProduct.Id, responseBody.Id);
            Assert.Equal(expectedProduct.Name, responseBody.Name);
        }
    }
}
