# Guia — Web MVC (Vinicius)

**Para:** Vinicius  
**De:** Anderson (Líder · Núcleo/API)  
**Atualizado:** 06/11/2026  
**Projeto:** `src/PharmaStock.Web`  
**Status:** núcleo + API + banco **já prontos** — você pode integrar de verdade agora

---

## Documentos de referência (leia nesta ordem)

| Doc | Por quê |
|-----|---------|
| [README.md](../../README.md) | Como subir o SQL Server + connection string `Padrao` |
| [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) | Regras FEFO, auth MVP, o que é fora do escopo |
| [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) | Seu bloco = **6A Web** |
| [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md) | Só se precisar entender seed / scripts |
| Template do professor | Views Razor + DI no `Program.cs` (molde visual) |
| Código em `src/PharmaStock.Negocio/*Negocio.cs` | Métodos que você vai chamar |

**Não invente SQL na Web.** Se faltar um método no Negócio, peça ao Anderson.

---

## 1. O que você entrega (MVP Web)

Aplicação **ASP.NET Core MVC (Razor)**, **responsiva**, no navegador.

| # | Fluxo | Prioridade |
|---|--------|------------|
| 1 | Login de funcionário | Obrigatório |
| 2 | Listar / cadastrar fornecedor | Obrigatório |
| 3 | Listar / cadastrar / detalhar produto | Obrigatório |
| 4 | Registrar **entrada** (lote + validade + preço unitário) | Obrigatório |
| 5 | Registrar **venda** (produto + qtd + preço; **sem** escolher lote) | Obrigatório |
| 6 | Listar lotes de um produto (consulta) | Recomendado |
| 7 | Listar vendas | Recomendado |

Flutter (alertas A7) = Roberto. SPs/triggers = Geovanni. Você = telas MVC.

---

## 2. Arquitetura (regra que não quebra)

```
Browser → Controllers (Web) → *Negocio → Repositorio (Dapper) → SQL Server
```

| Pode | Não pode |
|------|----------|
| Injetar `AuthNegocio`, `CadastroNegocio`, `EntradaNegocio`, `VendaNegocio`, `AlertaNegocio` no controller | Usar `I*Repositorio` / `SqlConnection` / Dapper no controller ou na View |
| ViewModels + Views Razor | Hardcode de connection string no código C# |
| Layout Bootstrap (ou similar) responsivo | Mostrar “escolha de lote” na tela de venda |
| Usar DTOs de `PharmaStock.Modelos.Dtos` se quiser | Duplicar regra FEFO na View |

**DI já está no `Program.cs` da Web** (igual à Api). Não apague esse bloco. Só use as classes de Negócio nos controllers.

Connection string: chave **`Padrao`** em `appsettings.json` (mesmo banco do Anderson). Como ajustar `Server=`: [README.md](../../README.md).

---

## 3. Preparar o ambiente (checklist)

Faça **nesta ordem**:

1. [ ] Clonar o repo e abrir `src/PharmaStock.sln`  
2. [ ] Instalar **.NET 10** SDK (`dotnet --version`)  
3. [ ] Ter SQL Server + banco `PharmaStock` com scripts de `database/` (tabelas → procedure → trigger → inserts) — ver README  
4. [ ] Em `PharmaStock.Web/appsettings.json`, conferir:

```json
"ConnectionStrings": {
  "Padrao": "Server=localhost\\SQLEXPRESS;Database=PharmaStock;Trusted_Connection=True;TrustServerCertificate=True"
}
```

(Troque o `Server=` se na sua máquina for LocalDB.)

5. [ ] No terminal:

```bash
cd pharmastock-pim-iv/src
dotnet restore PharmaStock.sln
dotnet build PharmaStock.sln
dotnet run --project PharmaStock.Web
```

6. [ ] Abrir a URL que o terminal mostrar (ex. `https://localhost:7xxx`) — hoje só Home padrão; as telas você cria  

**Opcional:** subir a Api em outro terminal (`dotnet run --project PharmaStock.Api`) e abrir `/scalar` para ver os mesmos fluxos em HTTP — útil para comparar, mas a Web **não precisa** chamar a Api por HTTP; ela chama o Negócio direto.

---

## 4. Classes de Negócio que você usa

| Classe | Para quê | Métodos principais |
|--------|----------|-------------------|
| `AuthNegocio` | Login | `LoginAsync(cpfOuEmail, senha)` |
| `CadastroNegocio` | Fornecedor / produto / lotes | `ListarFornecedoresAsync`, `CriarFornecedorAsync`, `ListarProdutosAsync`, `ObterProdutoAsync`, `CriarProdutoAsync`, `ListarLotesPorProdutoAsync` |
| `EntradaNegocio` | Entrada de estoque | `RegistrarAsync(idFuncionario, itens)` |
| `VendaNegocio` | Venda FEFO | `RegistrarAsync(idFuncionario, itens)`, `ListarAsync` |
| `AlertaNegocio` | Opcional na Web | `LotesAVencerAsync`, `EstoqueBaixoAsync` |

Namespace: `PharmaStock.Negocio`.

Exemplo de injeção no controller:

```csharp
public class ProdutosController : Controller
{
    private readonly CadastroNegocio _cadastro;

    public ProdutosController(CadastroNegocio cadastro)
    {
        _cadastro = cadastro;
    }
}
```

Se `DominioException` for lançada (validação / estoque insuficiente), capture e mostre a mensagem na tela (`ModelState` ou `TempData`).

---

## 5. Login de demonstração (seed)

Depois dos `inserts.txt`:

| Campo | Valor |
|-------|--------|
| E-mail | `ana.souza@pharmastock.local` |
| Senha | `hash_demo_ana` |

