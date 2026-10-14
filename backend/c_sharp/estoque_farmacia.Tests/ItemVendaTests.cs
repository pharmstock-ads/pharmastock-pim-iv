using estoque_farmacia.Models;
using Xunit; // framework de testes do dotnet

namespace estoque_farmacia.Tests;

public class ItemVendaTests
{
    [Fact] // atributo que diz pro xUnit que o metodo é um teste e executa ele

    public void Subtotal_CalculaPrecoVezesQuantidade()
    {
        var item = new ItemVenda
        {
            IdProduto = 1,
            NomeProduto = "Produto A",
            Quantidade = 3,
            PrecoUnitario = 10.00m
        };

        Assert.Equal(30.00m, item.Subtotal); // compara o resultado e se não bater o teste falha

    }
}


// roda dotnet test e ele executa todos os métodos marcados com [Fact] e reporta quantos passaram e quantos falharam

