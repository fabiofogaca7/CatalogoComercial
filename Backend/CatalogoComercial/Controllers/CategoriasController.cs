using CatalogoComercial.Api.Data;
using CatalogoComercial.Api.Dtos;
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
    public async Task<IActionResult> CreateCategoria(CategoriaRequest request)
    {
        var categoria = new Categoria
        {
            Nome = request.Nome,
            Descricao = request.Descricao
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        var categoriaDto = new CategoriaDto
        {
            Nome = categoria.Nome,
            Descricao = categoria.Descricao
        };

        return Created("", categoriaDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategorias()
    {
        var categorias = await _context.Categorias
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao
            })
            .ToListAsync();

        return Ok(categorias);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoria(int id, CategoriaRequest request)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (categoria == null)
        {
            return NotFound("Categoria não encontrada.");
        }

        categoria.Nome = request.Nome;
        categoria.Descricao = request.Descricao;
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
