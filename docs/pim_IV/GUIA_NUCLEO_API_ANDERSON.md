# Guia — Núcleo C# + API (Anderson)

**Atualizado:** 06/11/2026 (etapa 2 entregue — Dapper + API + Scalar)  
**Papéis:** Líder UNIP · Modelos · Negócio · Repositório · Api  
**Complementares:** [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) · [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) · [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md) · [README.md](../../README.md)

---

## 1. Contexto

Solution: `src/PharmaStock.sln`

```
PharmaStock.Modelos  →  entidades + Dtos/
PharmaStock.Repositorio  →  Dapper (CRUD + SPs)
PharmaStock.Negocio  →  regras FEFO — classes *Negocio
PharmaStock.Api  →  REST + OpenAPI + Scalar
PharmaStock.Web  →  Vinicius (DI já registrada; telas pendentes)
PharmaStock.Tests
```

Controllers só chamam Negócio. Connection string chave **`Padrao`** (ver README).

---

## 2. Decisões que te afetam

| Tema | Decisão |
|------|---------|
| SGBD | SQL Server |
| Acesso (**C1/C2**) | **Dapper** puro; sem EF/Migrations/`HasTrigger` |
| Cadastros (**C3**) | SQL no Repositório |
| `POST /api/lotes` (**C4**) | Fora do MVP |
| FEFO | Negócio (`FefoAlocador`) |
| Auth MVP | `funcionarioId`; sem JWT |
| Flutter A7 | `/api/alertas/lotes-a-vencer` e `/estoque-baixo` |
| Docs API | **Scalar** em `/scalar` |

---

## 3. Status das etapas

| Etapa | Branch | Status |
|-------|--------|--------|
| 1 — Modelos + Negócio + interfaces | `feature/nucleo-modelos-negocio` | ✅ |
| 2 — Dapper + API + Scalar | `feature/nucleo-repo-api` | ✅ (fluxo 2.D validado) |

### Liberar consumidores (feito)

1. Avisar **Vinicius** — Negócio pluggável; guia Web atualizado.  
2. Avisar **Roberto** — base URL + JSON no guia Mobile; Scalar para testar.  
3. **Daniel** — UX em paralelo ([GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md)).

---

## 4. Checklist pessoal

- [x] Modelos = schema (`tabelas.txt`)  
- [x] Interfaces do Repositório  
- [x] Negócio: FEFO + `*Negocio` + testes  
- [x] Repositório Dapper + SPs + transação  
- [x] API: login, cadastros, entrada, venda, alertas A7  
- [x] OpenAPI + Scalar  
- [x] DI Web + Api; controllers só Negócio  
- [ ] Trechos comentados / prints para ABNT  
- [ ] Postagem UNIP / coordenação contínua do grupo  

---

## 5. Dependências

| Você precisa de | De quem |
|-----------------|---------|
| Mudança de assinatura SP | Geovanni (avisar antes) |
| Prioridade / aceite de histórias | Geovanna |
| Telas | Vinicius |
| App | Roberto |
| Protótipos | Daniel |

---

## 6. Referências rápidas

- Setup banco + `Padrao`: [README.md](../../README.md)  
- SPs/triggers: `database/procedure/`, `database/trigger/`  
- Rotas: [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) §7  
- Ágil: [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md)  
- Estudo conexão: `PIM/pessoal/TUTORIAL_CONEXAO_BANCO_ESTUDO.md` (fora do repo)
