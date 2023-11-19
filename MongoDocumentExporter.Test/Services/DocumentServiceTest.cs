using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDocumentExporter.Services;
using MongoDocumentExporter.Utils;
using Moq;
using Testcontainers.MongoDb;
using Xunit;

namespace MongoDocumentExporter.Test.Services;

public class DocumentServiceTest : IAsyncLifetime
{
    private readonly MongoDbContainer _mongodb = new MongoDbBuilder()
        .WithImage("mongo:6")
        .WithUsername("test")
        .WithPassword("test")
        .WithPortBinding(27017)
        .WithExposedPort(27017)
        .Build();
    
    public async Task InitializeAsync()
    {
        await _mongodb.StartAsync();

        var client = new MongoClient("mongodb://test:test@localhost:27017");
        var db = client.GetDatabase("test");
        await db.CreateCollectionAsync("TestCollection");
        var collection = db.GetCollection<BsonDocument>("TestCollection");
        var testObjects = new List<BsonDocument>();
        testObjects.Add(new BsonDocument{{ "testField", 5000 }});
        testObjects.Add(new BsonDocument{{ "testField", 1 }});
        await collection.InsertManyAsync(testObjects);
    }

    public Task DisposeAsync()
    {
        var client = new MongoClient("mongodb://test:test@localhost:27017");
        var db = client.GetDatabase("test");
        var collection = db.GetCollection<BsonDocument>("TestCollection");
        collection.DeleteMany(FilterDefinition<BsonDocument>.Empty);
        
        return _mongodb.DisposeAsync().AsTask();
    }

    [Fact]
    public async void ShouldReturnAListWithAllDocuments()
    {
        // Given
        var expectedObjects = new List<BsonDocument>();
        expectedObjects.Add(new BsonDocument{{ "testField", 5000 }});
        expectedObjects.Add(new BsonDocument{{ "testField", 1 }});
        var rawProjectionOptions = "{}";
        var mockedEnvironmentVariables = new Mock<EnvironmentVariables>();
        DatabaseService.EnvironmentVariables = mockedEnvironmentVariables.Object;

        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("MONGODB_URI"))
            .Returns("mongodb://test:test@localhost:27017");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("DATABASE"))
            .Returns("test");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("COLLECTION"))
            .Returns("TestCollection");
        var documentService = new DocumentService(DatabaseService.GetCollection());
        
        // When
        var result = await documentService.FindDocuments(rawProjectionOptions, null, null);

        // Then
        result.Should().HaveCount(expectedObjects.Count);
        result.Any(firstObject => firstObject["testField"] == 5000).Should().BeTrue();
        result.Any(secondObject => secondObject["testField"] == 1).Should().BeTrue();
    }

    [Fact]
    public async void ShouldReturnAListWithOneObjectMatchingTheFilter()
    {
        // Given
        var rawProjectionOptions = "{}";
        var rawFilter = "{\"testField\": 5000}";
        var mockedEnvironmentVariables = new Mock<EnvironmentVariables>();
        DatabaseService.EnvironmentVariables = mockedEnvironmentVariables.Object;

        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("MONGODB_URI"))
            .Returns("mongodb://test:test@localhost:27017");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("DATABASE"))
            .Returns("test");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("COLLECTION"))
            .Returns("TestCollection");
        var documentService = new DocumentService(DatabaseService.GetCollection());
        
        // When
        var result = await documentService.FindDocuments(rawProjectionOptions, rawFilter, null);

        // Then
        result.Should().HaveCount(1);
        result.Any(firstObject => firstObject["testField"] == 5000).Should().BeTrue();
    }

    [Fact]
    public async void ShouldReturnAListWithAscendingValues()
    {
        // Given
        var expectedObjects = new List<BsonDocument>();
        expectedObjects.Add(new BsonDocument{{ "testField", 1 }});
        expectedObjects.Add(new BsonDocument{{ "testField", 5000 }});
        var rawProjectionOptions = "{\"_id\":0,\"testField\":1}";
        var rawSortOptions = "{\"testField\":1}";
        var mockedEnvironmentVariables = new Mock<EnvironmentVariables>();
        DatabaseService.EnvironmentVariables = mockedEnvironmentVariables.Object;

        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("MONGODB_URI"))
            .Returns("mongodb://test:test@localhost:27017");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("DATABASE"))
            .Returns("test");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("COLLECTION"))
            .Returns("TestCollection");
        var documentService = new DocumentService(DatabaseService.GetCollection());
        
        // When
        var result = await documentService.FindDocuments(rawProjectionOptions, null, rawSortOptions);

        // Then
        result.Should().HaveCount(expectedObjects.Count);
        result.Should().Equal(expectedObjects);
    }
}