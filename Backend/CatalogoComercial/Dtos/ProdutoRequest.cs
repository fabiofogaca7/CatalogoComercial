using System.ComponentModel.DataAnnotations;

namespace CatalogoComercial.Api.Dtos;

public class ProdutoRequest
{
    [Required]
    [MinLength(5)]
    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public int CategoriaId { get; set; }
}
