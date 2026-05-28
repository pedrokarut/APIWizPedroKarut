using APIWizPedroKarut.Data;
using APIWizPedroKarut.DTOs;
using APIWizPedroKarut.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIWizPedroKarut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemPedidoController : ControllerBase
    {
        private readonly ILogger<ItemPedidoController> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ItemPedidoController(ILogger<ItemPedidoController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var itens = await _context.ItensPedido.ToListAsync();
            var itensDTO = _mapper.Map<List<ItemPedidoDTO>>(itens);
            return Ok(itens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await _context.ItensPedido.FindAsync(Guid.Parse(id));
            var itemDTO = _mapper.Map<ItemPedidoDTO>(item);
            if (itemDTO == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        [HttpPost]
        public async Task<IActionResult> Post(string nome, int qtd, decimal preco, string pedidoId)
        {
            if(qtd <=0)
            {
                _logger.LogInformation("Tentativa de criar item de pedido com quantidade inválida: {Quantidade}", qtd);
                return BadRequest("Quantidade deve ser maior que zero.");
            }
            if(preco <=0)
            {
                _logger.LogInformation("Tentativa de criar item de pedido com preço inválido: {Preco}", preco);
                return BadRequest("Preço deve ser maior que zero.");
            }

            var novoItem = new ItemPedido();
            novoItem.ProdutoNome = nome;
            novoItem.Quantidade = qtd;
            novoItem.PrecoUnitario = preco;
            novoItem.PedidoId = pedidoId;
            await _context.ItensPedido.AddAsync(novoItem);

            var pedido = await _context.Pedidos.FindAsync(Guid.Parse(pedidoId));
            pedido.CalculaTotal(qtd, preco);
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Item de pedido criado com sucesso: {ProdutoNome}, Quantidade: {Quantidade}, Preço: {Preco}", nome, qtd, preco);
            return Ok(novoItem);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, string nomeProduto, int quantidade, decimal preco)
        {
            var item = await _context.ItensPedido.FindAsync(Guid.Parse(id));
            if (item == null)
            {
                _logger.LogInformation("Tentativa de atualizar item de pedido não encontrado: {Id}", id);
                return NotFound("Item de pedido não encontrado.");
            }
            if (quantidade <= 0)
            {
                _logger.LogInformation("Tentativa de atualizar item de pedido com quantidade inválida: {Quantidade}", quantidade);
                return BadRequest("Quantidade deve ser maior que zero.");
            }
            if (preco <= 0)
            {
                _logger.LogInformation("Tentativa de atualizar item de pedido com preço inválido: {Preco}", preco);
                return BadRequest("Preço deve ser maior que zero.");
            }

            item.ProdutoNome = nomeProduto;
            item.Quantidade = quantidade;
            item.PrecoUnitario = preco;
            _context.ItensPedido.Update(item);

            var pedido = await _context.Pedidos.FindAsync(Guid.Parse(item.PedidoId));
            pedido.CalculaTotal(quantidade, preco);
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Item de pedido atualizado com sucesso: {ProdutoNome}, Quantidade: {Quantidade}, Preço: {Preco}", nomeProduto, quantidade, preco);
            return Ok(item);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _context.ItensPedido.FindAsync(Guid.Parse(id));
            if (item == null)
            {
                _logger.LogInformation("Tentativa de excluir item de pedido não encontrado: {Id}", id);
                return NotFound();
            }
            _context.ItensPedido.Remove(item);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Item de pedido excluído com sucesso: {Id}", id);
            return Ok("Item excluído com sucesso:" + id);

        }

    }
}
