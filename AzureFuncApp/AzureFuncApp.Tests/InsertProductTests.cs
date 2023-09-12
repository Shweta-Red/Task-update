using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AzureFuncApp;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using ProductBL.Models;
using ProductBL;

namespace AzureFuncApp.Tests
{
    public class InsertProductTests
    {
        [Fact]
        public async void TestInsertProduct()
        {
            // Arrange
            var mockMain = new Mock<IMain>();
            var productToInsert = new Product { Id = "test-id", Name = "Test Product" };

            // Set up mock behavior
            mockMain.Setup(main => main.InsertProductAsync(It.IsAny<Product>()))
                    .ReturnsAsync(productToInsert);

            // Create a request with the JSON payload
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(JsonConvert.SerializeObject(productToInsert))
            };

            // Act
            var response = (ObjectResult)await insertProduct_Func.Run(request, mockMain.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
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
