using MongoDB.Bson;
using MongoDocumentExporter.Models;

namespace MongoDocumentExporter.Utils;

public class QueryBuilder
{
    private static async Task<QueryOptions> BuildOptions(string rawProjectionOptions, string? rawSortOptions = null)
    {
        var projectionOptions = await BsonDeserializer.Deserialize(rawProjectionOptions);
        BsonDocument? sortOptions = null;

        if (rawSortOptions != null) 
            sortOptions = await BsonDeserializer.Deserialize(rawSortOptions);

        return new QueryOptions(projectionOptions, sortOptions);
    }

    public static async Task<Query> Build(string rawProjectionOptions, string? rawFilter = null, string? rawSortOptions = null)
    {
        BsonDocument filter;
        if (rawFilter is null)
            filter = new BsonDocument();
        else
            filter = await BsonDeserializer.Deserialize(rawFilter);

        var options = await QueryBuilder.BuildOptions(rawProjectionOptions, rawSortOptions);

        return new Query(filter, options);
    }
}