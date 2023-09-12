using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using ProductBL.Models;
using Microsoft.AspNetCore.Http;
using ProductBL;
using System;
using Microsoft.AspNetCore.Http.Internal;

namespace AzureFuncApp
{
    public static class GetAllProducts_Func
    {
        [FunctionName("GetAllProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetAllProducts Function Triggered.");

            try
            {
                var main = new Main("AccountEndpoint = https://cosmosdb-demo-1.documents.azure.com:443/;AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;",
                    "cosmosdb - demo - database",
                    "cosmosdb-demo-container-1");

                // Get all products
                var products = await main.GetAllProductsAsync();

                return new OkObjectResult(products);
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
