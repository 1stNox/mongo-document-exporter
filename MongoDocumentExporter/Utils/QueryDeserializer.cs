using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace MongoDocumentExporter.Utils;

public class QueryDeserializer
{
    public Task<BsonDocument> DeserializeFilter(string rawFilter)
    {
        return Task.FromResult(BsonSerializer.Deserialize<BsonDocument>(rawFilter));
    }

    public void DeserializeOptions()
    {
        var rawSortOptions = Environment.GetEnvironmentVariable("SORT_OPTIONS");
        var rawProjectionOptions = Environment.GetEnvironmentVariable("PROJECTION_OPTIONS");
    }
}