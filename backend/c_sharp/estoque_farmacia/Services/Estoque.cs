namespace estoque_farmacia.Services;

public class Estoque
{
    private readonly LoteService _loteService;

    public Estoque(LoteService loteService)
    {
        _loteService = loteService;
    }

    public decimal ObterDisponivel(int idProduto)
    {
        return _loteService.ObterQuantidadeDisponivel(idProduto);
    }

    public bool TemEstoque(int idProduto, int quantidade)
    {
        return ObterDisponivel(idProduto) >= quantidade;
    }

    public bool Baixar(int idProduto, int quantidade)
    {
        return _loteService.BaixarEstoque(idProduto, quantidade);
    }
}
