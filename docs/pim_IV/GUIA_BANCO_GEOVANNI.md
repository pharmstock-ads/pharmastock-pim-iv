# Guia — Banco de dados (Geovanni)

**Para:** Geovanni  
**De:** Anderson (Líder · Núcleo/API)  
**Atualizado:** 06/11/2026  
**Refs:** [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) · [`database/README.md`](../../database/README.md) · [README.md](../../README.md)

Você é a **fonte da verdade** em `database/`. O C# se adapta ao que estiver versionado lá.

**Legenda:** ✅ feito · ⚠️ parcial / atenção · ❌ não feito

---

## Checklist (manual × recomendação)

| # | Item | Status | Obrigatório no manual? | Se falta / por quê (recomendação) |
|---|------|--------|------------------------|----------------------------------|
| — | SQL Server como SGBD | ✅ | **Sim** | — |
| — | Modelo físico (CREATE TABLE, PK/FK) em `tabelas/tabelas.txt` | ✅ | **Sim** | Já atende; `schema.sql` unificado é só organização |
| — | ≥ 1 stored procedure usada pelo sistema | ✅ | **Sim** | Várias `prc_*` + Api/Dapper já chamam |
| — | ≥ 1 trigger | ✅ | **Sim** | `trg_MovimentaItens`, recalcula entrada/venda |
| — | Procedure chamada pelo Repositório (não pela Web/Api) | ✅ | **Sim** | Núcleo etapa 2 fechou isso |
| — | Connection string só em `appsettings` (chave `Padrao`) | ✅ | **Sim** | Cada máquina só ajusta `Server=` |
| B1 | `schema.sql` unificado a partir de `tabelas.txt` | ❌ | **Não** | Facilita subir o banco com 1 arquivo; hoje os `.txt` bastam |
| B2 | Seed com estoque demo (entrada; lote perto de vencer p/ A7) | ⚠️ | **Não** | `inserts.txt` ok, mas lote qtd 0; recomendado p/ demo Flutter/apresentação |
| B3 | Ordem de deploy documentada (schema → SP → trigger → seed) | ⚠️ | **Não** | Evita erro ao rodar do zero; README já descreve a ordem |
| B4 | Lote na entrada via `prc_ObterOuCadastrarLote` | ✅ | **Não*** | *Contrato do time; manual só exige ≥1 SP |
| B5 | Padronizar SPs (`CREATE OR ALTER`, casing) | ❌ | **Não** | Menos atrito ao reaplicar scripts; não muda nota do manual |
| B6 | CHECK/trigger contra estoque negativo | ❌ | **Não** | MVP: Negócio C# já valida; SQL seria reforço |
| B7 | Connection string Dev (`Padrao` + `PharmaStock`) | ⚠️ | **Sim** (string em config) | Padrão fechado; cada PC configura o `Server=` local |
| B8 | Retestar SPs do zero num banco limpo | ⚠️ | **Não** | Confiança na demo; não é entrega formal do manual |
| P10 | Ampliar `senha_hash` (varchar 25) | ❌ | **Não** | Só se evoluir além do hash demo do MVP |
| — | Insumos DER / modelo físico pro ABNT | ⚠️ | **Sim** (doc acadêmico) | Você fornece; Docs redige o texto |

\* B4 é regra do **contrato do grupo**, não do manual.

---

## Resumo

| | |
|--|--|
| **Mínimo do manual (banco)** | ✅ Coberto (tabelas + SP + trigger + uso pelo Repositório + connection string) |
| **Opcional / polish** | B1, B2 (estoque demo), B3, B5, B6, B8, P10 |
| **Onde estão os scripts** | `database/tabelas/` · `database/procedure/` · `database/trigger/` · `database/inserts/` |

Mudança de assinatura de SP → **avisar Anderson antes** (quebra o Repositório).  
Fora do escopo banco: C# / Flutter / Razor / FEFO no Negócio / JWT / desconto.
