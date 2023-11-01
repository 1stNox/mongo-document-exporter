using FluentAssertions;
using MongoDB.Driver;
using MongoDocumentExporter.Services;
using MongoDocumentExporter.Utils;
using Moq;
using Testcontainers.MongoDb;
using Xunit;

namespace MongoDocumentExporter.Test.Services;

public class DatabaseServiceTest : IAsyncLifetime
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
    }

    public Task DisposeAsync()
    {
        return _mongodb.DisposeAsync().AsTask();
    }

    [Fact]
    public void ShouldReturnACollection()
    {
        // Given
        var mockedEnvironmentVariables = new Mock<EnvironmentVariables>();
        DatabaseService.EnvironmentVariables = mockedEnvironmentVariables.Object;

        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("MONGODB_URI"))
            .Returns("mongodb://test:test@localhost:27017");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("DATABASE"))
            .Returns("test");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("COLLECTION"))
            .Returns("TestCollection");
        
        // When
        var collection = DatabaseService.GetCollection();

        // Then
        collection.Should().NotBeNull();
    }
    
    [Fact]
    public void ShouldThrowNullReferenceExceptionDueToMissingMongoDbUri()
    {
        // Given
        Action action = () => DatabaseService.GetCollection();
        
        // When

        // Then
        action.Should().Throw<NullReferenceException>();
    }
    
    [Fact]
    public void ShouldThrowNullReferenceExceptionDueToMissingDatabase()
    {
        // Given
        Action action = () => DatabaseService.GetCollection();
        var mockedEnvironmentVariables = new Mock<EnvironmentVariables>();
        DatabaseService.EnvironmentVariables = mockedEnvironmentVariables.Object;

        mockedEnvironmentVariables.Setup(x => x.RetrieveEnvironmentVariable("MONGODB_URI"))
            .Returns("mongodb://test:test@localhost:27017");
        
        // When

        // Then
        action.Should().Throw<NullReferenceException>();
    }
    
    [Fact]
    public void ShouldThrowNullReferenceExceptionDueToMissingCollection()
    {
        // Given
        Action action = () => DatabaseService.GetCollection();
        var mockedEnvironmentVariables = new Mock<EnvironmentVariables>();
        DatabaseService.EnvironmentVariables = mockedEnvironmentVariables.Object;

        mockedEnvironmentVariables.Setup(x => x.RetrieveEnvironmentVariable("MONGODB_URI"))
            .Returns("mongodb://test:test@localhost:27017");
        
        mockedEnvironmentVariables.Setup(x => x.RetrieveEnvironmentVariable("DATABASE"))
            .Returns("test");
        
        // When

        // Then
        action.Should().Throw<NullReferenceException>();
    }
    
    [Fact]
    public void ShouldThrowArgumentExceptionDueToNonExistentCollection()
    {
        // Given
        Action action = () => DatabaseService.GetCollection();
        var mockedEnvironmentVariables = new Mock<EnvironmentVariables>();
        DatabaseService.EnvironmentVariables = mockedEnvironmentVariables.Object;

        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("MONGODB_URI"))
            .Returns("mongodb://test:test@localhost:27017");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("DATABASE"))
            .Returns("test");
        
        mockedEnvironmentVariables.Setup(envVars => envVars.RetrieveEnvironmentVariable("COLLECTION"))
            .Returns("abcdefg");
        
        // When

        // Then
        action.Should().Throw<ArgumentException>();
    }
}