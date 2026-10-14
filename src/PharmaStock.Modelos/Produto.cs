using System.Globalization;

namespace PharmaStock.Modelos;

/// <summary>Medicamento/produto do catálogo. Preços ficam em <see cref="ProdutoPreco"/>.</summary>
public class Produto
{
    public int Id { get; set; }

    /// <summary>Código interno único do produto.</summary>
    public int Codigo { get; set; }

    public string CodigoBarras { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public int? IdCategoria { get; set; }

    public int IdFornecedor { get; set; }

    /// <summary>Indica se a venda exige receita médica.</summary>
    public bool RequerReceita { get; set; }

    /// <summary>Valida se o código de barras é um número inteiro não vazio.</summary>
    public static bool CodigoBarrasValido(string? codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return false;

        return int.TryParse(
            codigo.Trim(),
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out _);
    }
}
