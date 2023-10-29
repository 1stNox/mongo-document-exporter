using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MongoDocumentExporter.Utils;

public class BsonDeserializer
{
    public static Task<BsonDocument> Deserialize(string bson)
    {
        return Task.FromResult(BsonSerializer.Deserialize<BsonDocument>(bson));
    } 
}