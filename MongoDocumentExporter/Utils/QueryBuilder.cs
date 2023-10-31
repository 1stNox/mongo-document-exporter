using MongoDB.Bson;
using MongoDB.Driver;
using MongoDocumentExporter.Models;

namespace MongoDocumentExporter.Utils;

public class QueryBuilder
{
    #region Builders

    private static FilterDefinitionBuilder<BsonDocument> FilterDefinitionBuilder { get; set; } =
        Builders<BsonDocument>.Filter;

    private static ProjectionDefinitionBuilder<BsonDocument> ProjectionDefinitionBuilder { get; set; } =
        Builders<BsonDocument>.Projection;
    
    private static SortDefinitionBuilder<BsonDocument> SortDefinitionBuilder { get; set; } =
        Builders<BsonDocument>.Sort;

    #endregion 

    private static async Task<FilterDefinition<BsonDocument>> BuildFilter(string rawFilter)
    {
        var filter = await BsonDeserializer.Deserialize(rawFilter);

        return FilterDefinitionBuilder.JsonSchema(filter);
    }
    
    private static async Task<ProjectionDefinition<BsonDocument>> BuildProjection(string rawProjection)
    {
        var projection = await BsonDeserializer.Deserialize(rawProjection);

        return ProjectionDefinitionBuilder.Combine(projection);
    }

    private static async Task<SortDefinition<BsonDocument>> BuildSort(string rawSort)
    {
        var sort = await BsonDeserializer.Deserialize(rawSort);

        return SortDefinitionBuilder.Combine(sort);
    }
    
    public static async Task<Query> Build(string rawProjection, string? rawFilter = null,
        string? rawSort = null)
    {
        FilterDefinition<BsonDocument> filter;
        if (rawFilter is null)
            filter = FilterDefinitionBuilder.Empty;
        else
            filter = await BuildFilter(rawFilter);

        var projection = await BuildProjection(rawProjection);

        if (rawSort is null)
            return new Query(filter, projection, null);

        var sort = await BuildSort(rawSort);
        return new Query(filter, projection, sort);
    }
}