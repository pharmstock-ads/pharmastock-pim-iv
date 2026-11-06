# Relatório — Engenharia de Software Ágil (PharmaStock · PIM IV)

**Para:** Geovanna (Engenharia de Software Ágil)  
**De:** Anderson (Líder UNIP · Núcleo C# + API)  
**Criado:** 14/10/2026  
**Atualizado:** 06/11/2026 (status técnico: núcleo/API etapa 2 ✅; UX = Daniel; Scalar; gaps Ágil §5 para alinhar com Geovanna)  
**Objetivo:** artefato de apoio às **Etapas 1 e 2** do manual — decisões, caracterização do negócio, **RF/RNF**, backlog/MVP e checklist para sprints e aceite.

**Guias por papel (encaminhar aos donos):**

| Papel | Documento |
|-------|-----------|
| Banco (Geovanni) | [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md) |
| Núcleo + API (Anderson) | [GUIA_NUCLEO_API_ANDERSON.md](./GUIA_NUCLEO_API_ANDERSON.md) |
| Web (Vinicius) | [GUIA_WEB_VINICIUS.md](./GUIA_WEB_VINICIUS.md) |
| Mobile (Roberto) | [GUIA_MOBILE_ROBERTO.md](./GUIA_MOBILE_ROBERTO.md) |
| UX (Daniel) | [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md) |

**Complementares:** [DIVISAO_EQUIPE_E_ROTEIRO.md](./DIVISAO_EQUIPE_E_ROTEIRO.md) · [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) · [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) · [`../../database/README.md`](../../database/README.md)

**Board Trello (PIM IV):** [https://trello.com/b/9ifnCGAL/pim-iv](https://trello.com/b/9ifnCGAL/pim-iv)  
Listas: BACKLOG · A FAZER · FAZENDO · FEITO · FUTUROS — cards alinhados aos RF/MVP e ao status técnico (06/11).

---

## 1. Contexto do projeto

O PIM IV é a **continuidade** do PharmaStock do PIM III ([repo III](https://github.com/pimIII/pimIII); trabalho atual em [pharmastock-pim-iv](https://github.com/pharmstock-ads/pharmastock-pim-iv)).

| PIM III (legado) | PIM IV (este semestre) |
|------------------|------------------------|
| Console + WinForms, memória | Solution em camadas + SQL Server |
| Scripts PostgreSQL de referência | SQL Server oficial |
| Protótipo HTML / UX | Web MVC + API + Flutter |

Código do III → `legacy/`. Solution nova → `src/PharmaStock.sln`.

Manual exige, em resumo: **Modelos → Repositório → Negócio → Web (MVC) + Api**, SQL Server com **≥1 trigger + ≥1 SP** (SP chamada pelo Repositório), app **Flutter**, documento ABNT.

---

## 2. Decisões fechadas (kickoff + alinhamentos)

| # | Tema | Decisão | Status |
|---|------|--------|--------|
| D1 / A1 | Negócio fictício | **Continuar PharmaStock / farmácia** (não reiniciar domínio) | **Confirmado** |
| D2 | Escopo MVP | Ver §3 | **Confirmado** |
| D3 | Líder na plataforma UNIP | **Anderson** | **Confirmado** (05/11) |
| D3b | Papel Anderson (técnico) | Núcleo C# (Modelos, Negócio, Repositório) **+ API** | **Confirmado** |
| D4 | Papel Geovanna | Engenharia de Software Ágil (RF/RNF, backlog, sprints, aceite) | **Confirmado** |
| D5 | SGBD | **SQL Server** — **não** PostgreSQL | **Confirmado** |
| D6 | Procedures / triggers | Entrada, venda, movimentação, lote; triggers de estoque/totais | **Entregue (refinar)** |
| D7 | Papel banco | **Geovanni** | **Confirmado** |
| D8 | Papel Web | **Vinicius** — MVC + responsivo | **Confirmado** |
| D9 | Papel Mobile | **Roberto** — Flutter | **Confirmado** |
| A7 | Feature Flutter (complementar) | **Lotes a vencer + estoque baixo** (consultas via API) | **Confirmado** |
| A2 | JWT / auth avançada | **Adiar** — MVP com `funcionarioId` no body | Depois |
| A5 / A6 | Itens abertos de polish (preço de tabela, desconto, etc.) | **Adiar** — fora do primeiro MVP | Depois |
| D10 | Papel UX/UI | **Daniel** — [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md) | **Confirmado** (06/11) |
| D11 | Persistência (C1/C2) | **Dapper puro** (sem EF/Migrations/`HasTrigger`); schema em `database/` | **Confirmado** |
| D12 | Docs API | OpenAPI + **Scalar** (`/scalar`), chave connection string **`Padrao`** | **Confirmado** |

### Material Ágil já no repo (A3 / A4)

Pasta `docs/pim_IV/gerenciamentodeprojetos/`:

| Arquivo | Nota |
|---------|------|
| `Gerenciamento_Projetos_Ageis_ATUALIZADO.pdf.pdf` | **Versão atualizada** (preferir esta) |
| `projetos-ageis-geovanna.pdf.pdf` | Cópia / versão anterior (mesmo conteúdo após merge) |

**Ação sua:** cruzar o PDF **ATUALIZADO** com a **§4 (RF/RNF)** deste relatório; manter/refinar User Stories no board Trello ([pim-iv](https://trello.com/b/9ifnCGAL/pim-iv)). O baseline técnico e os cards iniciais já estão lá — não precisa reinventar a partir do zero.

---

## 3. MVP funcional (backlog prioritário)

Ordem sugerida de valor:

1. **Login / funcionário** (auth didática: retorna `funcionarioId`, `nome`, `cargo`)  
2. **Cadastro de fornecedor e produto** (+ categoria se já existir no schema)  
3. **Entrada de estoque** (lote + validade) — SPs `prc_ObterOuCadastrarLote` + `prc_RegistrarEntrada*`  
4. **Venda com baixa de estoque** — Negócio escolhe lote **FIFO/FEFO**; SPs `prc_RegistrarVenda*`  
5. **Listagens essenciais na Web** (produtos, lotes, entradas, vendas)  
6. **Flutter:** alertas de **lotes a vencer** e **estoque baixo** (A7)

**Fora do MVP inicial (resolver depois):** JWT, desconto em venda, tabela `produtos_precos`, ML, LIBRAS (só seções curtas no ABNT).

---

## 4. Requisitos Funcionais (RF) e Não Funcionais (RNF)

Baseline derivado das **decisões do kickoff**, do **modelo/SPs/triggers** (`database/`), das **Orientações Service** e do **contrato técnico**.  
Prioridade: **Must** = MVP · **Should** = desejável no semestre · **Could** = depois / ABNT curto.

### 4.1 Atores

| Ator | Descrição |
|------|-----------|
| Funcionário (farmacêutico / estoquista / atendente) | Opera Web e/ou app; identificado após login |
| Sistema (Negócio + Banco) | Valida regras, escolhe lote (FEFO), persiste via SPs/triggers |
| Administrador acadêmico / demo | Usa seed (`inserts.txt`) para apresentação |

### 4.2 Requisitos Funcionais

| ID | Requisito | Prioridade | Origem / evidência | Aceite mínimo |
|----|-----------|------------|--------------------|---------------|
| **RF01** | Autenticar funcionário por CPF ou e-mail + senha e obter identificação da sessão (`funcionarioId`, nome, cargo) | Must | MVP §3 · `funcionario` · API login | Credenciais válidas + ativo → sucesso; inválidas → erro claro (ex.: 401). JWT **fora** do MVP |
| **RF02** | Cadastrar e listar **fornecedores** | Must | `fornecedor` · inserts | Nome + CNPJ únicos; listagem na Web |
| **RF03** | Cadastrar e listar **categorias** de produto | Should | `categoria` · inserts | Listar/criar; produto pode referenciar categoria |
| **RF04** | Cadastrar e listar **produtos** (código, nome, código de barras opcional, fornecedor, categoria) | Must | `produto` · inserts | Código único; vínculo opcional a fornecedor/categoria |
| **RF05** | Manter **lotes** por produto (número, validade, quantidade) | Must | `lote` · `prc_ObterOuCadastrarLote` | Cadastro/seed com qtd 0; estoque sobe só por movimentação |
| **RF06** | Obter lote existente ou cadastrar lote novo na entrada (`produto` + `numero_lote` + `validade`) | Must | `prc_ObterOuCadastrarLote` · Service | Mesmo produto+número → reutiliza `id_lote`; senão cria com qtd 0 |
| **RF07** | Registrar **entrada** de estoque (cabeçalho + itens) com preço unitário, quantidade > 0 e funcionário responsável | Must | `entrada` / `entradaitem` · `prc_RegistrarEntrada*` · `preco_unitario` | Após sucesso, `lote.quantidade` aumenta; `entrada.valor_total` recalculado |
| **RF08** | Registrar **venda** (cabeçalho + itens) com preço unitário e funcionário responsável | Must | `venda` / `vendaitem` · `prc_RegistrarVenda*` | Após sucesso, estoque do lote baixa; `venda.valor_total` recalculado |
| **RF09** | Na venda, o cliente **não escolhe lote**; o sistema seleciona por **FEFO** (validade mais próxima primeiro) | Must | Contrato · Service · decisão FEFO | Com 2+ lotes, baixa primeiro o de validade menor; item grava `id_lote` |
| **RF10** | Impedir venda com quantidade maior que o estoque disponível | Must | Service §5 · Negócio | Operação abortada + mensagem amigável (API 400 / Web ModelState) |
| **RF11** | Validar antes de entrada/venda: funcionário existente (e ativo), produto existente, qtd > 0, preço ≥ 0 | Must | Service §2 | Falha de validação não chama SP / não grava parcial |
| **RF12** | Registrar **movimentação** de estoque tipo `'e'` (entrada) ou `'s'` (saída) ligada ao item | Must | `movimentacao` · `prc_MovimentaItens` | Cada item de entrada/venda gera movimentação coerente |
| **RF13** | Atualizar automaticamente `lote.quantidade` após insert/delete em movimentação | Must | `trg_MovimentaItens` | App/Negócio **não** atualiza quantidade direto |
| **RF14** | Recalcular automaticamente `entrada.valor_total` e `venda.valor_total` | Must | `trg_RecalculaEntrada` / `trg_RecalculaVenda` | Total = Σ (preço × qtd); entrada com `COALESCE(...,0)` |
| **RF15** | Listar produtos, lotes, entradas e vendas na **Web** | Must | MVP §3 · guia Web | Telas Index/Details mínimas após Negócio |
| **RF16** | Consultar **lotes a vencer** (validade ≤ hoje + N dias) via API para o app | Must | A7 · Flutter | Lista com produto, lote, validade, qtd; parâmetro `dias` |
| **RF17** | Consultar **estoque baixo** (quantidade ≤ limite) via API para o app | Must | A7 · Flutter | Lista coerente; parâmetro `limite` (constante no MVP ok) |
| **RF18** | Login no app Flutter e navegação para as telas de alerta (RF16/RF17) | Must | A7 · guia Mobile | Mesmo contrato de auth da API |
| **RF19** | Disponibilizar dados de demonstração (seed) para apresentação | Should | `inserts/inserts.txt` | Cadastros ok; **falta** entrada demo para estoque ≠ 0 e alerta A7 |
| **RF20** | Impedir venda de lote **vencido** | Could | Service §8 (avaliar) | Fora do primeiro MVP, salvo se o time fechar cedo |
| **RF21** | Cancelar/estornar entrada ou venda | Could | Service §8 | Fora do MVP |
| **RF22** | Aplicar desconto na venda / tabela de preços de produto | Could | A5/A6 adiados | Fora do MVP |

### 4.3 Requisitos Não Funcionais

| ID | Categoria | Requisito | Prioridade | Origem |
|----|-----------|-----------|------------|--------|
| **RNF01** | Arquitetura | Solução em camadas: Modelos → Repositório → Negócio → Web e Api; Web/Api **não** acessam banco/Repositório | Must | Manual PIM IV · Contrato |
| **RNF02** | Persistência | SGBD **SQL Server**; ≥ 1 trigger e ≥ 1 stored procedure; SP executada pelo **Repositório** | Must | Manual · D5 |
| **RNF03** | Integridade | Integridade referencial (PK/FK/CHECK) no banco; quantidades e preços com CHECK conforme schema | Must | `tabelas.txt` |
| **RNF04** | Segurança (dados) | Connection string em `appsettings.json` (nunca hardcoded); consultas/SPs parametrizadas (anti SQL Injection) | Must | Manual · Contrato |
| **RNF05** | Segurança (auth) | MVP com identificação por `funcionarioId` pós-login; **JWT adiado** (A2) | Must (MVP) | Decisão A2 |
| **RNF06** | Usabilidade | Mensagens de erro compreensíveis na UI (sem stack/SQL cru para o usuário) | Must | Service §6 |
| **RNF07** | Confiabilidade | Entrada/venda com vários itens em **transação** (commit/rollback); sem gravação parcial | Must | Service §6 |
| **RNF08** | Manutenibilidade | Fonte da verdade dos dados = schema + SPs versionados; C# adapta-se ao banco | Must | Contrato §1.4 |
| **RNF09** | API | REST JSON + OpenAPI/**Scalar** em Development; status HTTP coerentes (200/201/400/401/404) | Must | Contrato §7 · `/scalar` |
| **RNF10** | Cliente Web | Interface **responsiva** (desktop + mobile browser) | Must | Manual Etapa 5 · guia Web |
| **RNF11** | Cliente Mobile | App **Flutter** consumindo apenas a Api (sem SQL no Dart) | Must | Manual Etapa 9 · A7 |
| **RNF12** | Desempenho | Operações de entrada/venda adequadas a demo acadêmica (sem meta numérica rígida no MVP) | Should | Contexto PIM |
| **RNF13** | Portabilidade / ambiente | Rodar em ambiente de desenvolvimento do grupo (LocalDB/Express/Docker + .NET + Flutter) | Should | Guias por papel |
| **RNF14** | Documentação | Documento ABNT cobrindo etapas do manual; RF/RNF e decisões rastreados | Must | Manual Etapa 10 |
| **RNF15** | Acessibilidade / LIBRAS | Seção **curta** no ABNT (sem feature de código dedicada) | Could | Decisão grupo |
| **RNF16** | Análise de dados / ML | Seção **curta** no ABNT (sem pipeline de ML no MVP) | Could | Decisão grupo |

### 4.4 Mapeamento rápido RF → implementação

| RF | Banco | Negócio / API | UI |
|----|-------|---------------|-----|
| RF01 | `funcionario` | `POST /api/auth/login` | Web login · Flutter login |
| RF02–RF04 | tabelas cadastro | CRUD Repositório + Negócio | Web |
| RF05–RF07 | SPs entrada + ObterOuCadastrarLote + triggers | Orquestra entrada | Web formulário entrada |
| RF08–RF11 | SPs venda + trigger | FEFO + valida estoque | Web formulário venda |
| RF12–RF14 | `movimentacao` + triggers | Não duplicar update de estoque/total | — |
| RF15 | consultas | Serviços de listagem | Web |
| RF16–RF18 | consultas lote | `GET /api/alertas/...` | Flutter |
| RF19 | `inserts.txt` (+ entrada demo) | — | Demo |

### 4.5 Fora de escopo do MVP (não viram card agora)

- JWT / OAuth · desconto · `produtos_precos` · estorno · biometria · offline sync · GraphQL · microsserviços  

Use §4 como backlog inicial; refine a redação acadêmica no PDF/board **sem contradizer** os IDs acima.

---

## 5. O que falta vs Manual do PIM IV (Etapas 1 e 2)

Fonte: `docs/manual/Manual_PIM_IV_2026.docx`.  
Este relatório **ainda não fecha sozinho** as Etapas 1–2 do **manual** (texto de negócio + board/sprints): o §4 já cobre o baseline de RF/RNF. O que está abaixo é o **gap acadêmico/operacional da Geovanna** — alinhar na conversa e marcar ✅ aqui quando fechar.

**Para alinhar com a Geovanna (checklist curto):**

1. Texto formal do negócio fictício (segmento, público, problemas) — §5.1  
2. Stakeholders completos  
3. Board Trello ([pim-iv](https://trello.com/b/9ifnCGAL/pim-iv)) — cards Must já criados; refinar sprints/labels  

4. 2–3 sprints (datas + donos + quais RF)  
5. Definition of Done (sugerido: Scalar + SP no SSMS + tela ou app)  
6. Cruzar §4 com o PDF ATUALIZADO em `gerenciamentodeprojetos/`

### 5.1 Etapa 1 — Definição e caracterização do negócio fictício

O manual exige contemplar: denominação; segmento; público-alvo; produtos/serviços; necessidades/problemas que justificam o sistema.  
**Resultado esperado:** contexto empresarial claro e relevância do sistema.

| Item do manual | Status | O que falta fazer |
|----------------|--------|-------------------|
| Denominação da empresa | ⚠️ | Formalizar no texto ABNT: **PharmaStock** (farmácia / gestão de estoque) — hoje só está na tabela de decisões |
| Segmento de atuação | ❌ | Escrever (ex.: varejo farmacêutico / farmácia de bairro ou rede pequena) |
| Público-alvo | ❌ | Escrever (ex.: farmacêuticos, estoquistas, atendentes, gestão da loja) |
| Produtos ou serviços oferecidos | ❌ | Descrever o que a **empresa fictícia** vende/oferece (medicamentos, etc.) **e** o que o **sistema** oferece |
| Necessidades / problemas que justificam o sistema | ⚠️ | Expandir em prosa: controle de validade, ruptura de estoque, rastreio de entrada/venda (hoje implícito no MVP) |
| Texto contínuo 1–2 páginas (ABNT) | ❌ | Capítulos/seções no documento final — não só este markdown |
| Continuidade PIM III | ⚠️ | Citar explicitamente no texto do negócio (já mencionado neste relatório) |

**Critério de pronto (manual + grupo):** qualquer integrante explica o negócio em ~1 minuto; o ABNT tem a caracterização completa.

### 5.2 Etapa 2 — Engenharia de Software Ágil Aplicada

O manual exige apresentar: stakeholders; RF; RNF; backlog; priorização; iterações e critérios de aceite.  
**Resultado esperado:** planejamento estruturado evidenciando práticas ágeis.

| Item do manual | Status | O que falta fazer |
|----------------|--------|-------------------|
| Partes interessadas (stakeholders) | ⚠️ | Lista nominada além dos atores §4.1 (ex.: farmacêutico, estoquista, gerente, atendente, sponsor acadêmico) + interesse de cada um |
| Levantamento e descrição dos RF | ⚠️ | §4.2 = baseline técnico; falta redação acadêmica (descrição por RF) e/ou User Stories no board alinhadas aos IDs RF01–RF22 |
| Levantamento e descrição dos RNF | ⚠️ | §4.3 = baseline; falta prosa no ABNT/PDF e vínculo aos critérios de qualidade da sprint |
| Organização em backlog | ⚠️ | Board [Trello PIM IV](https://trello.com/b/9ifnCGAL/pim-iv) criado com cards Must/Should; falta organizar sprints/labels |
| Priorização das funcionalidades | ⚠️ | MVP §3 + Must/Should/Could existem; falta ordenação visível no board (rank / MoSCoW no card) |
| Definição de iterações (sprints) | ❌ | Definir **2–3 sprints**: objetivo, janela de datas, quais RF entram, dono |
| Critérios de aceite | ⚠️ | Aceite na tabela RF; falta **aceite por sprint** e DoD do time |
| Evidências / artefatos para o documento | ❌ | Prints do board, tabela RF/RNF formatada para ABNT, resumo das sprints; cruzar com PDF em `gerenciamentodeprojetos/` |

**Critério de pronto (manual + grupo):** board atualizado + 2–3 sprints descritas + RF/RNF rastreáveis no ABNT.

### 5.3 Já coberto (não reabrir como “falta”)

- Decisão de continuar PharmaStock / farmácia (D1)  
- SGBD SQL Server (D5)  
- Baseline **RF01–RF22** e **RNF01–RNF16** (§4)  
- Prioridade Must do MVP (login → cadastros → entrada → venda FEFO → Web → Flutter A7)  
- Feature Flutter fechada: lotes a vencer + estoque baixo (A7 / RF16–RF18)  
- Papéis da equipe e líder UNIP (Anderson)

### 5.4 Ordem sugerida para fechar o gap (Geovanna)

1. Redigir **Etapa 1** (1–2 páginas) usando a grade §5.1.  
2. Completar **stakeholders** (§5.2).  
3. Passar RF Must → **cards no board** (manter IDs RF0x).  
4. Montar **Sprint 1 / 2 / 3** (objetivo + RF + aceite).  
5. Cruzar com o PDF **ATUALIZADO** e gerar prints para o ABNT.  
6. Marcar checklists da §10 (deste doc) e da [DIVISAO_EQUIPE_E_ROTEIRO.md](./DIVISAO_EQUIPE_E_ROTEIRO.md) Etapas 1–2.

---

## 6. Estado atual do repositório (o que já existe)

### 6.1 Estrutura

```
pharmastock-pim-iv/
├── src/PharmaStock.sln          ← Modelos, Negocio, Repositorio, Api, Web, Tests
├── database/                    ← SQL Server (tabelas, SPs, triggers)
├── mobile/                      ← pasta Flutter (a iniciar)
├── uiux/                        ← protótipos / UX
├── docs/pim_IV/                 ← este pacote de docs
└── legacy/                      ← PIM III (não mexer no fluxo novo)
```

### 6.2 Banco (Geovanni — avançado)

| Artefato | Situação |
|----------|----------|
| `database/tabelas/tabelas.txt` | CREATE TABLE limpo; **`entradaitem.preco_unitario`** (não mais `custo_unitario`) |
| `database/inserts/inserts.txt` | Seed parcial: 3 fornecedores, 3 funcionários, 4 categorias, 5 produtos, 5 lotes (**qtd 0**) |
| `database/procedure/*` | Entrada, venda, movimentação, ObterOuCadastrarLote; entrada item usa `@pPrecoUnitario` |
| `database/trigger/*` | Estoque + totais; `trg_RecalculaEntrada` com `COALESCE` + `preco_unitario` |
| `database/-- Orientações para a camada Service.pas` | Regras para a camada **Negócio** (Service = Negócio no C#) |
| `schema.sql` / `seed.sql` | Ainda placeholders — consolidar a partir de `tabelas/` + `inserts/` |

Tabelas principais: `fornecedor`, `funcionario`, `categoria`, `produto`, `lote`, `entrada`/`entradaitem`, `venda`/`vendaitem`, `movimentacao`.

### 6.3 Núcleo C# / API (Anderson) — **atualizado 06/11**

- Solution criada.  
- **Etapa 1** ✅: Modelos, interfaces Repo, Negócio FEFO + testes.  
- **Etapa 2** ✅ (`feature/nucleo-repo-api` / PR): Dapper + SPs + DTOs em `Modelos/Dtos` + DI Web/Api + API REST + **Scalar**; fluxo login → entrada → venda → alertas validado.  
- Detalhe: [GUIA_NUCLEO_API_ANDERSON.md](./GUIA_NUCLEO_API_ANDERSON.md) · [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) §7.

### 6.4 Web / Mobile / UX

- Web: `PharmaStock.Web` com DI pronta; **telas MVC ainda a fazer** (Vinicius) — [GUIA_WEB_VINICIUS.md](./GUIA_WEB_VINICIUS.md).  
- Mobile: pasta `mobile/` (criar app Flutter); API A7 pronta — [GUIA_MOBILE_ROBERTO.md](./GUIA_MOBILE_ROBERTO.md).  
- UX: Daniel — evoluir `uiux/` (base III) — [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md).

---

## 7. Divisão de papéis e liderança

| Papel | Responsável | Entrega principal |
|-------|-------------|-------------------|
| **Líder UNIP** | **Anderson** | Postagem na plataforma, coordenação, núcleo + API |
| Núcleo C# + API | Anderson | Camadas + REST + Scalar (**etapa 2 ✅**) |
| Banco | Geovanni | Tabelas/SPs/triggers ✅; unificar schema/seed demo opcional |
| Web (MVC + responsivo) | Vinicius | Telas Razor via Negócio |
| Mobile (Flutter) | Roberto | App via API (alertas A7) |
| Eng. Software Ágil | **Geovanna** | Negócio fictício, RF/RNF, backlog, sprints, aceite (**gaps §5**) |
| Documentação ABNT | _a definir_ | Doc final |
| UX/UI | **Daniel** | Personas / fluxos / protótipos — [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md) |

**ML e LIBRAS:** seções curtas no ABNT (Documentação), sem dono dedicado de código.

> Manual: até **6** integrantes na postagem UNIP — validar com coordenação (pode caber “apoio” fora da postagem).

---

## 8. Arquitetura (alinhada aos RNF)

```
Flutter ──► Api ──┐
                  ├──► Negocio ──► Repositorio ──► SQL Server
Web (MVC) ────────┘
```

- Web e Api **nunca** acessam banco direto.  
- FIFO/FEFO (escolha de lote na venda) = **Negócio C#**.  
- Atualização de `lote.quantidade` = **trigger** no banco.  
- Fonte da verdade dos dados = **schema + SPs** versionados por Geovanni.

Auth MVP: `POST /api/auth/login` → `{ funcionarioId, nome, cargo }`; cliente reenvia `funcionarioId` nas operações. JWT = depois (A2).

---

## 9. Ordem de trabalho (macro)

```
[1] Negócio fictício + Ágil — fechar §5 + RF/RNF §4 → board (+ cruzar PDF)   ← Geovanna (em aberto)
[2] Banco mínimo ✅ (tabelas/SP/trigger); polish schema/seed demo opcional
[3] Negócio + Repositório (Dapper) ✅
[4] API + Scalar ✅
[5] Web e Mobile em paralelo                                              ← foco atual UI
[6] UX (Daniel) acompanha
[7] Integração + documento ABNT
```

Detalhe com checkboxes: [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md).

---

## 10. Checklist Ágil (Geovanna) — use no board

### Etapa 1 — Negócio fictício

- [ ] Formalizar texto: denominação PharmaStock, segmento, público, problemas (estoque/validade/venda)
- [ ] Citar continuidade do PIM III
- [ ] 1–2 páginas prontas para o ABNT

### Etapa 2 — Ágil

- [x] Baseline **RF01–RF22** e **RNF01–RNF16** neste relatório (§4) — a partir do banco + decisões
- [ ] Cruzar §4 com o PDF **ATUALIZADO** em `gerenciamentodeprojetos/` (ajustar redação acadêmica se preciso)
- [ ] Stakeholders
- [ ] Backlog priorizado no board (1 card por RF Must; RNF como DoD / restrições)
- [ ] 2–3 sprints propostas (datas + donos)
- [ ] Definition of Done mínima (ex.: SP no SSMS + endpoint no Scalar + tela Web ou app)
- [x] Board Trello criado: [https://trello.com/b/9ifnCGAL/pim-iv](https://trello.com/b/9ifnCGAL/pim-iv) (cards FEITO / A FAZER / BACKLOG / FUTUROS)
- [ ] Board com sprints/labels e aceite por card (Geovanna)
- [x] SGBD = SQL Server (não reabrir debate Postgres)
- [ ] Feature Flutter A7 no backlog: RF16 + RF17 (+ RF18)
- [ ] Limite de 6 na UNIP + nomes/RAs na divisão
- [ ] Aceite de cada sprint com o dono técnico

### Critérios de aceite (atalho — detalhe na §4.2)

| RF | Aceite mínimo |
|----|----------------|
| RF01 | CPF/e-mail + senha válidos retornam funcionário ativo; inválidos → 401 |
| RF07 | Após entrada, quantidade do lote aumenta; `valor_total` recalculado |
| RF09–RF10 | Estoque insuficiente → erro; sucesso → lote mais próximo do vencimento baixa primeiro |
| RF16–RF17 | Lista lotes com validade ≤ N dias e/ou quantidade ≤ limite |

---

## 11. Pendências abertas (rastreio)

| ID | Pendência | Status | Quem |
|----|-----------|--------|------|
| P1 | Postgres vs SQL Server | **Fechado → SQL Server** | — |
| P2 | Procedures/triggers iniciais | **Feito** (refinar + unificar schema) | Geovanni |
| P3 | Venda vs movimentação | **Fechado** (documentos + movimentacao) | — |
| P4 | Nomes Docs / UX | **UX = Daniel** ✅; Docs ABNT ainda aberto | Grupo |
| P5 | Feature Flutter | **Fechado → lotes a vencer + estoque baixo** (RF16–RF18) | Roberto + Anderson |
| P6 | Limite de 6 na UNIP | Aberto | Anderson + Geovanna |
| P7 | `schema.sql` unificado + seed completo | **Parcial** — `inserts.txt` ok; falta schema.sql + estoque demo (entrada) | Geovanni |
| P8 | JWT / auth forte | **Adiado (A2)** · RF01 MVP sem JWT | Depois |
| P9 | Desconto / preço de tabela | **Adiado (A5/A6)** · RF22 Could | Depois |
| P10 | `senha_hash varchar(25)` curto p/ SHA-256 | Técnico — alinhar | Geovanni + Anderson |
| P11 | Baseline RF/RNF no relatório | **Feito** (§4) | Geovanna |
| P12 | Lacunas Etapas 1–2 do manual | **Aberto** — ver §5 | Geovanna |

---

## 12. Próximos passos por pessoa (visão Ágil)

| Quem | Próximo passo concreto |
|------|------------------------|
| **Geovanna** | **Prioridade:** fechar gaps §5 (texto negócio + stakeholders + board + 2–3 sprints); cruzar PDF ATUALIZADO; DoD com Scalar |
| **Geovanni** | Manual mínimo ✅; opcional: `schema.sql` / entrada demo — [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md) |
| **Anderson** | Núcleo/API ✅; coordenar UNIP; suporte a Web/Mobile/UX |
| **Vinicius** | **Implementar telas MVC** (Negócio já pluggável) — [GUIA_WEB_VINICIUS.md](./GUIA_WEB_VINICIUS.md) |
| **Roberto** | **Criar app Flutter** e consumir login + RF16/RF17 — [GUIA_MOBILE_ROBERTO.md](./GUIA_MOBILE_ROBERTO.md) |
| **Daniel** | Personas, fluxos, protótipos IV — [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md) |

---

## 13. Referências no repo

| Caminho | Para quê |
|---------|----------|
| `docs/manual/Manual_PIM_IV_2026.docx` | Manual oficial |
| `docs/pim_IV/gerenciamentodeprojetos/` | Material Ágil (PDF **ATUALIZADO**) |
| `database/inserts/inserts.txt` | Dados iniciais de demo |
| `docs/pim_IV/CONTRATO_TECNICO.md` | Arquitetura, SPs, rotas API |
| `database/tabelas/tabelas.txt` | Modelo físico atual |
| `database/procedure/` · `database/trigger/` | SPs e triggers |
| `database/-- Orientações para a camada Service.pas` | Regras de Negócio (validações / fluxos) |
| `src/PharmaStock.sln` | Solution do IV |
| `legacy/` | Só referência do III |

---

*Atualizar quando §5 (Geovanna) fechar. §4 = RF/RNF; §5 = gaps Ágil do manual; §6/§12 = status técnico. Guias por papel = operação do dia a dia.*
