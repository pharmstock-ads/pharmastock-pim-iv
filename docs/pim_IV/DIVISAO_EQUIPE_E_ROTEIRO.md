# PharmaStock — Divisão de equipe e roteiro de execução (PIM IV)

Documento interno do grupo. Alinha as **10 etapas do Manual do PIM IV** com a divisão de papéis, a ordem de trabalho e o checklist ponto a ponto de cada seção.


---

## 1. Visão geral dos papéis (7 pessoas)

**Líder na plataforma UNIP:** Anderson (05/11/2026).

Guias operacionais por pessoa: ver [README.md](./README.md) (pasta `docs/pim_IV`).

| # | Papel | Responsável | Etapas principais | Pastas / projetos |
|---|--------|-------------|-------------------|-------------------|
| 1 | **Núcleo C# + API** (+ líder UNIP) | Anderson | Etapa 4 (Modelos, Negócio, Repositório, Api) | `src/PharmaStock.Modelos`, `Negocio`, `Repositorio`, `Api` |
| 2 | **Banco de dados** | Geovanni | Etapa 3 | `database/` (SQL Server) |
| 3 | **Web** | Vinicius | Etapa 4 (Web) + Etapa 5 | `src/PharmaStock.Web` |
| 4 | **Mobile** | Roberto | Etapa 9 — Flutter: **lotes a vencer + estoque baixo** | `mobile/` |
| 5 | **Engenharia de Software Ágil** | Geovanna | Etapa 1 (apoio) + Etapa 2 | backlog, board, `docs/pim_IV/` (planejamento) |
| 6 | **Documentação acadêmica** | _a definir_ | Etapa 10 + montagem ABNT | `docs/pim_IV/` (documento final) |
| 7 | **UX / UI** | Daniel | Etapa 6 | `uiux/` · [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md) |

### Fora de papel dedicado

| Tema | Como tratar |
|------|-------------|
| Machine Learning / Análise de dados (Etapa 7) | Seção **curta** no documento; redator: Documentação; insumos: Banco + Núcleo |
| Comunicação / LIBRAS (Etapa 8) | Seção **curta** no documento; redator: Documentação; input do time |

---

## 2. Ordem macro de execução (obrigatória)

Não começar pela API. A API depende de requisitos, modelo de dados e regras mínimas.

```
[1] Negócio + Ágil          →  Etapas 1 e 2
[2] Banco + Modelos         →  Etapa 3 + início da Etapa 4 (Modelos)
[3] Negócio + Repositório   →  Etapa 4 (núcleo)
[4] API (contratos REST)    →  Etapa 4 (Api)
[5] Web e Mobile em paralelo→  Etapas 4/5 e 9
[6] UX acompanha desde cedo →  Etapa 6 (não bloqueia o núcleo)
[7] Integração + documento  →  Etapa 10 + ABNT
```

### Quem dá o pontapé

1. **Ágil** convoca kickoff (negócio fictício + backlog).
2. Em seguida, **Banco + Núcleo C#** fecham entidades/tabelas.
3. **API** só depois de Modelos/Negócio/Repositório mínimos estáveis.
4. **Web** e **Mobile** quando existirem endpoints (ou serviços de Negócio) utilizáveis.
5. **Documentação** escreve ao longo do semestre e consolida no fim.
6. **UX** pode iniciar wireframes logo após o backlog prioritário.

---

## 3. Dependências entre papéis

```
Ágil ──requisitos──► Banco + Núcleo C#
Banco ──SQL/proc───► Repositório (Núcleo)
Núcleo ──Negócio───► Web
Núcleo ──API───────► Mobile
UX ────telas───────► Web + Mobile
Todos ─artefatos───► Documentação
```

**Regra de ouro:** Web e Api **não** acessam banco nem Repositório diretamente. Tudo passa por Negócio.

```
Web ──┐
      ├──► Negocio ──► Repositorio ──► Modelos
Api ──┘                    │
                           └──► SQL Server (trigger / procedure)
```

