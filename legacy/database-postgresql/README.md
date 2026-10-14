# database — Scripts SQL de Referência

Esta pasta contém os scripts SQL do banco de dados **pharmstock**, modelado em **PostgreSQL**. São arquivos de referência acadêmica — **não estão conectados à aplicação C# em execução**, que utiliza listas em memória como persistência.

## Arquivos

| Arquivo | Conteúdo |
|---|---|
| `-- Banco e Tabelas.txt` | Criação do banco e das 8 tabelas |
| `-- Inserts.txt` | Dados de exemplo para popular as tabelas |
| `-- Integridade Banco.txt` | Testes de restrições (CHECK, FK, UNIQUE) |
| `-- Relatorios SQL.txt` | Queries de relatório operacional |

---

## Estrutura do banco (pharmstock)

```
fornecedor
    └── produto ──── produtos_precos
                └── lote ──── movimentacao
funcionario ──── movimentacao
             └── usuario_log
categoria ──── produto
```

### Tabelas

| Tabela | Descrição |
|---|---|
| `fornecedor` | Empresas fornecedoras (nome, CNPJ, telefone) |
| `funcionario` | Colaboradores (CPF único, cargo, senha_hash, ativo) |
| `usuario_log` | Registro de logins por funcionário |
| `categoria` | Categorias de produtos (ex: Analgésico, Antibiótico) |
| `produto` | Medicamentos (código, código de barras, nome, categoria, fornecedor) |
| `produtos_precos` | Preços separados (custo bruto, margem, acréscimo, preço de venda) |
| `lote` | Lotes de cada produto com validade e quantidade |
| `movimentacao` | Entradas e saídas de estoque (`tipo_mov`: `'E'` entrada / `'S'` saída) |

---

## Relatórios disponíveis

- **Produtos a vencer nos próximos 30 dias** — ordenado por validade
- **Produtos vencidos (perdas)** — com dias vencidos calculados
- **Alerta de estoque baixo** — produtos com quantidade ≤ 50
- **Movimentação completa** — entradas e saídas com funcionário responsável

---

## Como executar

1. Ter o PostgreSQL instalado e rodando
2. Executar `-- Banco e Tabelas.txt` para criar o banco e as tabelas
3. Executar `-- Inserts.txt` para inserir dados de exemplo
4. Executar as queries de `-- Relatorios SQL.txt` para visualizar os relatórios
5. Executar `-- Integridade Banco.txt` para testar as restrições (os inserts devem falhar propositalmente)

---

> A integração entre este banco e a aplicação C# está planejada como evolução futura do projeto.
