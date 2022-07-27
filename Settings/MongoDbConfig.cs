using Microsoft.Extensions.Configuration;

namespace BuscaAeroportos.Settings
{
    public class MongoDbConfig
    {
        private IConfiguration _configuration;
        public string Name { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Collection { get; private set; }
        public string ConnectionString { get; private set; }
        public MongoDbConfig(IConfiguration _config)
        {
            _configuration = _config;
            Name = _config["MongoDbConfig:Name"].ToString();
            Host = _config["MongoDbConfig:Host"].ToString();
            Port = int.Parse(_config["MongoDbConfig:Port"].ToString());
            Collection = _config["MongoDbConfig:Collection"].ToString();
            ConnectionString = $"mongodb://{Host}:{Port}";
        }
    }
}
