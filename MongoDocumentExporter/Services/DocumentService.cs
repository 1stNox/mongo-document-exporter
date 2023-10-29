using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace MongoDocumentExporter.Services;

public class DocumentService
{
    private string DatabaseName { get; set; } = Environment.GetEnvironmentVariable("DATABASE_NAME") ??
                                                    throw new NullReferenceException(
                                                    "DATABASE_NAME environment variable is not set");

    private string CollectionName { get; set; } = Environment.GetEnvironmentVariable("COLLECTION_NAME") ??
                                                    throw new NullReferenceException(
                                                        "COLLECTION_NAME environment variable is not set");

    private string RawFilter { get; set; } = Environment.GetEnvironmentVariable("FILTER") ??
                                                throw new NullReferenceException("FILTER environment variable is not set");

    private string? RawSortOptions { get; set; } = Environment.GetEnvironmentVariable("SORT_OPTIONS");

    private string? RawProjection { get; set; } = Environment.GetEnvironmentVariable("PROJECTION") ??
                                                    throw new NullReferenceException("PROJECTION environment variable is not set");

    

    private Task<BsonDocument> DeserializeFilter()
    {
        var filter = JsonSerializer.Deserialize<object>(RawFilter).ToBsonDocument() ?? throw new NullReferenceException("FILTER environment variable could not be deserialized");
        return Task.FromResult(filter);
    }

    private Task<BsonDocument> DeserializeOptions()
    {
        BsonDocument? sortOptions = null;
        if (RawSortOptions != null)
            sortOptions = JsonSerializer.Deserialize<object>(RawSortOptions!).ToBsonDocument();
        var projectionOptions = JsonSerializer.Deserialize<object>(RawProjection!).ToBsonDocument() ?? throw new NullReferenceException("PROJECTION environment variable could not be deserialized");

        var options = new BsonDocument();
        if (sortOptions != null)
            options["sort"] = sortOptions;
        options["projection"] = projectionOptions;

        return Task.FromResult(options);
    }
}