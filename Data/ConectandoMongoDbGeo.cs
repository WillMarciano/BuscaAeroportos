using BuscaAeroportos.Models;
using BuscaAeroportos.Settings;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BuscaAeroportos.Data
{
    public class ConectandoMongoDbGeo
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        public IMongoClient Client => _client;
        public IMongoCollection<Aeroporto> Airports => _database.GetCollection<Aeroporto>(Connection.Collection);
        public IConfiguration Configuration { get; }
        public MongoDbConfig Connection { get; set; }

        public ConectandoMongoDbGeo(IConfiguration configuration)
        {
            Configuration = configuration;
            Connection = new MongoDbConfig(Configuration);
            _client = new MongoClient(Connection.ConnectionString);
            _database = _client.GetDatabase(Connection.Name);
        }
    }
}
