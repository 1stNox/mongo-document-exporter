using MongoDB.Bson;
using MongoDB.Driver;
using MongoDocumentExporter.Utils;

namespace MongoDocumentExporter.Services;

public class DocumentService
{
    private IMongoCollection<BsonDocument> Collection { get; set; }
    
    public DocumentService(IMongoCollection<BsonDocument> collection)
    {
        Collection = collection;
    }
    
    public async Task<List<BsonDocument>?> FindDocuments(string rawProjectionOptions, string? rawFilter, string? rawSortOptions)
    {
        var query = await QueryBuilder.Build(rawProjectionOptions, rawFilter, rawSortOptions);
        
        if (query.Sort is null)
            return await Collection.Find(query.Filter).Project(query.Projection).ToListAsync();
        
        return await Collection.Find(query.Filter).Project(query.Projection).Sort(query.Sort).ToListAsync();
    }
}