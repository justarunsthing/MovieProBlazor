using System.Text.Json.Serialization;

namespace MovieProBlazor.Models
{
    public class CreditsResponse
    {
        public int Id { get; set; }

        [JsonPropertyName("cast")]
        public List<Cast> Casts { get; set; } = [];

        [JsonPropertyName("crew")]
        public List<Crew> Crew { get; set; } = [];
    }
}