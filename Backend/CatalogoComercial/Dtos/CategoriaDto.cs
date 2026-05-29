namespace CatalogoComercial.Api.Dtos;

public class CategoriaDto
{
    public int Id { get; set; }
    
    public string Nome { get; set; } = string.Empty;
    
    public string? Descricao { get; set; }
}
