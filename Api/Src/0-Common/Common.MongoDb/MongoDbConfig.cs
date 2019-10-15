namespace Common.MongoDb
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public bool Seed { get; set; }
    }
}
