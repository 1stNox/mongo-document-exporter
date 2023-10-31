using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDocumentExporter.Services;

public class DatabaseService
{
    private static MongoClient? DatabaseClient { get; set; }
    private static IMongoDatabase? Database { get; set; }
    
    private static MongoClient GetDatabaseClient()
    {
        if (DatabaseClient is not null)
            return DatabaseClient;
        
        var mongodbUri = Environment.GetEnvironmentVariable("MONGODB_URI");
        if (mongodbUri is null)
            throw new NullReferenceException("MONGODB_URI environment variable is not defined");

        DatabaseClient = new MongoClient(mongodbUri);

        return DatabaseClient;
    }
    
    private static IMongoDatabase GetDatabase()
    {
        if (Database is null)
        {
            var databaseName = Environment.GetEnvironmentVariable("DATABASE");
            if (databaseName is null)
                throw new NullReferenceException("DATABASE must be defined");
            
            Database = GetDatabaseClient().GetDatabase(databaseName) ??
                       throw new ArgumentException($"Database {databaseName} does not exist");
        }

        return Database;
    }

    public static IMongoCollection<BsonDocument> GetCollection()
    {
        var collectionName = Environment.GetEnvironmentVariable("COLLECTION");
        if (collectionName is null)
            throw new NullReferenceException("DATABASE must be defined");

        return GetDatabase().GetCollection<BsonDocument>(collectionName) ??
               throw new ArgumentException($"Collection {collectionName} does not exist");
    }
}