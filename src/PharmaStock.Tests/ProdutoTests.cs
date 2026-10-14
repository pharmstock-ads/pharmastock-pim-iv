using PharmaStock.Modelos;

namespace PharmaStock.Tests;

public class ProdutoTests
{
    [Theory]
    [InlineData("123456")]
    [InlineData("0")]
    [InlineData(" 42 ")]
    public void CodigoBarrasValido_AceitaNumerosInteiros(string codigo)
    {
        Assert.True(Produto.CodigoBarrasValido(codigo));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12A")]
    [InlineData("abc")]
    public void CodigoBarrasValido_RejeitaInvalidos(string? codigo)
    {
        Assert.False(Produto.CodigoBarrasValido(codigo));
    }

    [Fact]
    public void Produto_AtribuiPropriedades()
    {
        var produto = new Produto
        {
            Id = 1,
            Codigo = 100,
            Nome = "Dipirona",
            CodigoBarras = "789",
            IdFornecedor = 2,
            IdCategoria = 1,
            RequerReceita = false
        };

        Assert.Equal(1, produto.Id);
        Assert.Equal("Dipirona", produto.Nome);
        Assert.Equal(2, produto.IdFornecedor);
    }
}
