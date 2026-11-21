# `src/` — solution PharmaStock

Solution .NET do PIM IV (`PharmaStock.sln`).

| Projeto | Papel |
|---------|--------|
| [PharmaStock.Modelos](./PharmaStock.Modelos/) | Entidades + DTOs |
| [PharmaStock.Repositorio](./PharmaStock.Repositorio/) | Dapper + SQL / SPs |
| [PharmaStock.Negocio](./PharmaStock.Negocio/) | Regras (FEFO, auth, entrada, venda, alertas) |
| [PharmaStock.Web](./PharmaStock.Web/) | MVC Razor (browser) |
| [PharmaStock.Api](./PharmaStock.Api/) | REST + Scalar (Flutter) |
| [PharmaStock.Tests](./PharmaStock.Tests/) | xUnit |

```
Web ──┐
      ├──► Negocio ──► Repositorio ──► Modelos
Api ──┘
```

**Pré-requisitos:** .NET 10, SQL Server com banco `PharmaStock` (scripts em [`../database/`](../database/)), connection string chave **`Padrao`** nos `appsettings` da Web e da Api.

Visão geral do repo: [`../README.md`](../README.md).