---

## 4. Checklist ponto a ponto por etapa do manual

Atualize a coluna **Status**:

| Emoji | Significado |
|-------|-------------|
| ✅ | Feito |
| ⚠️ | Parcial / em andamento / bloqueado |
| ❌ | Não feito |

Progresso detalhado por bloco de código: [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md).

### Visão rápida das etapas

| Etapa | Dono | Status |
|-------|------|--------|
| 1 — Negócio fictício | Geovanna (+ Docs) | ⚠️ |
| 2 — Eng. Software Ágil | Geovanna | ⚠️ |
| 3 — Banco de Dados | Geovanni | ⚠️ |
| 4 — C# (núcleo / API / Web) | Anderson / Vinicius | ⚠️ (núcleo+API ✅; Web MVC ❌) |
| 5 — Web responsivo | Vinicius | ❌ |
| 6 — UX/UI | Daniel | ⚠️ |
| 7 — ML / dados (curta) | Documentação | ❌ |
| 8 — Comunicação / LIBRAS (curta) | Documentação | ❌ |
| 9 — Mobile Flutter | Roberto | ❌ |
| 10 — Integração final | Documentação + grupo | ❌ |

---

### Etapa 1 — Definição e caracterização do negócio fictício

**Dono do texto:** Ágil (Geovanna) + Documentação · **Participam:** grupo no kickoff · **Status:** ⚠️  
**Base:** [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md)

| Item | Status |
|------|--------|
| Denominação da empresa (PharmaStock / farmácia) | ⚠️ (decidido; falta texto formal ABNT) |
| Segmento de atuação | ❌ |
| Público-alvo | ❌ |
| Produtos/serviços oferecidos | ❌ |
| Problemas que justificam o sistema (estoque, validade, vendas) | ⚠️ (implícito no MVP/RF; falta prosa) |
| Registrar continuidade do PIM III (citar no doc) | ⚠️ (citado no relatório; falta cap. ABNT) |
| 1–2 páginas para introdução/contexto | ❌ |

**Critério de pronto:** qualquer integrante explica o negócio em 1 minuto.

---

### Etapa 2 — Engenharia de Software Ágil Aplicada

**Dono:** Geovanna · **Apoiam:** Núcleo, UX, Web/Mobile · **Status:** ⚠️  
**Base:** [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) §4

