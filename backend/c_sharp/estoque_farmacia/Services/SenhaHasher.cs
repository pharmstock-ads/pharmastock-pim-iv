using System.Security.Cryptography; // traz o SHA256 algoritmo de hash seguro
using System.Text; // traz o encondingutf8 converte string em bytes


namespace estoque_farmacia.Services;

public static class SenhaHasher
{
    public static string HashSha256(string senha)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        var sb = new StringBuilder();
        foreach (var b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
