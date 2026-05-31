using System.ComponentModel.DataAnnotations;

namespace CatalogoComercial.Api.Dtos;

public class CategoriaRequest
{
    [Required]
    [MinLength(5)]
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
