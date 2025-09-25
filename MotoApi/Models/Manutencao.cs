using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoApi.Models
{
    /// <summary>
    /// Representa um serviço de manutenção realizado em uma moto.
    /// </summary>
    public class Manutencao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Moto")]
        public int MotoId { get; set; }

        public Moto? Moto { get; set; } // Nullable agora

        [Required]
        public string TipoServico { get; set; }

        [Required]
        public DateTime DataServico { get; set; }

        [Required]
        public string Status { get; set; } // Pendente, Em andamento, Concluído

        public string Descricao { get; set; }
    }
}
