using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http;
using System;
using static System.Net.WebRequestMethods;
using ProductBL;           
using ProductBL.Models;
using Microsoft.AspNetCore.Http.Internal;

namespace AzureFuncApp
{
    public static class insertProduct_Func
    {
        [FunctionName("InsertProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("InsertProduct Function Triggered.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Product>(requestBody);

                var main = new Main("AccountEndpoint = https://cosmosdb-demo-1.documents.azure.com:443/;AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;",
                    "cosmosdb - demo - database",
                    "cosmosdb-demo-container-1");

                // Inserting the product
                var insertedProduct = await main.InsertProductAsync(product);

                return new OkObjectResult(insertedProduct);
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
