using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using ProductBL.Models;
using Microsoft.AspNetCore.Http;
using ProductBL;
using System;
using Microsoft.AspNetCore.Http.Internal;

namespace AzureFuncApp
{
    public static class UpdateProduct_Func
    {
        [FunctionName("UpdateProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{productId}")] HttpRequest req,
            ILogger log,
            string productId)
        {
            log.LogInformation("UpdateProduct Function Triggered.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updatedProduct = JsonConvert.DeserializeObject<Product>(requestBody);

                var main = new Main("AccountEndpoint = https://cosmosdb-demo-1.documents.azure.com:443/;AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;",
                    "cosmosdb - demo - database",
                    "cosmosdb-demo-container-1");

                // Update the product
                var product = await main.UpdateProductAsync(productId, updatedProduct);

                if (product != null)
                {
                    return new OkObjectResult(product);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Error: {ex.Message}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        public static Task<ObjectResult> Run(DefaultHttpRequest request, IMain @object)
        {
            throw new NotImplementedException();
        }
    }
}
