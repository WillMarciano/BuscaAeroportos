using Microsoft.Extensions.Configuration;

namespace BuscaAeroportos.Settings
{
    public class MongoDbConfig
    {
        public IConfiguration Configuration { get; }

        public string Name { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Collection { get; private set; }
        public string ConnectionString { get; private set; }

        public MongoDbConfig(IConfiguration configuration)
        {
            Configuration = configuration;
            Name = Configuration["MongoDbConfig:Name"].ToString();
            Host = Configuration["MongoDbConfig:Host"].ToString();
            Port = int.Parse(Configuration["MongoDbConfig:Port"].ToString());
            Collection = Configuration["MongoDbConfig:Collection"].ToString();
            ConnectionString = $"mongodb://{Host}:{Port}";

        }
    }
}
