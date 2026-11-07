# Banco de dados — PharmaStock PIM IV

Scripts do banco de dados oficial do projeto, desenvolvido em **Microsoft SQL Server**.

## Estrutura

- `schema.sql`: criação de todas as tabelas.
- `seed.sql`: dados demonstrativos iniciais.
- `procedures.sql`: procedures de lote, entrada, venda e movimentação.
- `triggers.sql`: triggers de atualização do estoque e dos totais.
- `tabelas/`, `inserts/`, `procedure/` e `trigger/`: scripts individuais, separados por responsabilidade.

Os arquivos consolidados são destinados à instalação inicial. Os arquivos individuais facilitam a manutenção pontual. Ao alterar um script individual, atualize também o arquivo consolidado correspondente para manter ambos sincronizados.

## Ordem de execução

1. Crie/seleciona o banco `PharmaStock`.
2. Execute `schema.sql`.
3. Execute `procedures.sql`.
4. Execute `triggers.sql`.
5. Execute `seed.sql`.

Execute os scripts no SQL Server Management Studio ou ferramenta compatível com o separador `GO`.

## Quantidades

As quantidades de `EntradaItem`, `VendaItem`, `Movimentacao` e `Lote` utilizam `DECIMAL(15,2)`, permitindo registrar quantidades fracionárias. Os parâmetros correspondentes das procedures também utilizam `DECIMAL(15,2)`.

## Procedures

- `prc_ObterOuCadastrarLote`: localiza um lote existente ou cadastra um novo e devolve seu identificador.
- `prc_MovimentaItens`: registra uma movimentação de entrada (`e`) ou saída (`s`).
- `prc_RegistrarEntrada`: cria o cabeçalho da entrada.
- `prc_RegistrarEntradaItem`: registra o item da entrada e gera a movimentação de estoque.
- `prc_RegistrarVenda`: cria o cabeçalho da venda.
- `prc_RegistrarVendaItem`: registra o item da venda e gera a movimentação de estoque.

## Triggers

- `trg_MovimentaItens`: atualiza a quantidade do lote após inserção ou exclusão de movimentações.
- `trg_RecalculaEntrada`: recalcula o valor total da entrada após inserção, atualização ou exclusão de itens.
- `trg_RecalculaVenda`: recalcula o valor total da venda após inserção, atualização ou exclusão de itens.

## Observações

- Os lotes inseridos pelo `seed.sql` começam com quantidade zero. O estoque é alterado por movimentações.
- As triggers de recálculo consideram todos os IDs de entrada/venda afetados pelo comando, sem depender de uma variável escalar única.
- A trigger `trg_MovimentaItens` foi mantida conforme a implementação atual do projeto. Antes de utilizar inserções/exclusões em lote que afetem várias movimentações do mesmo lote em um único comando, recomenda-se ampliar e testar sua lógica para agregação multi-linha.
- As regras de negócio, como validação de estoque disponível, devem permanecer na camada de serviço da aplicação, conforme a divisão de responsabilidades adotada pelo projeto.
