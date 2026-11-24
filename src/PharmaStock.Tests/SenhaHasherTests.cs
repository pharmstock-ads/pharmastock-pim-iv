// Testes do utilitário BCrypt.
using PharmaStock.Negocio.Seguranca;

namespace PharmaStock.Tests;

public class SenhaHasherTests
{
    [Fact]
    public void Hash_E_Verificar_Ok()
    {
        var hash = SenhaHasher.Hash("SenhaTeste@1");
        Assert.True(SenhaHasher.Verificar("SenhaTeste@1", hash));
    }

    [Fact]
    public void Verificar_SenhaErrada_Falha()
    {
        var hash = SenhaHasher.Hash("SenhaTeste@1");
        Assert.False(SenhaHasher.Verificar("outra", hash));
    }

    [Fact]
    public void Verificar_HashLegadoInvalido_Falha()
    {
        Assert.False(SenhaHasher.Verificar("hash_demo_ana", "hash_demo_ana"));
    }
}
