// Testes de subtotal do item de entrada.
using PharmaStock.Modelos;

namespace PharmaStock.Tests;

public class EntradaItemTests
{
    [Fact]
    public void Subtotal_UsaPrecoUnitario()
    {
        var item = new EntradaItem
        {
            IdProduto = 1,
            IdLote = 2,
            Quantidade = 10,
            PrecoUnitario = 5.25m
        };

        Assert.Equal(52.50m, item.Subtotal);
    }
}
