using MongoDocumentExporter.Utils;
using Xunit;

namespace MongoDocumentExporter.Test.Utils;

public class QueryDeserializerTest
{
    private QueryDeserializer QueryDeserializer { get; set; } = new ();
    
    [Fact]
    public async void ShouldReturnSimpleQueryFilter()
    {
        // Given
        var simpleQuery = "{\"createdAt\":\"2023-10-20\"}";

        // When
        var result = await QueryDeserializer.DeserializeFilter(simpleQuery);

        // Then
        Assert.Equal("2023-10-20", result["createdAt"]);
    }

    [Fact]
    public async void ShouldReturnComplexQueryFilter()
    {
        // Given
        var complexQuery = "{\"createdAt\":{\"$in\":[\"1\",\"2\"]}}";

        // When
        var result = await QueryDeserializer.DeserializeFilter(complexQuery);

        // Then
        Assert.Equal("{ \"$in\" : [\"1\", \"2\"] }", result["createdAt"].ToString());
    }

    [Fact]
    public async void ShouldThrowAnExceptionDueToInvalidBson()
    {
        // Given
        var invalidQuery = "{-";

        // When

        // Then
        await Assert.ThrowsAsync<FormatException>(async () =>
        {
            await QueryDeserializer.DeserializeFilter(invalidQuery);
        });
    }
    
    [Fact]
    public void ShouldReturnQueryOptionsWithSortAndProjection()
    {
        // Given
        
        // When
        
        // Then
    }

    [Fact]
    public void ShouldReturnQueryOptionsWithNoSortButProjection()
    {
        // Given
        
        // When
        
        // Then
    }

    [Fact]
    public void ShouldThrowANullReferenceExceptionDueToProjectionEnvVarBeingNull()
    {
        // Given
        
        // When
        
        // Then
    }
}