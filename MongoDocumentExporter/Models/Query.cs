using MongoDB.Bson;

namespace MongoDocumentExporter.Models;

public class Query
{
    public BsonDocument Filter { get; set; }
    public QueryOptions Options { get; set; }

    public Query(BsonDocument filter, QueryOptions options)
    {
        Filter = filter;
        Options = options;
    }
}