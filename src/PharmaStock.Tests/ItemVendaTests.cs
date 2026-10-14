using PharmaStock.Modelos;

namespace PharmaStock.Tests;

public class ItemVendaTests
{
    [Fact]
    public void Subtotal_E_PrecoUnitario_Vezes_Quantidade()
    {
        var item = new ItemVenda
        {
            IdProduto = 1,
            NomeProduto = "Dipirona",
            Quantidade = 3,
            PrecoUnitario = 10.50m
        };

        Assert.Equal(31.50m, item.Subtotal);
    }
}
