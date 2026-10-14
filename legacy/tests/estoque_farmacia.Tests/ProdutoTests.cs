using estoque_farmacia.Models;
using Xunit;

namespace estoque_farmacia.Tests;

public class ProdutoTests
{
    [Fact]
    public void CodigoBarrasValido_AceitaNumeroInteiro()
    {
        Assert.True(Produto.CodigoBarrasValido("789012"));
        Assert.True(Produto.CodigoBarrasValido("  123  "));
    }

    [Fact]
    public void CodigoBarrasValido_RejeitaVazioOuNaoNumerico()
    {
        Assert.False(Produto.CodigoBarrasValido(null));
        Assert.False(Produto.CodigoBarrasValido(""));
        Assert.False(Produto.CodigoBarrasValido("   "));
        Assert.False(Produto.CodigoBarrasValido("12.5"));
        Assert.False(Produto.CodigoBarrasValido("ABC"));
    }

    [Fact]
    public void Propriedades_PodemSerAtribuidas()
    {
        var p = new Produto
        {
            Id = 1,
            NomeProduto = "Dipirona 500mg",
            CodigoBarras = "789123",
            PrecoVenda = 12.50m,
            PrecoCusto = 8.00m,
            IdFornecedor = 2,
            RequerReceita = true
        };

        Assert.Equal(1, p.Id);
        Assert.Equal("Dipirona 500mg", p.NomeProduto);
        Assert.Equal("789123", p.CodigoBarras);
        Assert.Equal(12.50m, p.PrecoVenda);
        Assert.Equal(8.00m, p.PrecoCusto);
        Assert.Equal(2, p.IdFornecedor);
        Assert.True(p.RequerReceita);
    }
}
