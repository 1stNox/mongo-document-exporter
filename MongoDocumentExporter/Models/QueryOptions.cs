using MongoDB.Bson;

namespace MongoDocumentExporter.Models;

public class QueryOptions
{
    public BsonDocument ProjectionOptions { get; set; }
    public BsonDocument? SortOptions { get; set; }

    public QueryOptions(BsonDocument projectionOptions, BsonDocument? sortOptions)
    {
        ProjectionOptions = projectionOptions;
        SortOptions = sortOptions;
    }
}