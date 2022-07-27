using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace BuscaAeroportos.Models
{
    public class Aeroporto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> loc { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string code { get; set; }

        public Aeroporto(GeoJsonPoint<GeoJson2DGeographicCoordinates> loc, string name, string type, string code)
        {
            this.loc = loc;
            this.name = name;
            this.type = type;
            this.code = code;
        }

        public override string ToString()
        {
            var aeroporto = "";
            aeroporto += $"Loc: {loc}, " +
                   $"Name: {name}, " +
                   $"Type: {type}, " +
                   $"Code: {code}, ";

            return aeroporto;
        }
    }
}
