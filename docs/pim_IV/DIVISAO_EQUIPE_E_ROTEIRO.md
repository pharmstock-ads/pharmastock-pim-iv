# PharmaStock — Divisão de equipe e roteiro de execução (PIM IV)

Documento interno do grupo. Alinha as **10 etapas do Manual do PIM IV** com a divisão de papéis, a ordem de trabalho e o checklist ponto a ponto de cada seção.

> **Limite UNIP:** o manual permite grupo de **até 6** integrantes. Com 7 pessoas, confirmar com o coordenador antes da postagem na plataforma.

---

## 1. Visão geral dos papéis (7 pessoas)

| # | Papel | Responsável (preencher) | Etapas principais | Pastas / projetos |
|---|--------|-------------------------|-------------------|-------------------|
| 1 | **Núcleo C# + API** | _Anderson (proposta)_ | Etapa 4 (Modelos, Negócio, Repositório, Api) | `src/PharmaStock.Modelos`, `Negocio`, `Repositorio`, `Api` |
| 2 | **Banco de dados** | _a definir_ | Etapa 3 | `database/` |
| 3 | **Web** | _a definir_ | Etapa 4 (Web) + Etapa 5 | `src/PharmaStock.Web` |
| 4 | **Mobile** | _a definir_ | Etapa 9 | `mobile/` |
| 5 | **Engenharia de Software Ágil** | _a definir_ | Etapa 1 (apoio) + Etapa 2 | backlog, board, `docs/pim_IV/` (planejamento) |
| 6 | **Documentação acadêmica** | _a definir_ | Etapa 10 + montagem ABNT | `docs/pim_IV/` (documento final) |
| 7 | **UX / UI** | _a definir_ | Etapa 6 | `uiux/` |

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

### Etapa 1 — Definição e caracterização do negócio fictício

**Dono do texto:** Ágil (redator) + Documentação (revisão final)  
**Participam:** todo o grupo no kickoff  

- [ ] Definir denominação da empresa (continuidade PharmaStock / farmácia, salvo mudança acordada)
- [ ] Definir segmento de atuação
- [ ] Definir público-alvo (ex.: farmacêutico, atendente, gestor)
- [ ] Descrever produtos/serviços oferecidos
- [ ] Identificar problemas que justificam o sistema (estoque, validade FIFO, vendas, etc.)
- [ ] Registrar decisão: continuidade do PIM III (citar o trabalho anterior no documento)
- [ ] Entregar 1–2 páginas prontas para o capítulo de introdução/contexto

**Critério de pronto:** qualquer integrante consegue explicar o negócio em 1 minuto sem ambiguidade.

---

### Etapa 2 — Engenharia de Software Ágil Aplicada

**Dono:** Ágil  
**Apoiam:** Núcleo (viabilidade técnica), UX (fluxos), Web/Mobile (esforço de telas)

- [ ] Listar stakeholders (atores do sistema)
- [ ] Levantar requisitos funcionais (RF) — lista numerada
- [ ] Levantar requisitos não funcionais (RNF) — performance, segurança, usabilidade, etc.
- [ ] Montar backlog priorizado (MoSCoW ou ordenação simples)
- [ ] Definir iterações/sprints (mínimo 2–3 ciclos planejados)
- [ ] Definir critérios de aceite por item de alta prioridade
- [ ] Publicar board (Trello/GitHub Projects/etc.) e manter atualizado
- [ ] Gerar artefatos para o documento: print do board, tabela de RF/RNF, visão das sprints

**Critério de pronto:** top 5 itens do backlog estão claros o bastante para Banco e Núcleo modelarem.

**Itens sugeridos de alta prioridade (ajuste com o grupo):**

1. Autenticação / funcionário  
2. Cadastro de fornecedor e produto  
3. Entrada de lote  
4. Venda com baixa de estoque (FIFO por validade)  
5. Consultas/listagens essenciais (web)  
6. Funcionalidade complementar no app (mobile)

