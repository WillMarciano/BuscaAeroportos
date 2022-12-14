using BuscaAeroportos.Models;
using BuscaAeroportos.Models.Geocoding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BuscaAeroportos.Data;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BuscaAeroportos.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ConectandoMongoDbGeo _repository;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = new ConectandoMongoDbGeo(configuration);
        }

        public ActionResult Index()
        {
            //coordenadas quaisquer para mostrar o mapa
            var coordenadas = new Coordenada("São Paulo", "-23.64340873969638", "-46.730219057147224");
            return View(coordenadas);
        }

        public async Task<JsonResult> Localizar(HomeViewModel model)
        {
            await Task.Delay(0);
            Debug.WriteLine(model);

            //Captura a posição atual e adiciona a lista de pontos
            Coordenada coordLocal = ObterCoordenadasDaLocalizacao(model.Endereco);
            var aeroportosProximos = new List<Coordenada>();
            aeroportosProximos.Add(coordLocal);

            //Captura a latitude e longitude locais
            double lat = Convert.ToDouble(coordLocal.Latitude.Replace(".", ","));
            double lon = Convert.ToDouble(coordLocal.Longitude.Replace(".", ","));

            //Testa o tipo de aeroporto que será usado na consulta
            string tipoAero = "";

            if (model.Tipo == TipoAeroporto.Internacionais)
                tipoAero = "International";
            if (model.Tipo == TipoAeroporto.Municipais)
                tipoAero = "Municipal";

            //Captura o valor da distancia
            int distancia = model.Distancia * 1000;

            //Configura o ponto atual no mapa 
            var ponto = new GeoJson2DGeographicCoordinates(lon, lat);
            var localizacao = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(ponto);

            // filtro
            var construtor = Builders<Aeroporto>.Filter;
            var condicao = construtor.NearSphere(x => x.loc, localizacao, distancia);

            if (tipoAero != "")
                condicao = construtor.NearSphere(x => x.loc, localizacao, distancia) & construtor.Eq(x => x.type, tipoAero);

            //Captura  a lista
            var listaAeroportos = await _repository.Airports.Find(condicao).ToListAsync();

            //Escreve os pontos
            foreach (var item in listaAeroportos)
            {
                var aero = new Coordenada(item.name,
                                          item.loc.Coordinates.Latitude.ToString(),
                                          item.loc.Coordinates.Longitude.ToString());
                aeroportosProximos.Add(aero);
            }

            return Json(aeroportosProximos);
        }

        private Coordenada ObterCoordenadasDaLocalizacao(string endereco)
        {
            string url = $"{_configuration["GoogleMaps:Url"]}{endereco}";
            Debug.WriteLine(url);

            var coord = new Coordenada("Não Localizado", "-10", "-10");
            var response = new WebClient().DownloadString(url);
            var googleGeocode = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(response);
            //Debug.WriteLine(googleGeocode);

            if (googleGeocode.status == "OK")
            {
                coord.Nome = googleGeocode.results[0].formatted_address;
                coord.Latitude = googleGeocode.results[0].geometry.location.lat;
                coord.Longitude = googleGeocode.results[0].geometry.location.lng;
            }

            return coord;
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
