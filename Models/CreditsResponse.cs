namespace MovieProBlazor.Models
{
    public class CreditsResponse
    {
        public int Id { get; set; }
        public List<Cast> Casts { get; set; } = [];
        public List<Crew> Crew { get; set; } = [];
    }
}