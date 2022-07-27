using BuscaAeroportos.Models;
using BuscaAeroportos.Settings;
using MongoDB.Driver;

namespace BuscaAeroportos.Data
{
    public class ConectandoMongoDbGeo
    {
        private static readonly MongoDbConfig _config;
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;
        public IMongoClient Client => _client;
        public IMongoCollection<Aeroporto> Airports => _database.GetCollection<Aeroporto>(_config.Collection);

        static ConectandoMongoDbGeo()
        {
            _client = new MongoClient(_config.ConnectionString);
            _database = _client.GetDatabase(_config.Name);
        }
    }
}
