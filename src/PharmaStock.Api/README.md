# PharmaStock.Api

ASP.NET Core **Web API** REST. Consumida pelo **Flutter** e pelo **Scalar**.  
Controllers só chamam `*Negocio` — não acessam Repositório/SQL.

## Como rodar

1. Banco `PharmaStock` criado e scripts aplicados ([`../../database/`](../../database/)).  
2. Ajuste `ConnectionStrings:Padrao` em `appsettings.json` (seu `Server=`).  
3. Na pasta `src`:

```bash
dotnet run --project PharmaStock.Api --launch-profile http
```

| Recurso | URL |
|---------|-----|
| Api | http://localhost:5152 |
| Scalar (docs interativas) | http://localhost:5152/scalar |
| Base dos endpoints | http://localhost:5152/api |

Perfil `https`: https://localhost:7275/scalar  

### Login demo (seed)

- E-mail: `ana.souza@pharmastock.local`  
- Senha: `@Ana123`

### Fluxo rápido no Scalar

1. `POST /api/auth/login`  
2. `GET /api/produtos`  
3. `POST /api/entradas` (sobe estoque; seed começa com qtd 0)  
4. `POST /api/vendas`  
5. `GET /api/alertas/lotes-a-vencer` e `/api/alertas/estoque-baixo`

Contrato: [`../../docs/pim_IV/CONTRATO_TECNICO.md`](../../docs/pim_IV/CONTRATO_TECNICO.md) §7.

### Celular físico (Flutter)

A Api precisa escutar na rede:

```bash
dotnet run --project PharmaStock.Api --urls http://0.0.0.0:5152
```

A Web MVC **não** usa esta Api — ela chama o Negócio direto. Ver [`../PharmaStock.Web/`](../PharmaStock.Web/).
