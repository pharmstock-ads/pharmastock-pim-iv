// Contrato de entrada de estoque (SPs).
namespace PharmaStock.Repositorio.Abstracoes;

public record EntradaItemRegistro(
    int IdProduto,
    string NumeroLote,
    DateTime Validade,
    decimal Quantidade,
    decimal PrecoUnitario);

public interface IEntradaRepositorio
{
    Task<int> ObterOuCadastrarLoteAsync(
        int idProduto,
        string numeroLote,
        DateTime validade,
        CancellationToken ct = default);

    Task<int> RegistrarEntradaCompletaAsync(
        int idFuncionario,
        IReadOnlyList<EntradaItemRegistro> itens,
        CancellationToken ct = default);
}
