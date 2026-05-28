using System.ComponentModel.DataAnnotations;

namespace APIWizPedroKarut.DTOs
{
    public class PedidoDTO
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [StringLength(300)]
        public string? ClienteNome { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string? Status { get; set; }

        public decimal ValorTotal { get; set; }
    }
}
