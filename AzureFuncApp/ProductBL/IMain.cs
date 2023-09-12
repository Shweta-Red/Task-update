using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using ProductBL.Models;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace ProductBL
{
    public interface IMain
    {
        Task<Product> InsertProductAsync(Product product);
        Task<Product> UpdateProductAsync(string productId, Product updatedProduct);
        Task<bool> DeleteProductAsync(string productId);
        Task<Product> GetProductByIdAsync(string productId);
        Task<List<Product>> GetAllProductsAsync();
    }

    public class Main : IMain
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public Main(string connectionString, string databaseName, string containerName)
        {
            var client = new CosmosClient("AccountEndpoint=https://cosmosdb-demo-1.documents.azure.com:443/;AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;");
            _container = client.GetContainer("cosmosdb - demo - database", "cosmosdb-demo-container-1");
        }

        public async Task<Product> InsertProductAsync(Product product)
        {
            var response = await _container.CreateItemAsync(product);
            return response.Resource;
        }

        public async Task<Product> UpdateProductAsync(string productId, Product updatedProduct)
        {
            var response = await _container.ReplaceItemAsync(updatedProduct, productId);
            return response.Resource;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            if (_container == null)
            {
                Console.WriteLine("Cosmos DB container is not initialized.");
                return false; // Or handle the situation as appropriate for your application
            }

            try
            {
                var response = await _container.DeleteItemAsync<Product>(productId, new PartitionKey(productId));
                return response.StatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting product: " + ex.Message);
                return false; // Or handle the exception as appropriate for your application
            }
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
