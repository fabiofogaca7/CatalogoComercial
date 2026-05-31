using System.ComponentModel.DataAnnotations;

namespace CatalogoComercial.Api.Models;

public class Produto
{
    public int Id { get; set; }

    [Required]
    [MinLength(5)]
    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }
}
