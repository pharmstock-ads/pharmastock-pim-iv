# Guia — Núcleo C# + API (Anderson)

**Atualizado:** 06/11/2026 (etapa 2 entregue na branch `feature/nucleo-repo-api`)  
**Papéis:** Líder UNIP · Modelos · Negócio · Repositório · Api  
**Complementares:** [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) · [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) · [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md) · [README.md](../../README.md)

---

## 1. Contexto

Solution: `src/PharmaStock.sln`

```
PharmaStock.Modelos  →  entidades + Dtos/
PharmaStock.Repositorio  →  único acesso ao SQL Server (Dapper: CRUD + SPs)
PharmaStock.Negocio  →  regras (FEFO, validações) — classes *Negocio
PharmaStock.Api  →  REST + OpenAPI + Scalar (Flutter)
PharmaStock.Web  →  Vinicius (só chama Negócio; DI já registrada)
PharmaStock.Tests
```

Regra do manual: **Web e Api não acessam Repositório/DB** nos controllers.

Fonte da verdade: `database/` (Geovanni). Orientação de fluxo: `database/-- Orientações para a camada Service.pas` (Service = **Negócio**).  
Setup local do banco + `Padrao`: ver README do repositório.

---

## 2. Decisões que te afetam

| Tema | Decisão |
|------|---------|
| SGBD | SQL Server |
| Acesso a dados (**C1/C2 FECHADO**) | **Dapper** (CRUD + SPs); **sem** EF Migrations |
| Cadastros (**C3 FECHADO**) | SQL parametrizado no Repositório |
| `POST /api/lotes` (**C4 FECHADO**) | **Fora do MVP** — lote via entrada + `prc_ObterOuCadastrarLote` |
| FIFO/FEFO | No **Negócio** (ordenar lotes por `validade ASC` com qtd > 0) |
| Auth MVP | Login retorna `funcionarioId`; sem JWT por enquanto (A2 adiado) |
| Feature Flutter (API) | Endpoints de **lotes a vencer** e **estoque baixo** (A7) |
| Preço na venda | No DTO da API (sem tabela de preços no MVP) |
| Desconto | Fora do MVP |

---

## 3. Passo a passo de desenvolvimento

### Status das etapas (branches)

| Etapa | Branch | Status |
|-------|--------|--------|
| 1 — Modelos + Negócio + interfaces | `feature/nucleo-modelos-negocio` | **Feito** |
| 2 — Dapper + API + SQL + Scalar | `feature/nucleo-repo-api` | **Feito** (fluxo 2.D validado) |

### Fase A — Alinhar Modelos ao schema — **FEITO**

1. ~~Ler `tabelas.txt` / criar `Entrada`+`EntradaItem`~~ ✅  
2. ~~Ajustar entidades (`PrecoUnitario`, `IdLote` em item venda)~~ ✅  
3. ~~Testes de entidades~~ ✅  

### Fase B — Repositório (SPs + consultas) — **FEITO**

Wrappers (Dapper), um método por SP:

| Método sugerido | SP |
|-----------------|----|
| `ObterOuCadastrarLoteAsync(...)` | `prc_ObterOuCadastrarLote` |
| `RegistrarEntradaAsync(...)` | `prc_RegistrarEntrada` |
| `RegistrarEntradaItemAsync(...)` | `prc_RegistrarEntradaItem` (`@pPrecoUnitario`) |
| `RegistrarVendaAsync(...)` | `prc_RegistrarVenda` |
| `RegistrarVendaItemAsync(...)` | `prc_RegistrarVendaItem` |

Mais CRUDs simples: listar/obter fornecedor, produto, lote, funcionário (login por CPF/e-mail).

Connection string só em `appsettings.json` (Api e Web), chave **`Padrao`**.  
`Server=` muda por máquina (ex. `localhost\SQLEXPRESS` ou `(localdb)\mssqllocaldb`); o código só lê `GetConnectionString("Padrao")`.

### Fase C — Negócio — **FEITO**

No código: `FefoAlocador`, `AuthNegocio`, `CadastroNegocio`, `EntradaNegocio`, `VendaNegocio`, `AlertaNegocio` + testes + Repo Dapper.

### Fase D — API — **FEITO**

Base `/api`, JSON, OpenAPI + **Scalar** em Development (`/scalar`).

| Método | Rota | Notas |
|--------|------|-------|
| POST | `/api/auth/login` | `{ cpfOuEmail, senha }` → `{ funcionarioId, nome, cargo }` |
| GET/POST | `/api/fornecedores` | Cadastro |
| GET/POST | `/api/produtos` | Cadastro / listagem |
| GET | `/api/lotes?produtoId=` | Listagem (**sem** POST lotes — C4) |
| POST | `/api/entradas` | Orquestra Negócio (cria lote se preciso) |
| POST | `/api/vendas` | Sem `loteId` no body do cliente |
| GET | `/api/vendas` · `/api/vendas/{id}` | Histórico |
| GET | `/api/alertas/lotes-a-vencer?dias=` | **A7** Flutter |
| GET | `/api/alertas/estoque-baixo?limite=` | **A7** Flutter |

Controllers finos → só Negócio. DTOs separados das entidades.

### Fase E — Liberar consumidores

Quando login + 1 fluxo (entrada ou venda) + alertas estiverem no Swagger:

1. Avisar **Vinicius** (Web pode espelhar os mesmos serviços de Negócio).  
2. Avisar **Roberto** (base URL + exemplos JSON).  
3. Atualizar checkboxes em [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md).

---

## 4. Checklist pessoal

- [x] Modelos = schema (`tabelas.txt`)  
- [x] Interfaces do Repositório  
- [x] Negócio: FEFO + validações + serviços  
- [x] Testes xUnit (FEFO, Auth, entidades)  
- [ ] Repositório Dapper chama ≥ 1 SP de verdade  
- [ ] API: login, cadastros, entrada, venda, alertas A7  
- [ ] Swagger documentado  
- [ ] Sem referência de Api → Repositorio  
- [ ] Postagem UNIP / coordenação do grupo (líder)

---

## 5. Dependências e bloqueios

| Você precisa de | De quem |
|-----------------|---------|
| `schema.sql` estável + assinaturas SP | Geovanni |
| Prioridade de histórias / aceite | Geovanna |
| Telas | Vinicius (depois do Negócio) |
| App | Roberto (depois da API) |

Se Geovanni mudar nome/parâmetro de SP: atualizar Repositório **no mesmo PR/semana**.

---

## 6. Referências rápidas

- SPs/triggers: `database/procedure/`, `database/trigger/`  
- Inventário: `database/README.md`  
- Rotas detalhadas: [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) §7  
- Relatório Ágil: [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md)
