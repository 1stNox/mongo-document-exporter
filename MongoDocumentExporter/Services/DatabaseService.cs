using MongoDB.Bson;
using MongoDB.Driver;
using MongoDocumentExporter.Utils;

namespace MongoDocumentExporter.Services;

public class DatabaseService
{
    private static MongoClient? DatabaseClient { get; set; }
    private static IMongoDatabase? Database { get; set; }
    public static EnvironmentVariables EnvironmentVariables { get; set; } = new();
    
    private static MongoClient GetDatabaseClient()
    {
        if (DatabaseClient is not null)
            return DatabaseClient;
        
        var mongodbUri = EnvironmentVariables.RetrieveEnvironmentVariable("MONGODB_URI");
        if (mongodbUri is null)
            throw new NullReferenceException("MONGODB_URI environment variable is not defined");

        DatabaseClient = new MongoClient(mongodbUri);

        return DatabaseClient;
    }
    
    private static IMongoDatabase GetDatabase()
    {
        if (Database is null)
        {
            var databaseName = EnvironmentVariables.RetrieveEnvironmentVariable("DATABASE");
            if (databaseName is null)
                throw new NullReferenceException("DATABASE must be defined");
            
            Database = GetDatabaseClient().GetDatabase(databaseName) ??
                       throw new ArgumentException($"Database {databaseName} does not exist");
        }

        return Database;
    }

    public static IMongoCollection<BsonDocument> GetCollection()
    {
        var collectionName = EnvironmentVariables.RetrieveEnvironmentVariable("COLLECTION");
        if (collectionName is null)
            throw new NullReferenceException("COLLECTION must be defined");

        var exists = GetDatabase().ListCollectionNames().ToList().Any(x => x == collectionName);
        if (!exists) throw new ArgumentException($"Collection {collectionName} does not exist");
        
        return GetDatabase().GetCollection<BsonDocument>(collectionName);
    }
}