using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDocumentExporter.Models;

public class Query
{
    public FilterDefinition<BsonDocument> Filter { get; set; }
    public ProjectionDefinition<BsonDocument> Projection { get; set; }
    public SortDefinition<BsonDocument>? Sort { get; set; }

    public Query(FilterDefinition<BsonDocument> filter, ProjectionDefinition<BsonDocument> projection, SortDefinition<BsonDocument>? sort)
    {
        Filter = filter;
        Projection = projection;
        Sort = sort;
    }
}