---

### Etapa 3 — Modelagem de Banco de Dados

**Dono:** Banco (pessoa única)  
**Apoia:** Núcleo C# (alinhamento das entidades)

- [ ] Identificar informações essenciais ao negócio (≥ 5 entidades)
- [ ] Modelo conceitual (DER) — diagrama
- [ ] Modelo lógico — tabelas, PKs, FKs, cardinalidades
- [ ] Modelo físico em **SQL Server** (`database/schema.sql`)
- [ ] Criar **ao menos 1 trigger** (`database/triggers.sql`) + justificar no doc
- [ ] Criar **ao menos 1 stored procedure** (`database/procedures.sql`) + justificar no doc
- [ ] Script de seed (`database/seed.sql`) com dados de demonstração
- [ ] Justificativa técnica do banco relacional
- [ ] Descrever principais operações/consultas previstas
- [ ] Entregar prints/diagramas para Documentação

**Critério de pronto:** schema aplicável no SQL Server; procedure e trigger testáveis; Núcleo consegue mapear entidades.

**Observações técnicas do manual:**

- Connection string apenas em `appsettings.json` (nunca hardcoded)
- Consultas parametrizadas
- Procedure chamada pela camada Repositório
- Se houver trigger + EF Core 7+, usar `HasTrigger` no mapeamento

---

### Etapa 4 — Desenvolvimento C# (POO, MVC e Web API)

#### 4.A — Núcleo (Modelos + Negócio + Repositório) — dono: Núcleo C# + API

- [ ] Criar/ajustar classes em `PharmaStock.Modelos` (sem dependência de outras camadas)
- [ ] Aplicar encapsulamento / herança / polimorfismo onde fizer sentido (evidenciar no doc)
- [ ] Interfaces e classes de repositório em `PharmaStock.Repositorio`
- [ ] Implementar acesso a dados (EF Core e/ou Dapper)
- [ ] Executar a stored procedure pelo Repositório
- [ ] Implementar regras em `PharmaStock.Negocio` (validações, estoque, venda, etc.)
- [ ] Configurar injeção de dependência nos `Program.cs` de Web e Api
- [ ] Garantir que Web/Api **não** referenciam Repositório diretamente
- [ ] Escrever testes básicos em `PharmaStock.Tests` (regras críticas)
- [ ] Separar trechos de código comentados para o documento (1 repositório + 1 classe de negócio no mínimo)

**Critério de pronto:** dá para cadastrar entidades principais e executar ao menos um fluxo de negócio sem UI.

#### 4.B — API — dono: Núcleo C# + API

- [ ] Controllers REST em `PharmaStock.Api`
- [ ] DTOs (não expor entidades cruas, quando possível)
- [ ] Verbos HTTP e status codes adequados
- [ ] Swagger/OpenAPI documentado
- [ ] Endpoints alinhados ao que o Mobile precisa
- [ ] Autenticação mínima acordada com o grupo (mesmo que simples no MVP)
- [ ] Trecho de controller da API comentado para o documento
- [ ] Publicar contrato (lista de rotas) para Mobile e Documentação

**Critério de pronto:** Mobile consegue consumir ≥ 1 fluxo completo via API.

#### 4.C — Web (parte MVC da Etapa 4) — dono: Web

- [ ] Controllers MVC finos (só orquestram Negócio)
- [ ] Views Razor + layouts + partials
- [ ] ViewModels para telas
- [ ] Integração com as regras do Núcleo (sem SQL na UI)
- [ ] Trecho de controller MVC comentado para o documento
- [ ] Print da solution no Visual Studio mostrando os 5+ projetos

**Critério de pronto:** telas principais do backlog web funcionam contra o Negócio/banco.

#### Artefatos obrigatórios da Etapa 4 (Documentação coleta)

- [ ] Descrição da arquitetura em camadas
- [ ] Diagrama: Web/Api → Negócio → Repositório → SQL Server + Flutter
- [ ] Trechos: repositório, negócio, controller MVC, controller API, chamada da procedure