(Também existem Carlos e Mariana — ver `database/inserts/inserts.txt`.)

MVP **sem JWT**: após login, guarde na **sessão** pelo menos:

- `funcionarioId`  
- `nome`  
- `cargo` (opcional)

Nas telas de entrada/venda, use o `funcionarioId` da sessão (não peça de novo no formulário, ou use hidden field).

---

## 6. Passo a passo das telas

### Passo A — Layout e menu

1. Ajuste `Views/Shared/_Layout.cshtml`.  
2. Menu sugerido: **Home · Fornecedores · Produtos · Entradas · Vendas · Sair**.  
3. Bootstrap (já costuma vir no template MVC) + navbar responsiva (hambúrguer no celular).

### Passo B — Login (`AccountController` ou `AuthController`)

| Action | O que faz |
|--------|-----------|
| GET `/Account/Login` | Mostra o form (e-mail/CPF + senha) |
| POST `/Account/Login` | Chama `_auth.LoginAsync(...)`; se ok → grava sessão → redirect Home; se erro → mensagem |
| POST `/Account/Logout` | Limpa sessão |

Proteção simples: nas actions sensíveis, se não houver `funcionarioId` na sessão → redirect para Login.

### Passo C — Fornecedores

| Tela | Action | Negócio |
|------|--------|---------|
| Lista | GET Index | `ListarFornecedoresAsync` |
| Criar | GET Create + POST Create | `CriarFornecedorAsync(nome, cnpj, telefone)` |

Campos: nome da empresa, CNPJ, telefone (opcional).

### Passo D — Produtos

| Tela | Action | Negócio |
|------|--------|---------|
| Lista | Index | `ListarProdutosAsync` |
| Detalhe | Details(id) | `ObterProdutoAsync` |
| Criar | Create | `CriarProdutoAsync` com `Produto` (`Codigo`, `Nome`, `CodBarra`, `IdCategoria`, `IdFornecedor`) |

`codigo` deve ser **único** e diferente dos seeds (1001–1005).  
`codBarra` é texto (string), não número.

### Passo E — Entrada de estoque

Formulário (um ou mais itens):

| Campo UI | Vai para |
|----------|----------|
| (sessão) funcionário | `idFuncionario` |
| Produto (select) | `IdProduto` |
| Número do lote | `NumeroLote` |
| Validade | `Validade` |
| Quantidade | `Quantidade` |
| Preço unitário | `PrecoUnitario` |

Chamada:

```csharp
var itens = new List<EntradaItemPedido> { /* ... */ };
var idEntrada = await _entrada.RegistrarAsync(funcionarioId, itens);
```

**Não** crie lote em tela separada — o lote nasce na entrada (C4).

Depois da entrada, a quantidade do lote sobe (trigger no banco). Confira listando lotes do produto.

### Passo F — Venda

Formulário:

| Campo UI | Vai para |
|----------|----------|
| (sessão) funcionário | `idFuncionario` |
| Produto | `IdProduto` |
| Quantidade | `Quantidade` |
| Preço unitário | `PrecoUnitario` |

**Sem campo de lote.** O Negócio faz FEFO (pode usar mais de um lote).

```csharp
var itens = new List<VendaItemPedido> { /* ... */ };
var idVenda = await _venda.RegistrarAsync(funcionarioId, itens);
```

Se estoque insuficiente → `DominioException` → mostre a mensagem ao usuário (não deixe a tela “quebrar”).

### Passo G — Consultas extras (recomendado)

- Lotes por produto: `ListarLotesPorProdutoAsync(produtoId)`  
- Histórico de vendas: `ListarAsync()` no `VendaNegocio`  

---

## 7. ViewModels (recomendado)

Não passe entidade crua direto para a View se ficar confuso. Crie ViewModels em `Models/` ou use DTOs de `PharmaStock.Modelos.Dtos`.

Exemplo mental: `LoginViewModel`, `FornecedorFormViewModel`, `EntradaFormViewModel` (com lista de itens).

Validação: Data Annotations + `ModelState.IsValid`.

---

## 8. Responsivo (Etapa 5 do manual)

- Menu que colapsa no celular  
- Tabelas com scroll horizontal **ou** cards em telas estreitas  
- Testar em ~375px de largura e no desktop  
- Guardar capturas para o ABNT / Geovanna  

---

## 9. Critérios de aceite

- [ ] Login com Ana (seed) funciona  
- [ ] Cadastra fornecedor e produto pela UI  
- [ ] Entrada aumenta estoque do lote  
- [ ] Venda baixa estoque **sem** o usuário escolher lote  
- [ ] Mensagem clara se estoque insuficiente  
- [ ] Layout usável no celular  
- [ ] Nenhum SQL/ADO/Dapper no controller ou View  

---

## 10. Fora do escopo (não fazer agora)

- Flutter, alterar SPs/triggers  
- JWT, Identity, BCrypt  
- Desconto, push, ML  
- Telas de alerta A7 (prioridade do mobile; na Web só se sobrar tempo)

---

## 11. Ordem sugerida de commits / PRs

1. Layout + Login + sessão  
2. Fornecedores + Produtos  
3. Entradas  
4. Vendas + listagens  
5. Ajustes responsivos  

Branch sugerida: `feature/web-mvc-vinicius` a partir de `main` (depois do merge da API).

---

## 12. Dependências / quem ajuda

| Precisa | Quem |
|---------|------|
| Métodos de Negócio / bug no FEFO | Anderson |
| Banco / seed / scripts | Geovanni |
| Prioridade de telas / aceite | Geovanna |
| Visual / protótipos | Daniel — [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md) |

Dúvida “posso deixar o usuário escolher o lote na venda?” → **Não** no MVP.