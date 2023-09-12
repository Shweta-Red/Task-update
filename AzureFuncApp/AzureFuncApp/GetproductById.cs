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
    public static class GetProductById_Func
    {
        [FunctionName("GetProductById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{productId}")] HttpRequest req,
            ILogger log,
            string productId)
        {
            log.LogInformation("GetProductById Function Triggered.");

            try
            {
                var main = new Main("AccountEndpoint = https://cosmosdb-demo-1.documents.azure.com:443/;AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;",
                    "cosmosdb - demo - database",
                    "cosmosdb-demo-container-1");

                // Get the product by ID
                var product = await main.GetProductByIdAsync(productId);

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
