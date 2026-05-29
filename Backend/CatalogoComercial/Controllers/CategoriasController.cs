using CatalogoComercial.Api.Data;
using CatalogoComercial.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoComercial.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly CatalogoDbContext _context;

    public CategoriasController(CatalogoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategoria([FromBody] Categoria categoria)
    {
        if (categoria == null)
        {
            return BadRequest("Dados da categoria inválidos.");
        }

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return Created("", categoria);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategorias()
    {
        var categorias = await _context.Categorias.ToListAsync();
        return Ok(categorias);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoria(int id, [FromBody] Categoria categoria)
    {
        if (categoria.Id != id)
        {
            return BadRequest("O ID da categoria no corpo da requisição deve corresponder ao ID na URL.");
        }

        var updateCategoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (updateCategoria == null)
        {
            return NotFound("Categoria não encontrada.");
        }
        
        updateCategoria.Nome = categoria.Nome;
        updateCategoria.Descricao = categoria.Descricao;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
        {
            return NotFound("Categoria não encontrada.");
        }

        var produtosAssociados = await _context.Produtos
            .AnyAsync(p => p.CategoriaId == id);

        if (produtosAssociados)
        {
            return Conflict("Não é possível excluir uma categoria que possua produtos vinculados.");
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
