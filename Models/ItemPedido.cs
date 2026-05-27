using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIWizPedroKarut.Models
{
    [Table("ItemPedido")]
    public class ItemPedido
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

        [JsonIgnore]
        public Pedido? Pedido;

        
    }
}
