using APIWizPedroKarut.Data;
using APIWizPedroKarut.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWizPedroKarut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly ILogger<PedidoController> _logger;
        private AppDbContext _context;

        public PedidoController(ILogger<PedidoController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            return Ok(pedidos);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetPorStatus(string status)
        {
            var pedidos = await _context.Pedidos.Where(p => p.Status == status).ToListAsync();
            if (pedidos == null)
                return NotFound("Nenhum pedido encontrado com esse status");
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var pedido = await _context.Pedidos.FindAsync(Guid.Parse(id));
            if (pedido == null)
            {
                _logger.LogInformation("Pedido com ID {PedidoId} não encontrado.", id);
                return NotFound();
            }
            var itens = await _context.ItensPedido.Where(i => i.PedidoId == id).ToListAsync();
            return Ok(new { Pedido = pedido, Itens = itens });
        }

        [HttpPost]
        public async Task<IActionResult> Post(string nome, string status)
        {
            var novoPedido = new Pedido();
            novoPedido.Status = status;
            novoPedido.ClienteNome = nome;
            await _context.Pedidos.AddAsync(novoPedido);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Novo pedido criado com ID {PedidoId} para o cliente {ClienteNome}.", novoPedido.Id, nome);
            return Ok("Pedido criado: " + novoPedido.Id);

        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, string nome, string status)
        {
            var pedido = await _context.Pedidos.FindAsync(Guid.Parse(id));
            if (pedido == null)
            {
                _logger.LogInformation("Pedido com ID {PedidoId} não encontrado para atualização.", id);
                return NotFound("Pedido não encontrado");
            }
            pedido.ClienteNome = nome;            
            if(pedido.Status == "Pago" && status == "Cancelado")
            {
                _logger.LogInformation("Tentativa de cancelar um pedido pago com ID {PedidoId}.", id);
                return BadRequest("Não é possível alterar um pedido cancelado");
            }
            pedido.Status = status;
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Pedido com ID {PedidoId} atualizado para o cliente {ClienteNome} com status {Status}.", id, nome, status);
            return Ok(pedido);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var pedido = await _context.Pedidos.FindAsync(Guid.Parse(id));
            if (pedido == null)
            {
                _logger.LogInformation("Pedido com ID {PedidoId} não encontrado para exclusão.", id);
                return NotFound();
            }
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Pedido com ID {PedidoId} excluído com sucesso.", id);
            return Ok("Pedido excluído: " + id);
        }

    }
}
