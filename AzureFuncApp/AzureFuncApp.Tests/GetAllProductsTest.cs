using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using AzureFuncApp;
using ProductBL.Models;
using Microsoft.AspNetCore.Http.Internal;
using ProductBL;

namespace AzureFuncApp.Tests
{
    public class GetAllProductsTests
    {
        [Fact]
        public async void TestGetAllProducts()
        {
            // Arrange
            var mockMain = new Mock<IMain>();
            var expectedProducts = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1" },
                new Product { Id = "2", Name = "Product 2" },
                new Product { Id = "3", Name = "Product 3" }
            };

            // Set up mock behavior
            mockMain.Setup(main => main.GetAllProductsAsync())
                    .ReturnsAsync(expectedProducts);

            // Create a request without route parameters
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            // Act
            var response = (ObjectResult)await GetAllProducts_Func.Run(request, mockMain.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            var responseBody = (List<Product>)response.Value;
            Assert.Equal(expectedProducts.Count, responseBody.Count);

            // Check if all expected products are present in the response
            foreach (var expectedProduct in expectedProducts)
            {
                var matchingProduct = responseBody.FirstOrDefault(p => p.Id == expectedProduct.Id);
                Assert.NotNull(matchingProduct);
                Assert.Equal(expectedProduct.Name, matchingProduct.Name);
            }
        }
    }
}
