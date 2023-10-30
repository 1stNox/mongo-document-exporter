using FluentAssertions;
using MongoDB.Bson;
using MongoDocumentExporter.Models;
using MongoDocumentExporter.Utils;
using Xunit;

namespace MongoDocumentExporter.Test.Utils;

public class QueryBuilderTest
{
    [Fact]
    public async void ShouldCreateAQueryWithFilterAndOptionsWithSortAndProjection()
    {
        // Given
        var rawFilter = "{ \"price\": { \"$gte\": 3000 } }";
        var rawProjectionOptions = "{ \"_id\": 1, \"createdAt\": 0 }";
        var rawSortOptions = "{ \"createdAt\": -1 }";
        var expectedOptions = new QueryOptions(
            new BsonDocument { ["_id"] = 1, ["createdAt"] = 0 }, 
            new BsonDocument { ["createdAt"] = -1 }
        );
        var expectedQuery = new Query(
            new BsonDocument
            {
                ["price"] = new BsonDocument
                {
                    ["$gte"] = 3000
                }
            },
            expectedOptions
        );

        // When
        var query = await QueryBuilder.Build(rawProjectionOptions, rawFilter, rawSortOptions);

        // Then
        query.Should().BeEquivalentTo(expectedQuery, "because they have the same values");
    }

    [Fact]
    public async void ShouldCreateAQueryWithFilterAndOptionsWithoutSortButProjection()
    {
        // Given
        var rawFilter = "{ \"price\": { \"$gte\": 3000 } }";
        var rawProjectionOptions = "{ \"_id\": 1, \"createdAt\": 0 }";
        string? rawSortOptions = null;
        var expectedOptions = new QueryOptions(
            new BsonDocument { ["_id"] = 1, ["createdAt"] = 0 }, 
            null
        );
        var expectedQuery = new Query(
            new BsonDocument
            {
                ["price"] = new BsonDocument
                {
                    ["$gte"] = 3000
                }
            },
            expectedOptions
        );

        // When
        var query = await QueryBuilder.Build(rawProjectionOptions, rawFilter, rawSortOptions);

        // Then
        query.Should().BeEquivalentTo(expectedQuery, "because they have the same values");
    }

    [Fact]
    public async void ShouldCreateAQueryWithoutFilterButOptionsWithSortAndProjection()
    {
        // Given
        string? rawFilter = null;
        var rawProjectionOptions = "{ \"_id\": 1, \"createdAt\": 0 }";
        var rawSortOptions = "{ \"createdAt\": -1 }";
        var expectedOptions = new QueryOptions(
            new BsonDocument { ["_id"] = 1, ["createdAt"] = 0 }, 
            new BsonDocument { ["createdAt"] = -1 }
        );
        var expectedQuery = new Query(
            new BsonDocument(),
            expectedOptions
        );

        // When
        var query = await QueryBuilder.Build(rawProjectionOptions, rawFilter, rawSortOptions);

        // Then
        query.Should().BeEquivalentTo(expectedQuery, "because they have the same values");
    }

    [Fact]
    public async void ShouldCreateAQueryWithoutFilterButOptionsWithoutSortButProjection()
    {
        // Given
        string? rawFilter = null;
        var rawProjectionOptions = "{ \"_id\": 1, \"createdAt\": 0 }";
        string? rawSortOptions = null;
        var expectedOptions = new QueryOptions(
            new BsonDocument { ["_id"] = 1, ["createdAt"] = 0 }, 
            null
        );
        var expectedQuery = new Query(
            new BsonDocument(),
            expectedOptions
        );

        // When
        var query = await QueryBuilder.Build(rawProjectionOptions, rawFilter, rawSortOptions);

        // Then
        query.Should().BeEquivalentTo(expectedQuery, "because they have the same values");
    }
}