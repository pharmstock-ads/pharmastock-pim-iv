// Testes basicos da entidade Produto.
using PharmaStock.Modelos;

namespace PharmaStock.Tests;

public class ProdutoTests
{
    [Fact]
    public void Produto_AtribuiPropriedades()
    {
        var produto = new Produto
        {
            Id = 1,
            Codigo = 1001,
            Nome = "Dipirona",
            CodBarra = "7891000100028",
            IdFornecedor = 2,
            IdCategoria = 1
        };

        Assert.Equal(1, produto.Id);
        Assert.Equal(1001, produto.Codigo);
        Assert.Equal("Dipirona", produto.Nome);
        Assert.Equal(2, produto.IdFornecedor);
    }
}
