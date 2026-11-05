# Guia — Banco de dados (Geovanni)

**Para:** Geovanni  
**De:** Anderson (Líder · Núcleo/API)  
**Atualizado:** 05/11/2026 (após merge `feature/ajustes-database`)  
**Objetivo:** deixar claro o que já está pronto, o que falta e o passo a passo para fechar a entrega de banco do PIM IV.

**Contexto geral:** [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) · [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md)

---

## 1. Seu papel

Você é a **fonte da verdade dos dados**. O C# (Anderson) se adapta ao que você versionar em `database/` — não o contrário.

| Entrega | Obrigatório no manual? |
|---------|------------------------|
| Modelo físico (CREATE TABLE, PK/FK, CHECK) | Sim |
| ≥ 1 **stored procedure** usada pelo sistema | Sim |
| ≥ 1 **trigger** | Sim |
| Seed de demonstração | Forte recomendação (demo/apresentação) |
| Texto DER + físico no ABNT | Sim (você fornece insumos; Docs redige) |

---

## 2. O que já está no repo (não jogar fora)

```
database/
├── tabelas/tabelas.txt              ← CREATE TABLE (base do schema)
├── inserts/inserts.txt              ← seed parcial (fornecedor → lote qtd 0)
├── procedure/
│   ├── prc_ObterOuCadastrarLote.txt
│   ├── prc_RegistrarEntrada.txt
│   ├── prc_RegistrarEntradaItem.txt   ← @pPrecoUnitario (não mais custo)
│   ├── prc_RegistrarVenda.txt
│   ├── prc_RegistrarVendaItem.txt
│   └── prc_MovimentaItens.txt
├── trigger/
│   ├── trg_MovimentaItens.txt
│   ├── trg_RecalculaEntrada.txt       ← usa preco_unitario + COALESCE(...,0)
│   └── trg_RecalculaVenda.txt
├── -- Orientações para a camada Service.pas   ← regras p/ Negócio C#
├── schema.sql / seed.sql / procedures.sql / triggers.sql  ← placeholders
└── README.md
```

Tabelas cobertas: `fornecedor`, `funcionario`, `categoria`, `produto`, `lote`, `entrada`/`entradaitem`, `venda`/`vendaitem`, `movimentacao`.

**Mudança recente (merge):** coluna `entradaitem.custo_unitario` virou **`preco_unitario`**; SP/trigger alinhadas.

---

## 3. Decisões que você deve respeitar

| Tema | Decisão |
|------|---------|
| SGBD | **SQL Server** apenas |
| Quem escolhe lote na venda (FIFO) | **Negócio C#** — SPs recebem `@pIdLote` pronto |
| Quem atualiza `lote.quantidade` | **Trigger** (`trg_MovimentaItens`) |
| Lote na entrada | Usar **`prc_ObterOuCadastrarLote`** (não INSERT solto de lote no app) |
| Preço de venda no MVP | Vem no DTO/parâmetro da venda (sem tabela `produtos_precos` agora) |
| Desconto / JWT | Fora do MVP — não bloquear schema por isso |

---

## 4. Passo a passo (ordem recomendada)

### Passo 1 — Unificar o schema

1. Copiar/organizar o conteúdo de `tabelas/tabelas.txt` para **`database/schema.sql`**.
2. Garantir ordem de criação respeitando FKs (fornecedor/categoria → produto → lote → documentos → movimentacao).
3. Homogeneizar:
   - `CREATE` vs `CREATE OR ALTER` nas SPs  
   - casing (`lote` vs `Lote`) — escolher **um** e manter  
   - `GO` entre batches  
4. Rodar o script do zero num banco limpo (LocalDB / SQL Express / Docker) e anotar erros.

### Passo 2 — Revisar pontos técnicos conhecidos

