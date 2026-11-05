// Testes da alocacao FEFO.
using PharmaStock.Modelos;
using PharmaStock.Negocio.Excecoes;
using PharmaStock.Negocio.Fefo;

namespace PharmaStock.Tests;

public class FefoAlocadorTests
{
    [Fact]
    public void UmLoteCobreTudo()
    {
        var lotes = new[]
        {
            new Lote { Id = 1, Validade = new DateTime(2027, 1, 1), Quantidade = 50 }
        };

        var aloc = FefoAlocador.Alocar(lotes, 20);

        Assert.Single(aloc);
        Assert.Equal(1, aloc[0].IdLote);
        Assert.Equal(20, aloc[0].Quantidade);
    }

    [Fact]
    public void DoisLotes_FazSplitPorValidade()
    {
        var lotes = new[]
        {
            new Lote { Id = 10, Validade = new DateTime(2027, 6, 1), Quantidade = 40 },
            new Lote { Id = 20, Validade = new DateTime(2027, 1, 1), Quantidade = 30 }
        };

        var aloc = FefoAlocador.Alocar(lotes, 50);

        Assert.Equal(2, aloc.Count);
        Assert.Equal(20, aloc[0].IdLote);
        Assert.Equal(30, aloc[0].Quantidade);
        Assert.Equal(10, aloc[1].IdLote);
        Assert.Equal(20, aloc[1].Quantidade);
    }

    [Fact]
    public void EstoqueInsuficiente_Lanca()
    {
        var lotes = new[]
        {
            new Lote { Id = 1, Validade = new DateTime(2027, 1, 1), Quantidade = 5 }
        };

        Assert.Throws<EstoqueInsuficienteException>(() => FefoAlocador.Alocar(lotes, 10));
    }

    [Fact]
    public void LoteZerado_EIgnorado()
    {
        var lotes = new[]
        {
            new Lote { Id = 1, Validade = new DateTime(2026, 1, 1), Quantidade = 0 },
            new Lote { Id = 2, Validade = new DateTime(2027, 1, 1), Quantidade = 15 }
        };

        var aloc = FefoAlocador.Alocar(lotes, 10);

        Assert.Single(aloc);
        Assert.Equal(2, aloc[0].IdLote);
        Assert.Equal(10, aloc[0].Quantidade);
    }
}
