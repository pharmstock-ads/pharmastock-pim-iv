# Guia — Web MVC (Vinicius)

**Para:** Vinicius  
**De:** Anderson (Líder · Núcleo/API)  
**Atualizado:** 05/11/2026  
**Projeto:** `src/PharmaStock.Web`  
**Contexto:** [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) · [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md)

---

## 1. O que você entrega

Aplicação **ASP.NET Core MVC** (Razor), **responsiva**, que usa a farmácia PharmaStock no navegador.

Fluxos prioritários do MVP (mesma ordem do Ágil):

1. Login de funcionário  
2. Cadastro / listagem de fornecedor e produto  
3. Registrar **entrada** (lote + validade + **preço unitário**)  
4. Registrar **venda** (produto + quantidade; sistema escolhe o lote)  
5. Consultas: produtos, lotes, histórico de entradas/vendas  

Você **não** implementa o app Flutter (Roberto) nem as stored procedures (Geovanni).

---

## 2. Arquitetura — regra que não pode quebrar

```
Browser → Controllers (Web) → Negocio → Repositorio → SQL Server
```

| Pode | Não pode |
|------|----------|
| Injetar / chamar classes de `PharmaStock.Negocio` | Referenciar `PharmaStock.Repositorio` |
| Usar ViewModels + Views Razor | Abrir `SqlConnection` / Dapper / EF no controller |
| Connection string só se a Web hospedar o Negócio (via DI) | Hardcode de senha/servidor no código |
| Layout responsivo (Bootstrap ou similar do template) | Duplicar regra FIFO na View |

A **Api** é irmã da Web: os dois consomem o **mesmo Negócio**. Você não precisa chamar HTTP da própria Api (a menos que queira para testes).

---

## 3. Contexto do domínio (resumo)

- Farmácia com **lotes** e **validade**.  
- **Entrada** aumenta estoque; **venda** diminui.  
- Estoque do lote é atualizado por **trigger** no banco.  
- Na venda, o usuário **não escolhe lote** — o Negócio faz FIFO/FEFO.  
- Auth MVP: sessão/cookie ou campo escondido com `funcionarioId` após login (JWT adiado).

Tabelas (Geovanni): fornecedor, funcionario, categoria, produto, lote, entrada/entradaitem, venda/vendaitem, movimentacao.

---

## 4. O que esperar antes de “começar de verdade”

### Já pode fazer agora (preparação)

- [ ] Clonar o repo e abrir `src/PharmaStock.sln` no Visual Studio / Rider / VS Code  
- [ ] Rodar `dotnet restore` + garantir que `PharmaStock.Web` sobe (`dotnet run --project src/PharmaStock.Web`)  
- [ ] Estudar pastas: `Controllers`, `Views`, `ViewModels` (criar se não existirem)  
- [ ] Definir layout base (`_Layout.cshtml`) + menu: Produtos, Fornecedores, Entradas, Vendas, Logout  
- [ ] Wireframes simples (ou alinhar com UX se houver dono)  
- [ ] Ler [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) e marcar seus itens quando avançar  

### Esperar o núcleo (Anderson) para integrar

Você fica **bloqueado de persistir de verdade** até existir Negócio mínimo. Enquanto isso, pode usar ViewModels mockados nas Views.

Sinais de “pode integrar”:

| Sinal | Significado |
|-------|-------------|
| Classes de serviço no `PharmaStock.Negocio` (login, listar produtos, registrar entrada/venda) | Pode plugar os controllers |
| Banco sobe com tabelas + `inserts/inserts.txt` (+ entrada demo) | Demo com dados reais (logins Ana/Carlos/Mariana) |
| Anderson avisar no grupo | Contrato estável o bastante |

Se o Negócio ainda não expuser um método, **não invente SQL na Web** — peça o método ao Anderson.

---

## 5. Passo a passo de implementação

### Passo 1 — Ambiente

1. .NET 8 SDK (ou a versão do `.csproj`).  
2. SQL Server local (mesmo do Geovanni) — connection string em `appsettings.Development.json`.  
3. Referência de projeto: Web → Negocio → … (já deve estar na solution).

### Passo 2 — Login

1. `AccountController`: GET login (form), POST valida via Negócio.  
2. Guardar `funcionarioId` (e nome) em sessão.  
3. Filtro/`[Authorize]` simples ou checagem manual nas actions protegidas.

### Passo 3 — Cadastros

| Tela | Actions | Observação |
|------|---------|------------|
| Fornecedores | Index, Create | Campos: nome, CNPJ, telefone |
| Produtos | Index, Create, Details | Código, nome, categoria, fornecedor |

Validação: Data Annotations no ViewModel + mensagens amigáveis.

### Passo 4 — Entrada de estoque

Formulário:

- Funcionário (da sessão)  
- Itens: produto, **número do lote**, validade, quantidade, **preço unitário** (campo alinhado ao banco: `preco_unitario`)  

Controller monta DTO/comando e chama Negócio. Sucesso → redirect para detalhes da entrada. Erro → TempData/ModelState.

Logins de demo (após seed): ver `database/inserts/inserts.txt` (ex.: `ana.souza@pharmastock.local` / hash demo — Anderson define como validar senha no MVP).

### Passo 5 — Venda

Formulário:

- Itens: produto, quantidade, preço unitário (MVP)  
- **Sem** campo de lote na UI  

Negócio escolhe o lote; se estoque insuficiente, mostrar mensagem clara.

### Passo 6 — Responsivo (Etapa 5 do manual)

- Menu colapsável no mobile  
- Tabelas com scroll horizontal ou cards em telas estreitas  
- Testar em 375px e desktop  
- Capturas para o ABNT / Geovanna  

### Passo 7 — Polimento

- Mensagens de sucesso/erro  
- Empty states (“nenhum produto”)  
- Links entre listagem ↔ detalhe  

---

## 6. Critérios de aceite (para a Geovanna validar)

- Login com funcionário do seed funciona  
- Dá para cadastrar fornecedor e produto pela UI  
- Entrada aumenta quantidade do lote (conferir no banco ou tela de lotes)  
- Venda baixa estoque sem o usuário escolher lote  
- Layout usável no celular  
- Nenhum SQL/ADO no projeto Web  

---

## 7. Fora do seu escopo / depois

- Flutter, SPs, triggers  
- JWT, desconto, ML, LIBRAS  
- Telas só de alerta (prioridade do mobile A7) — opcional espelhar na Web se der tempo  

---

## 8. Contatos / dependências

| Precisa de | Quem |
|------------|------|
| Métodos de Negócio estáveis | Anderson |
| Schema + seed | Geovanni |
| Prioridade das telas / aceite | Geovanna |
| Protótipo visual | UX (se definido) |

Dúvida de regra (“posso mostrar o lote na venda?”) → **não** na UI de venda; alinhar com Anderson.
