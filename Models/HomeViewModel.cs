using System.ComponentModel.DataAnnotations;

namespace BuscaAeroportos.Models
{
    public enum TipoAeroporto
    {
        Todos,
        Municipais,
        Internacionais
    }

    public class HomeViewModel
    {
        [Required]
        public string Endereco { get; set; }
        public int Distancia { get; set; }
        public TipoAeroporto Tipo { get; set; }

        public override string ToString()
        {
            return $"Endereço: {Endereco}, Distância: {Distancia}, Tipo: {Tipo}.";
        }
    }
}