| Item | Status |
|------|--------|
| Stakeholders / atores | ⚠️ (atores §4.1; falta lista completa de stakeholders) |
| Requisitos funcionais (RF) | ⚠️ (RF01–RF22 no relatório; falta prosa/board) |
| Requisitos não funcionais (RNF) | ⚠️ (RNF01–RNF16 no relatório; falta prosa/board) |
| Backlog priorizado | ⚠️ (MVP §3 + prioridade Must/Should/Could; falta board) |
| Iterações/sprints (mín. 2–3) | ❌ |
| Critérios de aceite (alta prioridade) | ⚠️ (na tabela RF; falta por sprint) |
| Board Trello ([pim-iv](https://trello.com/b/9ifnCGAL/pim-iv)) | ⚠️ (quadro + cards; falta sprints/aceite) |
| Artefatos para o doc (prints, RF/RNF, sprints) | ❌ |

**MVP sugerido (alta prioridade):** login → fornecedor/produto → entrada de lote → venda FIFO → listagens web → feature Flutter.

**Critério de pronto:** top 5 do backlog claros para Banco e Núcleo.

---

### Etapa 3 — Modelagem de Banco de Dados

**Dono:** Geovanni · **SGBD:** SQL Server · **Status:** ⚠️

| Item | Status |
|------|--------|
| Informações essenciais (≥ 5 entidades) | ⚠️ |
| Modelo conceitual (DER) | ❌ |
| Modelo lógico (PK/FK) | ❌ |
| Modelo físico (`tabelas.txt` → `schema.sql`) | ⚠️ |
| ≥ 1 trigger (`trg_MovimentaItens`, `trg_RecalculaEntrada`, `trg_RecalculaVenda`) | ✅ |
| ≥ 1 stored procedure (`prc_RegistrarEntrada*`, `prc_RegistrarVenda*`, `prc_MovimentaItens`) | ✅ |
| Seed (`inserts/inserts.txt`; falta estoque demo) | ⚠️ |
| Justificativa técnica (texto ABNT) | ❌ |
| Principais operações/consultas descritas | ❌ |
| Prints/diagramas para Documentação | ❌ |

**Contrato das SPs:** [`database/README.md`](../../database/README.md).  
Fluxo: abrir entrada/venda → itens → SP grava `movimentacao` → trigger atualiza `lote` e totais.

**Obs. manual:** connection string só em `appsettings.json` (chave `Padrao`); procedure via Repositório. `HasTrigger` **não se aplica** (Dapper puro, sem EF).

**Critério de pronto:** schema aplicável; SPs/triggers testáveis; Núcleo mapeia entidades.

---

### Etapa 4 — Desenvolvimento C# (POO, MVC e Web API)

**Status geral:** ⚠️ (núcleo+API ✅ 06/11; Web MVC ainda ❌)

#### 4.A — Núcleo (Modelos + Negócio + Repositório) — Anderson

| Item | Status |
|------|--------|
| Classes em `PharmaStock.Modelos` (alinhadas a `tabelas.txt`) | ✅ |
| `Entrada` / `EntradaItem` / FEFO no Negócio | ✅ |
| Interfaces do Repositório (`Abstracoes/`) | ✅ |
| Acesso a dados (Dapper) | ✅ |
| Executar stored procedure pelo Repositório | ✅ |
| Regras em `PharmaStock.Negocio` (`*Negocio`) | ✅ |
| Encapsulamento / herança / polimorfismo (evidenciar no doc) | ❌ (ABNT) |
| DI nos `Program.cs` (Web e Api) | ✅ |
| Controllers só Negócio (Repo só no `Program.cs` para DI) | ✅ |
| Testes básicos (`PharmaStock.Tests`) | ✅ |
| Trechos comentados para o doc (repo + negócio) | ❌ (ABNT) |

**Critério de pronto:** cadastro + fluxo entrada/venda via Negócio+Dapper — **atendido**.

#### 4.B — API — Anderson

| Item | Status |
|------|--------|
| Controllers REST | ✅ |
| DTOs em `Modelos/Dtos` | ✅ |
| Verbos HTTP / status codes | ✅ |
| OpenAPI + **Scalar** | ✅ |
| Endpoints alinhados ao Mobile (A7) | ✅ |
| Autenticação mínima do MVP | ✅ |
| Trecho de controller para o doc | ❌ (ABNT) |
| Lista de rotas para Mobile e Docs | ✅ (contrato §7 + Scalar) |

**Critério de pronto:** Mobile consome ≥ 1 fluxo via API — **API pronta**; app Flutter ainda ❌.

#### 4.C — Web (MVC) — Vinicius

| Item | Status |
|------|--------|
| Controllers MVC finos | ❌ |
| Views Razor + layouts + partials | ❌ |
| ViewModels | ❌ |
| Integração com Negócio (sem SQL na UI) | ❌ |
| Trecho de controller para o doc | ❌ |
| Print da solution (5+ projetos) | ❌ |

**Critério de pronto:** telas prioritárias do backlog web funcionam.

#### Artefatos da Etapa 4 (Documentação)

| Item | Status |
|------|--------|
| Descrição da arquitetura em camadas | ❌ |
| Diagrama Web/Api → Negócio → Repositório → SQL Server + Flutter | ❌ |
| Trechos: repositório, negócio, MVC, API, chamada da procedure | ❌ |

---

### Etapa 5 — Desenvolvimento Web Responsivo

**Dono:** Vinicius · **Status:** ❌

| Item | Status |
|------|--------|
| Layout responsivo (desktop / tablet / mobile web) | ❌ |
| Consistência visual (Blue-Pharma / protótipos) | ❌ |
| Formulários e listagens em tela estreita | ❌ |
| Integração Razor/CSS com Negócio | ❌ |
| Capturas em resoluções diferentes | ❌ |

**Critério de pronto:** telas prioritárias não quebram em viewport estreita.

---

### Etapa 6 — UX e UI Design

**Dono:** Daniel · **Status:** ⚠️ · [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md)

| Item | Status |
|------|--------|
| Personas (mín. 2) | ❌ |
| Fluxos principais | ❌ |
| Wireframes / protótipos em `uiux/` | ⚠️ |
| Análise de usabilidade + melhorias | ❌ |
| PNGs/Figma + texto para Documentação | ⚠️ |

**Critério de pronto:** Web e Mobile têm referência visual clara.

---

### Etapa 7 — Machine Learning e Análise de Dados _(seção curta)_

**Dono do texto:** Documentação · **Status:** ❌

| Item | Status |
|------|--------|
| Dados relevantes (estoque baixo, validade, vendas) | ❌ |
| Análise simples / ML básico | ❌ |
| Indicador/relatório interpretável | ❌ |
| ½–1 página no documento | ❌ |

Fazer depois que houver seed/dados reais.

---

### Etapa 8 — Comunicação, Liderança, Negociação e LIBRAS _(seção curta)_

**Dono do texto:** Documentação · **Status:** ❌

| Item | Status |
|------|--------|
| Comunicação do time (reuniões, board, GitHub) | ❌ |
| 1 situação de liderança/negociação | ❌ |
| Proposta de acessibilidade / LIBRAS | ❌ |
| ½–1 página no documento | ❌ |

---

### Etapa 9 — Desenvolvimento Mobile (Flutter)

**Dono:** Roberto · **Depende de:** API ✅ (pronta) · **Status:** ❌ (app ainda não criado)

| Item | Status |
|------|--------|
| App Flutter em `mobile/` | ❌ |
| Telas adaptadas a mobile | ❌ |
| Consumir Web API (REST) | ❌ |
| ≥ 1 funcionalidade complementar ao web | ❌ |
| Erros de rede / loading | ❌ |
| Capturas para Documentação | ❌ |
| Documentar endpoints usados | ❌ |

**Sugestões de feature:** consulta estoque/validade, alerta de lote, movimentação simples, dashboard resumido.

**Critério de pronto:** demo do app falando com a API.

---

### Etapa 10 — Integração e avaliação final

**Dono:** Documentação + grupo · **Status:** ❌

| Item | Status |
|------|--------|
| Integrar Web + API + Banco + Mobile | ❌ |
| Checklist RF (feito / parcial / não feito) | ❌ |
| Avaliação vs objetivos do negócio | ❌ |
| Limitações técnicas/conceituais | ❌ |
| Melhorias / evolução futura | ❌ |
| Ensaio de apresentação | ❌ |

**Critério de pronto:** caminho feliz ponta a ponta (produto/lote → venda → consulta no app).

---

## 5. Documento acadêmico (ABNT)

**Dono:** Documentação · **Status:** ❌

| Item | Status |
|------|--------|
| Capa | ❌ |
| Resumo + palavras-chave | ❌ |
| Abstract + keywords | ❌ |
| Sumário | ❌ |
| Introdução | ❌ |
| Desenvolvimento (20–30 páginas) | ❌ |
| Conclusão | ❌ |
| Referências (ABNT) | ❌ |

**Regras:** citar PIM III; só o **líder** posta na UNIP; prazo/canal oficiais.  
**Pasta:** `docs/pim_IV/`.

---

## 6. Roteiro por fase no tempo (sugerido)

| Fase | Quando | Foco | Status |
|------|--------|------|--------|
| A — Fundação | sem. 1–2 | Kickoff Ágil, entidades, esqueleto do doc | ⚠️ |
| B — Dados e núcleo | sem. 3–5 | Schema/seed, Modelos ✅, Negócio ✅, Repo Dapper | ⚠️ |
| C — Interfaces | sem. 5–8 | API, Web, Mobile, UX | ❌ |
| D — Fechamento | sem. 8–fim | Integração, ABNT, postagem | ❌ |

### Detalhe por fase

| Fase | Quem | O que fazer | Status |
|------|------|-------------|--------|
| A | Geovanna | Kickoff, RF/RNF, backlog v1, board | ⚠️ (RF/RNF no relatório; falta board/sprints) |
| A | Daniel (UX) | Personas + fluxos + wireframes | ⚠️ |
| A | Geovanni + Anderson | Lista de entidades alinhada | ✅ |
| A | Docs | Esqueleto do documento | ❌ |
| B | Geovanni | Schema, seed (+ SPs/triggers já ✅) | ⚠️ |
| B | Anderson | Modelos, Negócio, Repositório, testes | ✅ |
| B | Geovanna | Refinar backlog e aceite | ❌ |
| B | Daniel (UX) | Protótipos alta fidelidade | ⚠️ |
| B | Docs | Caps. negócio / ágil / banco | ❌ |
| C | Anderson | API + Scalar | ✅ |
| C | Vinicius | MVC + responsivo | ❌ |
| C | Roberto | Flutter + A7 | ❌ |
| C | Daniel (UX) | Ajustes de usabilidade | ❌ |
| C | Docs | Capturas, diagramas, trechos | ❌ |
| D | Todos | Integração ponta a ponta | ❌ |
| D | Docs | Etapas 7–8, 10, PDF final | ❌ |
| D | Geovanna | Retrospectiva + evidências | ❌ |
| D | Líder | Postagem UNIP | ❌ |

---

## 7. Definição de pronto por papel

| Papel | Quem | “Pronto” significa | Status |
|-------|------|--------------------|--------|
| Ágil | Geovanna | Backlog, sprints e aceite documentados | ⚠️ (baseline RF/RNF ✅; board/sprints ❌) |
| Banco | Geovanni | Schema + seed + SPs/triggers + diagramas | ⚠️ (SP/trigger ✅; schema.sql/DER ❌) |
| Núcleo + API | Anderson | Camadas + fluxo + Scalar | ✅ |
| Web | Vinicius | Telas prioritárias responsivas | ❌ |
| Mobile | Roberto | App Flutter com ≥ 1 feature via API | ❌ |
| UX | Daniel | Personas, fluxos e protótipos usados pelo time | ⚠️ |
| Documentação | _a definir_ | Doc ABNT completo com evidências | ❌ |

---

## 8. Cadastro dos nomes

| Papel | Nome | RA | Status |
|-------|------|-----|--------|
| Núcleo C# + API | Anderson | | ✅ |
| Banco de dados | Geovanni | | ✅ |
| Web | Vinicius | | ✅ |
| Mobile | Roberto | | ✅ |
| Eng. Software Ágil | Geovanna | | ✅ |
| Documentação | | | ❌ |
| UX / UI | Daniel | | ✅ |
| Líder (postagem UNIP) | Anderson | | ✅ |

---

## 9. Referências internas do repositório

| Caminho | Uso |
|---------|-----|
| `docs/manual/Manual_PIM_IV_2026.docx` | Manual oficial |
| `docs/pim_IV/ORDEM_DESENVOLVIMENTO.md` | Ordem dos blocos + status visual |
| `docs/pim_III/` | Material do semestre passado |
| `legacy/` | Console, WinForms, HTML e PG do III |
| `src/PharmaStock.sln` | Solution do IV |
| `database/` | Scripts SQL Server |
| `mobile/` | App Flutter |
| `uiux/` | Casos de uso e protótipos |

---

*Documento vivo: atualizar a coluna Status (✅ / ⚠️ / ❌) conforme as sprints.*
