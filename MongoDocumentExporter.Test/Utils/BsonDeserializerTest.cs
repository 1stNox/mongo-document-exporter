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
        
        // When
        var result = await BsonDeserializer.Deserialize(simpleQuery);
        
        // Then
        Assert.Equal("2023-10-20", result["createdAt"]);
    }

    [Fact]
    public async void ShouldReturnAComplexQuery()
    {
        // Given
        var complexQuery = "{\"createdAt\":{\"$in\":[\"1\",\"2\"]}, \"updatedAt\":\"2023-10-21T12:00:00.000Z\"}";
        
        // When
        var result = await BsonDeserializer.Deserialize(complexQuery);
        
        // Then
        Assert.Equal("{ \"$in\" : [\"1\", \"2\"] }", result["createdAt"].ToString());
        Assert.Equal("2023-10-21T12:00:00.000Z", result["updatedAt"].ToString());
    }

    [Fact]
    public async void ShouldThrowAFormatException()
    {
        // Given
        var invalidQuery = "{-";
        
        // When
        
        // Then
        await Assert.ThrowsAsync<FormatException>(async () =>
        {
            await BsonDeserializer.Deserialize(invalidQuery);
        });
    }
}