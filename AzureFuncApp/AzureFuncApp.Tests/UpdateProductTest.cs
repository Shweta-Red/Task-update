using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AzureFuncApp;
using ProductBL.Models;
using Newtonsoft.Json;
using ProductBL;
using Microsoft.AspNetCore.Http.Internal;

namespace AzureFuncApp.Tests
{
    public class UpdateProductTests
    {
        [Fact]
        public async void TestUpdateProduct()
        {
            // Arrange
            var mockMain = new Mock<IMain>();
            var productId = "test-id";
            var updatedProduct = new Product { Id = productId, Name = "Updated Product" };

            // Set up mock behavior
            mockMain.Setup(main => main.UpdateProductAsync(productId, It.IsAny<Product>()))
                    .ReturnsAsync(updatedProduct);

            // Create a request with the JSON payload
            var context = new DefaultHttpContext();
            ((dynamic)context.Request).RouteValues.Add("id", productId); // Set the "id" route parameter
            var request = new DefaultHttpRequest(context)
            {
                Body = GenerateStreamFromString(JsonConvert.SerializeObject(updatedProduct))
            };

            // Act
            var response = (ObjectResult)await UpdateProduct_Func.Run(request, mockMain.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            var responseBody = (Product)response.Value;
            Assert.Equal(updatedProduct.Id, responseBody.Id);
            Assert.Equal(updatedProduct.Name, responseBody.Name);
        }

        // Helper method to convert string to stream
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
