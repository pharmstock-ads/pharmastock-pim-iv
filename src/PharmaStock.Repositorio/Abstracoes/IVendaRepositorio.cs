// Contrato de venda (SPs). O Negócio chama esta interface; a implementação Dapper é injetada via DI.
namespace PharmaStock.Repositorio.Abstracoes;

public record VendaItemRegistro(
    int IdProduto,
    int IdLote,
    int Quantidade,
    decimal PrecoUnitario);

public record VendaListagem(
    int IdVenda,
    int IdFuncionario,
    DateTime DataVenda,
    decimal ValorTotal,
    string FuncionarioNome,
    string Produtos,
    int QuantidadeItens);

public interface IVendaRepositorio
{
    Task<int> RegistrarVendaCompletaAsync(
        int idFuncionario,
        IReadOnlyList<VendaItemRegistro> itens,
        CancellationToken ct = default);

    Task<IReadOnlyList<VendaListagem>> ListarAsync(CancellationToken ct = default);

    Task<VendaListagem?> ObterPorIdAsync(int idVenda, CancellationToken ct = default);
}
