// Entidade preco de produto (fora do MVP).
namespace PharmaStock.Modelos;

public class ProdutoPreco
{
    public int Id { get; set; }
    public int IdProduto { get; set; }
    public decimal Preco { get; set; }
    public DateTime VigenciaInicio { get; set; }
    public DateTime? VigenciaFim { get; set; }
}
