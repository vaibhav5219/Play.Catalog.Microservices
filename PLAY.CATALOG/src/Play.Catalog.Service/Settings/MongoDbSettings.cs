namespace Play.Catalog.Service.Settings
{
    public class MongoDbSettings
    {
        public int Port { get; init; }
        public string Host { get; init; }
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}