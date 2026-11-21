# PharmaStock.Tests

Testes xUnit do núcleo (FEFO, auth, entidades, etc.).

```bash
cd src
dotnet test PharmaStock.Tests
```

Não sobe Web nem Api. Não precisa do SQL Server para a maior parte dos testes (fakes/repositórios em memória onde aplicável).
