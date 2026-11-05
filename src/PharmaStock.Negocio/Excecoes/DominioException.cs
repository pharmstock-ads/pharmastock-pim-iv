// Excecoes de regra de negocio.
namespace PharmaStock.Negocio.Excecoes;

public class DominioException : Exception
{
    public DominioException(string message) : base(message) { }
}

public class EstoqueInsuficienteException : DominioException
{
    public EstoqueInsuficienteException(string message) : base(message) { }
}