---

### Etapa 5 — Desenvolvimento Web Responsivo

**Dono:** Web  
**Apoia:** UX (layout e breakpoints)

- [ ] Layout responsivo (desktop / tablet / mobile web)
- [ ] Consistência visual com a identidade (Blue-Pharma / protótipos)
- [ ] Formulários e listagens usáveis em tela estreita
- [ ] Integração front (Razor/CSS) com back (Negócio)
- [ ] Capturas de tela em resoluções diferentes para o documento

**Critério de pronto:** as telas prioritárias não “quebram” em viewport estreita.

---

### Etapa 6 — UX e UI Design

**Dono:** UX/UI  
**Apoiam:** Web e Mobile (implementação)

- [ ] Definir personas (mínimo 2)
- [ ] Descrever fluxos principais (login, cadastro produto, lote, venda, app)
- [ ] Wireframes ou protótipos atualizados em `uiux/`
- [ ] Análise básica de usabilidade + melhorias propostas
- [ ] Entregar PNGs/links Figma + texto para Documentação

**Critério de pronto:** Web e Mobile têm referência visual clara do que construir.

---

### Etapa 7 — Machine Learning e Análise de Dados _(seção curta)_

**Dono do texto:** Documentação  
**Insumos:** Banco + Núcleo/Web (relatórios/indicadores)

- [ ] Listar dados relevantes (estoque baixo, validade, vendas)
- [ ] Aplicar análise simples ou ML básico (ex.: tendência de vendas, alerta preditivo simples)
- [ ] Gerar indicador/relatório interpretável no negócio
- [ ] Escrever ½–1 página no documento

**Não é papel de tempo integral.** Fazer depois que houver dados reais no seed/banco.

---

### Etapa 8 — Comunicação, Liderança, Negociação e LIBRAS _(seção curta)_

**Dono do texto:** Documentação  
**Insumos:** Ágil (cerimônias) + todo o grupo

- [ ] Descrever comunicação do time (reuniões, board, GitHub)
- [ ] Relatar 1 situação de liderança/negociação (priorização do backlog)
- [ ] Proposta breve de acessibilidade / LIBRAS (ex.: glossário, recurso inclusivo na UI ou na doc)
- [ ] Escrever ½–1 página no documento

---

### Etapa 9 — Desenvolvimento Mobile (Flutter)

**Dono:** Mobile  
**Depende de:** API estável (contratos)

- [ ] Criar app em `mobile/pharmastock_app` (Flutter)
- [ ] Telas adaptadas a dispositivos móveis
- [ ] Consumir a Web API (REST)
- [ ] Implementar **ao menos 1 funcionalidade complementar** ao sistema web
- [ ] Tratar erros de rede / estados de loading
- [ ] Capturas de tela para Documentação
- [ ] Documentar endpoints usados

**Sugestões de funcionalidade complementar:** consulta rápida de estoque/validade, alerta de lote crítico, registro simples de movimentação, dashboard resumido.

**Critério de pronto:** demo gravável do app falando com a API real (ou ambiente local documentado).

---

### Etapa 10 — Integração e avaliação final

**Dono da consolidação:** Documentação  
**Participam:** todos

- [ ] Integrar Web + API + Banco + Mobile em um fluxo demonstrável
- [ ] Checklist de RF prioritários: feito / parcial / não feito
- [ ] Avaliar resultados vs objetivos do negócio fictício
- [ ] Listar limitações técnicas e conceituais
- [ ] Propor melhorias / evolução futura
- [ ] Ensaiar apresentação (se houver)

**Critério de pronto:** um caminho feliz ponta a ponta (ex.: cadastrar produto/lote → vender/consultar → ver no app).

---

## 5. Documento acadêmico (ABNT) — checklist da Documentação

Estrutura mínima do manual:

- [ ] Capa  
- [ ] Resumo + palavras-chave  
- [ ] Abstract + keywords  
- [ ] Sumário  
- [ ] Introdução  
- [ ] Desenvolvimento (**20 a 30 páginas**) cobrindo as etapas  
- [ ] Conclusão  
- [ ] Referências (ABNT)  

**Regras importantes:**

- Citar o PIM III (continuidade; evitar autoplágio sem referência)
- Somente o **líder** posta na plataforma da UNIP
- Não entregar fora do prazo / fora do canal oficial

**Pasta sugerida:** `docs/pim_IV/` (rascunhos, imgs, versão final).

---

## 6. Roteiro por fase no tempo (sugerido)

### Fase A — Fundação (semana 1–2)

| Quem | O que fazer |
|------|-------------|
| Ágil | Kickoff, RF/RNF, backlog v1, board |
| UX | Personas + fluxos + wireframes iniciais |
| Banco + Núcleo | Lista de entidades alinhada |
| Docs | Esqueleto do documento + sumário |
| Web / Mobile / API | Aguardar modelagem mínima (podem ler legado em `legacy/`) |

### Fase B — Dados e núcleo (semana 3–5)

| Quem | O que fazer |
|------|-------------|
| Banco | Schema, trigger, procedure, seed |
| Núcleo | Modelos, Negócio, Repositório, testes |
| Ágil | Refinar backlog; critérios de aceite dos itens em sprint |
| UX | Protótipos de alta fidelidade das telas prioritárias |
| Docs | Capítulos 1–3 (negócio, ágil, banco) em rascunho |

### Fase C — Interfaces (semana 5–8)

| Quem | O que fazer |
|------|-------------|
| Núcleo + API | Expor endpoints + Swagger |
| Web | MVC + responsivo das telas prioritárias |
| Mobile | App + consumo da API + 1 feature complementar |
| UX | Ajustes de usabilidade com base na implementação |
| Docs | Capturas, diagramas de arquitetura, trechos de código |

### Fase D — Fechamento (semana 8–fim)

| Quem | O que fazer |
|------|-------------|
| Todos | Integração ponta a ponta, correção de bugs |
| Docs | Etapas 7–8 curtas, Etapa 10, revisão ABNT, PDF final |
| Ágil | Retrospectiva + evidências de sprints para o doc |
| Líder | Postagem na plataforma UNIP + cópia impressa se exigida |

---

## 7. Definição de pronto por papel (resumo)

| Papel | “Pronto” significa |
|-------|-------------------|
| Ágil | Backlog priorizado, sprints e aceite documentados |
| Banco | SQL Server com trigger + procedure executáveis + diagramas |
| Núcleo + API | Camadas corretas; fluxo de negócio; API documentada no Swagger |
| Web | Telas prioritárias responsivas ligadas ao Negócio |
| Mobile | App Flutter com ≥ 1 feature via API |
| UX | Personas, fluxos e protótipos entregues e usados pelo time |
| Documentação | Doc ABNT completo, coerente, com evidências de todas as etapas |

---

## 8. Cadastro dos nomes (preencher)

| Papel | Nome | RA |
|-------|------|----|
| Núcleo C# + API | | |
| Banco de dados | | |
| Web | | |
| Mobile | | |
| Eng. Software Ágil | | |
| Documentação | | |
| UX / UI | | |
| Líder (postagem UNIP) | | |

---

## 9. Referências internas do repositório

| Caminho | Uso |
|---------|-----|
| `docs/manual/Manual_PIM_IV_2026.docx` | Manual oficial |
| `docs/pim_III/` | Diagramas e material do semestre passado (referência) |
| `legacy/` | Console, WinForms, HTML e PostgreSQL do PIM III |
| `src/PharmaStock.sln` | Solution do PIM IV |
| `database/` | Scripts SQL Server |
| `mobile/` | App Flutter |
| `uiux/` | Casos de uso e protótipos |

---

*Documento vivo: atualizar checklists conforme o andamento das sprints.*
