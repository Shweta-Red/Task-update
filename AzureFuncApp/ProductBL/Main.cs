using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using ProductBL.Models;

namespace ProductBL
{
    public class CosmosMain : IMain
    {
        private readonly Container _container;

        public CosmosMain(string connectionString, string databaseName, string containerName)
        {
            var client = new CosmosClient("AccountEndpoint=https://cosmosdb-demo-1.documents.azure.com:443/;AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;");
            _container = client.GetContainer("cosmosdb - demo - database", "cosmosdb-demo-container-1");
        }


        public async Task<Product> InsertProductAsync(Product product)
        {
            var response = await _container.CreateItemAsync(product, new PartitionKey(product.Id));
            return response.Resource;
        }

        public async Task<Product> UpdateProductAsync(string productId, Product updatedProduct)
        {
            var response = await _container.ReplaceItemAsync(updatedProduct, productId, new PartitionKey(productId));
            return response.Resource;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var response = await _container.DeleteItemAsync<Product>(productId, new PartitionKey(productId));
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Product>(productId, new PartitionKey(productId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var query = _container.GetItemQueryIterator<Product>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Product>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
