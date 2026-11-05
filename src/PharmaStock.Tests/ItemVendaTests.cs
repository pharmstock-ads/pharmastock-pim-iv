// Testes de subtotal do item de venda.
using PharmaStock.Modelos;

namespace PharmaStock.Tests;

public class ItemVendaTests
{
    [Fact]
    public void Subtotal_E_PrecoVezesQuantidade()
    {
        var item = new ItemVenda
        {
            IdProduto = 1,
            IdLote = 3,
            Quantidade = 3,
            PrecoUnitario = 10.50m
        };

        Assert.Equal(31.50m, item.Subtotal);
    }
}