| Ponto | Sugestão |
|-------|----------|
| `funcionario.senha_hash varchar(25)` | Curto demais para SHA-256 hex (64) ou Base64. Ampliar (ex.: `varchar(128)` ou `varbinary`) |
| `quantidade` misturando `int` e `decimal` | Padronizar (preferência: `decimal` no lote; alinhar itens) |
| `prc_ObterOuCadastrarLote` — `@pNumeroLote VARCHAR(10)` vs coluna `varchar(50)` | Alinhar tamanhos |
| Unique de lote | Considerar UNIQUE `(id_produto, numero_lote)` se ainda não houver |
| `ativo` CHECK `('s','n')` vs inserts com `'S'` | Em collation CI costuma passar; preferir minúsculo no seed para bater o CHECK literal |
| Seed: lotes com `quantidade = 0` | Correto (estoque só via movimentação). Falta rodar **entrada** de demo (ou script extra) para Flutter A7 |

### Passo 3 — Versionar procedures e triggers “oficiais”

1. Manter os `.txt` por procedure **ou** consolidar em `procedures.sql` / `triggers.sql` (o importante é o Anderson saber o que executar).
2. Atualizar `database/README.md` com a lista final + parâmetros (INPUT/OUTPUT).
3. Testar no SSMS o fluxo completo:

```
-- Entrada
prc_ObterOuCadastrarLote → prc_RegistrarEntrada → prc_RegistrarEntradaItem
-- Conferir lote.quantidade e entrada.valor_total

-- Venda (lote já existente com estoque)
prc_RegistrarVenda → prc_RegistrarVendaItem
-- Conferir baixa no lote e venda.valor_total
```

### Passo 4 — Seed (já começou)

Já existe **`database/inserts/inserts.txt`**:

| Bloco | Conteúdo |
|-------|----------|
| Fornecedor | 3 distribuidoras |
| Funcionario | Ana, Carlos, Mariana (`senha_hash` demo) |
| Categoria | Analgesicos, Antibioticos, Vitaminas, Higiene |
| Produto | 1001–1005 (Paracetamol, Dipirona, Amox, Vit C, Sabonete) |
| Lote | 5 lotes com validade 2027–2028 e **quantidade 0** |

Ainda falta (recomendado):

1. Copiar/renomear inserts → `seed.sql` (ou documentar ordem: tabelas → SPs → triggers → inserts).  
2. Script de **entrada de demo** (via SPs) para popular estoque.  
3. Pelo menos **1 lote com validade próxima** (ex.: +15 dias) e **1 com pouca qtd** após entrada — senão o app Flutter (A7) fica vazio na apresentação.

### Passo 5 — Entregar para o núcleo

Avisar Anderson quando:

- [ ] `schema.sql` sobe limpo do zero  
- [ ] Todas as SPs/triggers aplicam sem erro  
- [ ] Inserts/seed rodam (e, se possível, entrada demo)  
- [ ] README atualizado (assinaturas das SPs — `@pPrecoUnitario` na entrada)

---

## 5. O que o Anderson vai consumir (contrato mínimo)

| Procedure | Uso no C# |
|-----------|-----------|
| `prc_ObterOuCadastrarLote` | Entrada: resolve `id_lote` |
| `prc_RegistrarEntrada` | Abre cabeçalho → `@pIdEntrada OUTPUT` |
| `prc_RegistrarEntradaItem` | Item + mov. tipo `'e'` (`@pPrecoUnitario`) |
| `prc_RegistrarVenda` | Abre cabeçalho → `@pIdVenda OUTPUT` |
| `prc_RegistrarVendaItem` | Item + mov. tipo `'s'` (já com lote escolhido no Negócio) |
| `prc_MovimentaItens` | Preferencialmente só chamada **pelas** SPs acima |

Consultas de cadastro (listar produto, fornecedor, lotes a vencer, estoque baixo) podem ser SQL parametrizado no Repositório — não precisam ser SP, a menos que você queira.

---

## 6. Critério de pronto (Definition of Done — banco)

- Script completo documentado e versionado  
- Trigger + procedure demonstráveis na apresentação  
- Seed para demo  
- Sem quebrar FKs nos fluxos de entrada/venda  
- README da pasta `database/` coerente com os arquivos reais  

---

## 7. Fora do seu escopo agora

- Código C#, Flutter, Razor  
- Escolher lote FIFO (é Negócio)  
- JWT, desconto, ML  

Dúvida de regra de negócio → alinhar com Anderson + Geovanna (Ágil). Mudança de assinatura de SP → **avisar Anderson antes** (quebra o Repositório).
