# PharmaStock.Web

ASP.NET Core **MVC (Razor)** — interface no navegador.  
Controllers só chamam `*Negocio`. **Não** chama a Api por HTTP.

Telas atuais (resumo): login/sessão, hub Home, **entrada**, **venda**.

## Como rodar

1. Banco `PharmaStock` criado e scripts aplicados ([`../../database/`](../../database/)).  
2. Ajuste `ConnectionStrings:Padrao` em `appsettings.json`.  
3. Na pasta `src`:

```bash
dotnet run --project PharmaStock.Web --launch-profile http
```

| Recurso | URL |
|---------|-----|
| Site | http://localhost:5238 |
| Login | http://localhost:5238/Account/Login |
| Entrada | http://localhost:5238/Entradas/Registrar |
| Venda | http://localhost:5238/Vendas/Registrar |

Sem sessão, `/` redireciona para o login.

### Login demo (seed)

- E-mail: `ana.souza@pharmastock.local`  
- Senha: `hash_demo_ana`

### Caminho feliz

1. Login  
2. **Nova entrada** (produto do seed, lote, validade, qtd — use ponto decimal, ex. `10` ou `10.5`)  
3. **Nova venda** (sem escolher lote; FEFO no Negócio)

A Api **não** precisa estar rodando para a Web. Flutter usa a Api — ver [`../PharmaStock.Api/`](../PharmaStock.Api/) e [`../../mobile/`](../../mobile/).

Guia do papel Web: [`../../docs/pim_IV/GUIA_WEB_VINICIUS.md`](../../docs/pim_IV/GUIA_WEB_VINICIUS.md).
