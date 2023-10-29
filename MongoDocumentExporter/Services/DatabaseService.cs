using MongoDB.Driver;

namespace MongoDocumentExporter.Services;

public class DatabaseService
{
    private static MongoClient? DatabaseClient { get; set; }

    public static MongoClient GetDatabaseClient()
    {
        if (DatabaseClient is not null)
            return DatabaseClient;
        
        var mongodbUri = Environment.GetEnvironmentVariable("MONGODB_URI");
        if (mongodbUri is null)
            throw new NullReferenceException("MONGODB_URI environment variable is not defined");

        DatabaseClient = new MongoClient(mongodbUri);

        return DatabaseClient;
    }
}