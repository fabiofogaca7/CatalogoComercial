using System.ComponentModel.DataAnnotations;

namespace CatalogoComercial.Api.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required]
    [MinLength(5)]
    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public ICollection<Produto> Produtos { get; set; } = [];
}
