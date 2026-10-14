# Relatório de decisões — Kickoff PharmaStock (PIM IV)

**Para:** Geovanna (Engenharia de Software Ágil)  
**De:** Anderson (Núcleo C# + API)  
**Data:** 14/10/2026  
**Objetivo:** registrar de onde o projeto parte, o que já foi decidido e o que ainda precisa de alinhamento — base para backlog, RF/RNF e planejamento das sprints.

**Documento complementar:** [DIVISAO_EQUIPE_E_ROTEIRO.md](./DIVISAO_EQUIPE_E_ROTEIRO.md)

---

## 1. Contexto

O PIM IV é a **continuidade** do sistema PharmaStock iniciado no PIM III ([repositório do III](https://github.com/pimIII/pimIII); trabalho atual em [pharmastock-pim-iv](https://github.com/pharmstock-ads/pharmastock-pim-iv)).

No III entregamos:

- Domínio de farmácia (produto, fornecedor, lote, venda, funcionário)
- Apps Console + WinForms (persistência **em memória**)
- Scripts PostgreSQL de **referência** (não conectados à aplicação)
- Protótipo HTML e artefatos de UX

No IV o manual exige, em resumo:

- Solution em camadas: **Modelos → Repositório → Negócio → Web (MVC) + Api**
- Banco relacional com **trigger + stored procedure** (procedure chamada pelo sistema)
- App **Flutter** consumindo a API
- Documento ABNT cobrindo as etapas do manual

Código legado do III está em `legacy/` (não é a entrega principal). A solution nova está em `src/PharmaStock.sln`.

---

## 2. Decisões já tomadas (kickoff técnico)

| # | Tema | Decisão | Status |
|---|------|--------|--------|
| D1 | Negócio fictício | **Continuar PharmaStock / farmácia** (mesmo domínio do PIM III) | **Confirmado** |
| D2 | Escopo MVP funcional | Seguir a sugestão de fluxos prioritários (ver §3) | **Confirmado** |
| D3 | Papel Anderson | **Núcleo C#** (Modelos, Negócio, Repositório) **+ API** | **Confirmado** |
| D4 | Papel Geovanna | **Engenharia de Software Ágil** (Etapas 1–2 do manual: negócio no texto ágil, stakeholders, RF/RNF, backlog, sprints, aceite) | **Confirmado** |
| D5 | Banco (SGBD) | Preferência informal: **PostgreSQL**, reaproveitando o modelo do PIM III | **Pendente de validação** — ver §4 |
| D6 | Detalhes de modelagem (trigger, procedure, campos finais) | Alinhar com o(a) dev de banco | **Pendente** |

---

## 3. MVP funcional sugerido (partimos daqui)

Prioridade para o primeiro ciclo de entrega (ajuste fino cabe a você no backlog):

1. **Autenticação / funcionário** (login)
2. **Cadastro de fornecedor e produto**
3. **Entrada de lote** (estoque com validade)
4. **Venda com baixa de estoque FIFO por validade** (regra já existente no legado)
5. **Listagens/consultas essenciais na Web**
6. **Uma funcionalidade complementar no app Flutter** (a definir com Mobile; exemplos: consulta de estoque/validade, alerta de lote crítico)

Isso deve virar itens do backlog com critérios de aceite (sua Etapa 2).

---

## 4. Banco de dados — atenção importante

### O que o Anderson entendeu / propôs

- Dá para **adaptar o modelo do PIM III** (entidades e relacionamentos em `legacy/database-postgresql/`).
- Há menção de seguir com **PostgreSQL**.

### O que o Manual do PIM IV pede (texto oficial)

O manual deste semestre cita explicitamente **SQL Server** como banco do projeto, com trigger e procedure integradas à aplicação (camada Repositório).

### Implicação para o planejamento (Geovanna)

| Opção | Prós | Contras |
|-------|------|---------|
| **A — SQL Server (conforme manual)** | Alinhado à rubrica; menos risco na correção | Scripts do III precisam ser **convertidos** (tipos, serial→IDENTITY, etc.) |
| **B — PostgreSQL (preferência do time)** | Reaproveita quase direto o SQL do III | **Pode divergir do manual**; só seguir se o professor/coordenador **autorizar por escrito** |

**Recomendação de processo:**

1. Você (Ágil) + dono(a) do banco confirmam com o professor: *“Podemos usar PostgreSQL ou é obrigatório SQL Server?”*
2. Enquanto isso, o domínio (entidades) permanece o mesmo — Anderson pode avançar em **Modelos** sem travar no SGBD.
3. Trigger e procedure ficam a cargo do banco; Anderson só precisa do contrato (nome da procedure, parâmetros, tabelas afetadas) para chamar no Repositório.

**Entidades herdadas do III (ponto de partida do DER):**

- `fornecedor`, `funcionario`, `usuario_log`, `categoria`
- `produto`, `produtos_precos`, `lote`, `movimentacao`
- No código legado também há conceito de **Venda / ItemVenda** (vale unificar com movimentação ou modelar tabela de venda no IV — decisão com o banco)

---

## 5. Divisão de papéis (visão para o backlog)

| Papel | Responsável | O que entrega para o Ágil “ver” |
|-------|-------------|-------------------------------|
| Núcleo C# + API | Anderson | Camadas + endpoints REST (Swagger) |
| Banco | _a confirmar com o outro dev_ | Schema, trigger, procedure, seed |
| Web (MVC + responsivo) | _a definir_ | Telas Razor |
| Mobile (Flutter) | _a definir_ | App consumindo a API |
| Eng. Software Ágil | **Geovanna** | RF/RNF, backlog, sprints, aceite |
| Documentação ABNT | _a definir_ | Documento final |
| UX/UI | _a definir_ | Personas, fluxos, protótipos |

**ML e LIBRAS:** não terão dono dedicado; vão como seções curtas no documento acadêmico (Documentação), se o professor cobrar.

> Lembrete: manual limita grupo a **até 6** na postagem UNIP — validar se os 7 entram ou se alguém fica de fora na plataforma.

---

## 6. Ordem de trabalho acordada (para você montar as sprints)

```
[1] Negócio + Ágil (Geovanna lidera o kickoff formal)
[2] Banco + Modelos (Anderson) em paralelo
[3] Negócio + Repositório (Anderson)
[4] API (Anderson) — contratos para Mobile
[5] Web e Mobile em paralelo
[6] UX desde cedo (não bloqueia o núcleo)
[7] Integração + documento final
```

**Pontapé de processo:** Geovanna (reunião de negócio + backlog).  
**Pontapé técnico de domínio:** Anderson + Banco (entidades/FKs).  
**API não é o primeiro passo de código.**

---

## 7. O que a Geovanna pode fazer já com este relatório

Checklist inicial sugerido (Etapas 1 e 2):

- [ ] Formalizar no board a decisão **D1** (PharmaStock / farmácia) e redigir o texto curto do negócio fictício
- [ ] Transformar o MVP do §3 em **backlog priorizado** (IDs, descrição, aceite)
- [ ] Listar **stakeholders/atores** (ex.: Administrador, Atendente, sistema mobile)
- [ ] Separar **RF** e **RNF** (ex.: responsividade, hash de senha, disponibilidade local)
- [ ] Propor **2–3 sprints** com o que entra em cada uma
- [ ] Agendar alinhamento **Banco + Anderson** (SGBD oficial + trigger/procedure + venda vs movimentação)
- [ ] Confirmar com o coordenador/professor a questão **PostgreSQL vs SQL Server** e o **limite de 6 integrantes**
- [ ] Preencher nomes/RAs na tabela da divisão (doc de roteiro)

---

## 8. Próximos passos do Anderson (transparência)

Enquanto Ágil e Banco fecham o formal:

1. Subir entidades em `PharmaStock.Modelos` a partir do domínio do III  
2. Esboçar interfaces de `Repositorio` e regras de `Negocio` (FIFO, validações)  
3. Só então expor endpoints na `PharmaStock.Api` alinhados ao backlog que você priorizar  

Bloqueio real para a API: MVP e contratos de dados estáveis o suficiente (não precisa do SQL 100% pronto no dia 1).

---

## 9. Pendências explícitas (para não se perderem)

| ID | Pendência | Quem resolve |
|----|-----------|--------------|
| P1 | PostgreSQL vs SQL Server (manual) | Geovanna + Banco + professor |
| P2 | Nome do dono do banco + definição de trigger/procedure | Geovanna agenda; Banco decide tecnicamente com Anderson |
| P3 | Unificar `movimentacao` (SQL III) com `Venda` (código III) | Banco + Anderson |
| P4 | Nomes dos demais papéis (Web, Mobile, Docs, UX) | Geovanna / líder do grupo |
| P5 | Feature complementar do Flutter | Mobile + Anderson (contrato API) + Geovanna (aceite) |
| P6 | Limite de 6 na postagem UNIP | Líder / Geovanna com coordenação |

---

## 10. Referências rápidas no repo

| Caminho | Para quê |
|---------|----------|
| `docs/manual/Manual_PIM_IV_2026.docx` | Regras oficiais do semestre |
| `docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md` | Divisão detalhada + checklists por etapa |
| `legacy/database-postgresql/` | SQL de referência do III (adaptar) |
| `legacy/console/.../Models` e `Services` | Domínio e regras (FIFO, etc.) |
| `src/PharmaStock.sln` | Solution do IV |
| `uiux/` | Casos de uso e protótipos já existentes |

---

*Este relatório é um ponto de partida. Geovanna pode versionar decisões novas no board e pedir atualização deste arquivo quando P1–P6 forem fechados.*
