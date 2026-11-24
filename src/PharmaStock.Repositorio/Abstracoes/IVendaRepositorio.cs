// Contrato de venda (SPs). O Negócio chama esta interface; a implementação Dapper é injetada via DI.
namespace PharmaStock.Repositorio.Abstracoes;

public record VendaItemRegistro(
    int IdProduto,
    int IdLote,
    int Quantidade,
    decimal PrecoUnitario);

public interface IVendaRepositorio
{
    Task<int> RegistrarVendaCompletaAsync(
        int idFuncionario,
        IReadOnlyList<VendaItemRegistro> itens,
        CancellationToken ct = default);

    Task<IReadOnlyList<(int IdVenda, int IdFuncionario, DateTime DataVenda, decimal ValorTotal)>> ListarAsync(
        CancellationToken ct = default);
}
