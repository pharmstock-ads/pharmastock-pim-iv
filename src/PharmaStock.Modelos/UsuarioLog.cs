// Entidade log de usuario (fora do MVP).
namespace PharmaStock.Modelos;

public class UsuarioLog
{
    public int Id { get; set; }
    public int IdFuncionario { get; set; }
    public DateTime DataHora { get; set; }
    public string Acao { get; set; } = string.Empty;
}
