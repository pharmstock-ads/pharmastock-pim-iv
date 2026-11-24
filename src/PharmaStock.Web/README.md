# PharmaStock.Web

ASP.NET Core **MVC (Razor)** — interface no navegador.  
Controllers só chamam `*Negocio`. **Não** chama a Api por HTTP.

## Telas (MVP)

| Área | Rotas |
|------|--------|
| Login / sessão | `/Account/Login`, logout |
| Início (hub de operações) | `/` |
| Dashboard (resumo + alertas) | `/Dashboard` |
| Fornecedores | `/Fornecedores`, `/Fornecedores/Criar` |
| Produtos | `/Produtos`, `/Produtos/Criar`, `/Produtos/Detalhe/{id}`, `/Produtos/Lotes/{id}` |
| Entrada | `/Entradas/Registrar` |
| Vendas | `/Vendas`, `/Vendas/Registrar` |

## Como rodar

1. Banco `PharmaStock` criado e scripts aplicados ([`../../database/`](../../database/)) — **inclua `triggers.sql`**.  
2. Ajuste `ConnectionStrings:Padrao` em `appsettings.json`.  
3. Na pasta `src`:

```bash
dotnet run --project PharmaStock.Web --launch-profile http
```

| Recurso | URL |
|---------|-----|
| Site | http://localhost:5238 |
| Login | http://localhost:5238/Account/Login |
| Dashboard | http://localhost:5238/Dashboard |
| Entrada | http://localhost:5238/Entradas/Registrar |
| Venda | http://localhost:5238/Vendas/Registrar |

Sem sessão, rotas protegidas redirecionam para o login.

### Login demo (seed)

- E-mail: `ana.souza@pharmastock.local`  
- Senha: `@Ana123`

### Caminho feliz (sem Scalar)

1. Login  
2. (Opcional) Novo fornecedor / novo produto  
3. **Nova entrada** — produto, lote, validade, quantidade e preço  
   - Cultura **pt-BR**: `1,00` / `10,50` são aceitos (validação + binding)  
4. **Nova venda** — sem escolher lote (FEFO no Negócio)  
5. Conferir lotes em **Produtos → Detalhe → Lotes** e histórico em **Vendas**

Seed deixa lotes com quantidade **0**; sem entrada a venda falha por estoque insuficiente.

A Api **não** precisa estar rodando para a Web. Flutter usa a Api — ver [`../PharmaStock.Api/`](../PharmaStock.Api/) e [`../../mobile/`](../../mobile/).

Papéis e status: [`../../docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md`](../../docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md).
