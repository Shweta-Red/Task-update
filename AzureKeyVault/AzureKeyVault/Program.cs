using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

public class Program
{
    private readonly SecretClient _secretClient;
    private readonly DefaultAzureCredential _azureCredential;
   

    public Program(SecretClient secretClient, DefaultAzureCredential azureCredential)
    {
        _secretClient = secretClient;
        _azureCredential = azureCredential;
    }

    public async Task<string> GetCosmosDbConnectionStringAsync()
    {
        
        string secretName = "First-Key-Vault-cosmos";

        try
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);

            return secret.Value;
        }
        catch (Exception ex)
        {
            // Handle the exception or log it as needed
            throw;
        }
    }

    public static void Main(string[] args)
    {
        // Initialize dependencies
        string keyVaultName = "First-Key-Vault-cosmos";
        var secretClient = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"), new DefaultAzureCredential());

        // Create and run the program
        var program = new Program(secretClient, new DefaultAzureCredential());
        program.Run();
    }

    public void Run()
    {
        try
        {
            string cosmosDbConnectionString = GetCosmosDbConnectionStringAsync().Result;

            Console.WriteLine("Cosmos DB Connection String:");
            Console.WriteLine(cosmosDbConnectionString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
