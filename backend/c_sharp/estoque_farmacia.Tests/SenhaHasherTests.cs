using estoque_farmacia.Services;
using Xunit;

namespace estoque_farmacia.Tests;

public class SenhaHasherTests
{
    [Fact]
    public void HashSha256_MesmaSenha_ProduzMesmoHash()
    {
        var h1 = SenhaHasher.HashSha256("senha123");
        var h2 = SenhaHasher.HashSha256("senha123");
        Assert.Equal(h1, h2);
    }

    [Fact]
    public void HashSha256_SenhasDiferentes_ProduzemHashesDiferentes()
    {
        var h1 = SenhaHasher.HashSha256("abc");
        var h2 = SenhaHasher.HashSha256("xyz");
        Assert.NotEqual(h1, h2);
    }

    [Fact]
    public void HashSha256_Retorna64CaracteresHexadecimais()
    {
        var hash = SenhaHasher.HashSha256("teste");
        Assert.Equal(64, hash.Length);
        Assert.Matches("^[a-f0-9]+$", hash);
    }

    [Fact]
    public void HashSha256_SenhaVazia_RetornaHashDeterministico()
    {
        var esperado = SenhaHasher.HashSha256("");
        Assert.Equal(esperado, SenhaHasher.HashSha256(""));
    }
}
