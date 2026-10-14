# estoque_farmacia_winforms

Interface **desktop** (Windows Forms) do sistema de gestão de estoque/farmácia do PIM III.

## Relação com `estoque_farmacia`

O projeto **`estoque_farmacia`** (console) partilha **Models** e **Services** via ficheiros ligados no `.csproj` (`Compile Include="..\estoque_farmacia\..." Link="..."`).

| Pasta / projeto | Papel |
|-----------------|--------|
| `estoque_farmacia` | Núcleo (serviços, modelos) + UI console |
| `estoque_farmacia_winforms` | UI gráfica |

## Stack

- .NET 10 (`net10.0-windows`)
- Windows Forms

## Como executar

```bash
dotnet run
```

## Estrutura

- `Program.cs` — arranque e primeira janela (`LoginForm`).
- `Forms/` — ecrãs (login, menu, cadastros, vendas).
- `UIHelper.cs` — estilos comuns aos formulários.
