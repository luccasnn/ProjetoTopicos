using System.Text.Json.Serialization;

namespace CadastroApi.Models
{
    // Representa uma cidade no sistema
    public class Cidade
    {
        // Identificador único da cidade
        public int Id { get; set; }

        // Nome da cidade
        public string Nome { get; set; } = string.Empty;

        // Estado em que a cidade está localizada
        public string Estado { get; set; } = string.Empty;

        // Lista de pessoas que moram na cidade (não será serializada para evitar ciclos)
        [JsonIgnore]
        public List<Pessoa> Pessoas { get; set; } = new();
    }
}
