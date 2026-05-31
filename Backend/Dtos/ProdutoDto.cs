namespace CatalogoComercial.Api.Dtos;

public class ProdutoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int CategoriaId { get; set; }
    public CategoriaDto? Categoria { get; set; }
}
