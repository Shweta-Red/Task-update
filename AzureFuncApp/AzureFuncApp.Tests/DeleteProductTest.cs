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
    public class DeleteProductTests
    {
        [Fact]
        public async void TestDeleteProduct()
        {
            // Arrange
            var mockMain = new Mock<IMain>();
            var productId = "test-id";

            // Set up mock behavior
            mockMain.Setup(main => main.DeleteProductAsync(productId))
                    .ReturnsAsync(true); // Assuming deletion was successful

            // Create a request with the route parameters
            var context = new DefaultHttpContext();
            ((dynamic)context.Request).RouteValues.Add("id", productId); // Set the "id" route parameter

            var request = new DefaultHttpRequest(context);

            // Act
            var response = (StatusCodeResult)await DeleteProduct_Func.Run(request, mockMain.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status204NoContent, response.StatusCode);
        }
    }
}
