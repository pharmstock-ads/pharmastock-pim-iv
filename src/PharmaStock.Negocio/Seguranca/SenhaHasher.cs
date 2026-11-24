// Hash e verificação de senha (BCrypt). Usado só no Negócio.
namespace PharmaStock.Negocio.Seguranca;

public static class SenhaHasher
{
    public static string Hash(string senhaPlana)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(senhaPlana);
        return BCrypt.Net.BCrypt.HashPassword(senhaPlana);
    }

    public static bool Verificar(string senhaPlana, string hashSalvo)
    {
        if (string.IsNullOrWhiteSpace(senhaPlana) || string.IsNullOrWhiteSpace(hashSalvo))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(senhaPlana, hashSalvo);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Hash inválido / legado (ex. hash_demo_*) — trata como credencial inválida.
            return false;
        }
    }
}
