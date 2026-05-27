using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIWizPedroKarut.Models
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [StringLength(300)]
        public string? ClienteNome { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string? Status { get; set; }

        public decimal ValorTotal { get; set; }

        public List<ItemPedido> Itens;

        public Pedido()
        {
            Itens = new List<ItemPedido>();
        }

        public decimal CalculaTotal(int qtd, decimal preco)
        {
            this.ValorTotal = qtd * preco;
            return this.ValorTotal;
        }
    }
}
