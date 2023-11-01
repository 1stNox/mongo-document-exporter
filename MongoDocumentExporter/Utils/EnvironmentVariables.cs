namespace MongoDocumentExporter.Utils;

public class EnvironmentVariables
{
    public virtual string? RetrieveEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name);
    }
}