using CatalogoComercial.Api.Data;
using CatalogoComercial.Api.Dtos;
using CatalogoComercial.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoComercial.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly CatalogoDbContext _context;

    public ProdutosController(CatalogoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduto(ProdutoRequest request)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == request.CategoriaId);

        if (categoria == null)
        {
            return BadRequest("Categoria associada ao produto não encontrada.");
        }

        var produto = new Produto
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco,
            CategoriaId = request.CategoriaId
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        var produtoDto = new ProdutoDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            CategoriaId = produto.CategoriaId,
            Categoria = new CategoriaDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao
            }
        };

        return Created("", produtoDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                CategoriaId = p.CategoriaId,
                Categoria = new CategoriaDto
                {
                    Id = p.Categoria.Id,
                    Nome = p.Categoria.Nome,
                    Descricao = p.Categoria.Descricao
                }
            })
            .ToListAsync();

        return Ok(produtos);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduto(int id, ProdutoRequest request)
    {
        var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == request.CategoriaId);

        if (categoria == null)
        {
            return BadRequest("Categoria associada ao produto não encontrada.");
        }

        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (produto == null)
        {
            return NotFound("Produto não encontrado.");
        }

        produto.Nome = request.Nome;
        produto.Descricao = request.Descricao;
        produto.Preco = request.Preco;
        produto.CategoriaId = request.CategoriaId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var deleteProduto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (deleteProduto == null)
        {
            return NotFound("Produto não encontrado.");
        }
        
        _context.Produtos.Remove(deleteProduto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
