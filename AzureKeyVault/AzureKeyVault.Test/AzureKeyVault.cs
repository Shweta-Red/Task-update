using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel;

public class ProgramTests
{
    
    public async void Main_SuccessfullyGetsSecret()
    {
        // Arrange
        string keyVaultName = "First-Key-Vault-cosmos";
        string secretName = "First-Key-Vault-cosmos";
        string expectedSecretValue = "AccountEndpoint=https://cosmosdb-demo-1.documents.azure.com:443/;" +
            "AccountKey=GNZkD3NSbfR9HixHFOhgnOH3oaLd2F5VqvGNjSYOSSYcoyzzLB5vXTbujokCHYZeTtbrQ2IIS4idACDbZqYkqw==;";

        var mockSecretClient = new Mock<SecretClient>(MockBehavior.Strict);
        var mockKeyVaultSecret = new Mock<KeyVaultSecret>(MockBehavior.Strict);

        mockSecretClient
      .Setup(client => client.GetSecret(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Response.FromValue(mockKeyVaultSecret.Object, new Mock<Response>().Object));



        mockKeyVaultSecret
            .Setup(secret => secret.Value)
            .Returns(expectedSecretValue);

        var mockCredential = new Mock<TokenCredential>(MockBehavior.Strict);
        var mockAzureCredential = new Mock<DefaultAzureCredential>(MockBehavior.Strict);

        mockAzureCredential
            .Setup(credential => credential.GetTokenAsync(It.IsAny<TokenRequestContext>(), default))
            .ReturnsAsync(new AccessToken("fake_token", DateTimeOffset.UtcNow.AddMinutes(5)));

        var program = new Program(mockSecretClient.Object, mockAzureCredential.Object);

        // Act
        string cosmosDbConnectionString = await program.GetCosmosDbConnectionStringAsync();

        // Assert
        string consoleOutput = GetConsoleOutput();
        Xunit.Assert.Contains("Cosmos DB Connection String:", consoleOutput);
        Xunit.Assert.Contains(expectedSecretValue, consoleOutput);
    }

    private string GetConsoleOutput()
    {
        using (var consoleOutput = new System.IO.StringWriter())
        {
            Console.SetOut(consoleOutput);
            return consoleOutput.ToString();
        }
    }
}
