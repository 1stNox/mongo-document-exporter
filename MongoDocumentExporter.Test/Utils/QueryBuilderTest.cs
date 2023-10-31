using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
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
        var expectedQuery = new Query(
            Builders<BsonDocument>.Filter.Gte("price", 3000),
            Builders<BsonDocument>.Projection.Include("_id").Exclude("createdAt"),
            Builders<BsonDocument>.Sort.Descending("createdAt")
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
        var expectedQuery = new Query(
            Builders<BsonDocument>.Filter.Gte("price", 3000),
            Builders<BsonDocument>.Projection.Include("_id").Exclude("createdAt"),
            null
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
        var expectedQuery = new Query(
            Builders<BsonDocument>.Filter.Empty,
            Builders<BsonDocument>.Projection.Include("_id").Exclude("createdAt"),
            Builders<BsonDocument>.Sort.Descending("createdAt")
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
        var expectedQuery = new Query(
            Builders<BsonDocument>.Filter.Empty,
            Builders<BsonDocument>.Projection.Include("_id").Exclude("createdAt"),
            null
        );

        // When
        var query = await QueryBuilder.Build(rawProjectionOptions, rawFilter, rawSortOptions);

        // Then
        query.Should().BeEquivalentTo(expectedQuery, "because they have the same values");
    }
}