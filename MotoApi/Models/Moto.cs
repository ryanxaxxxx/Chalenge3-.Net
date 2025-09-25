namespace MotoApi.Models
{
    public class Moto
    {
        public int Id { get; set; }
        public string Modelo { get; set; } = string.Empty; // ex: pop100, eletrica, 150
        public string Placa { get; set; } = string.Empty;
    }
}
