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
    public static class DeleteProduct_Func
    {
        [FunctionName("DeleteProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{productId}")] HttpRequest req,
            ILogger log,
            string productId)
        {
            log.LogInformation("DeleteProduct Function Triggered.");

            try
            {
                var main = new Main("AccountEndpoint = https://cosmosdb-demo-1.documents.azure.com:443/;AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;",
                    "cosmosdb - demo - database",
                    "cosmosdb-demo-container-1");

                // Delete the product
                var result = await main.DeleteProductAsync(productId);

                if (result)
                {
                    return new OkResult();
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

        public static Task<StatusCodeResult> Run(DefaultHttpRequest request, IMain @object)
        {
            throw new NotImplementedException();
        }
    }
}
