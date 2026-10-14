// precisamos do System.Globalization pra usar o CultureInfo na validação do código de barras
using System.Globalization;

// esse arquivo faz parte do namespace de modelos do sistema
namespace estoque_farmacia.Models;

// classe que representa um produto no estoque da farmácia
public class Produto
{
    // id gerado automaticamente pelo service quando salvar
    public int Id { get; set; }

    // nome do medicamento/produto
    public string NomeProduto { get; set; }

    // código de barras — começa vazio pra não ser nulo
    public string CodigoBarras { get; set; } = string.Empty;

    // preço que cobra do cliente
    public decimal PrecoVenda { get; set; }

    // preço que pagou ao fornecedor
    public decimal PrecoCusto { get; set; }

    // qual fornecedor abastece esse produto (chave estrangeira em memória)
    public int IdFornecedor { get; set; }

    // true = precisa de receita médica pra vender
    public bool RequerReceita { get; set; }

    // método estático: pode chamar sem criar um objeto Produto
    // valida se o código de barras é um número inteiro
    public static bool CodigoBarrasValido(string? codigo)
    {
        // se veio vazio ou nulo, já reprova
        if (string.IsNullOrWhiteSpace(codigo))
            return false;

        // tenta converter pra int — se conseguir é válido
        // o "out _" descarta o resultado, só queremos saber se converteu
        return int.TryParse(codigo.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
    }
}
