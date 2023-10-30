using FluentAssertions;
using MongoDB.Bson;
using MongoDocumentExporter.Utils;
using Xunit;

namespace MongoDocumentExporter.Test.Utils;

public class BsonDeserializerTest
{
    [Fact]
    public async void ShouldReturnASimpleQuery()
    {
        // Given
        var simpleQuery = "{\"createdAt\":\"2023-10-20\"}";
        var expectedDocument = new BsonDocument
        {
            ["createdAt"] = "2023-10-20"
        };
        
        // When
        var result = await BsonDeserializer.Deserialize(simpleQuery);
        
        // Then
        result.Should().BeEquivalentTo(expectedDocument, "because they have the same values");
    }

    [Fact]
    public async void ShouldReturnAComplexQuery()
    {
        // Given
        var complexQuery = "{\"createdAt\":{\"$in\":[\"1\",\"2\"]}, \"updatedAt\":\"2023-10-21T12:00:00.000Z\"}";
        var expectedInRange = new BsonArray { "1", "2" };
        var expectedDocument = new BsonDocument
        {
            ["createdAt"] = new BsonDocument
            {
                ["$in"] = expectedInRange
            },
            ["updatedAt"] = "2023-10-21T12:00:00.000Z"
        };
        
        // When
        var result = await BsonDeserializer.Deserialize(complexQuery);
        
        // Then
        result.Should().BeEquivalentTo(expectedDocument);
    }

    [Fact]
    public void ShouldThrowAFormatException()
    {
        // Given
        var invalidQuery = "{-";
        Action action = () => BsonDeserializer.Deserialize(invalidQuery);
        
        // When
        
        // Then
        action.Should().Throw<FormatException>();
    }
}