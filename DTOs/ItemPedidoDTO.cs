using System.ComponentModel.DataAnnotations;

namespace APIWizPedroKarut.DTOs
{
    public class ItemPedidoDTO
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [StringLength(300)]
        public string? ProdutoNome { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public decimal PrecoUnitario { get; set; }

        [Required]
        public string PedidoId { get; set; }
    }
}
