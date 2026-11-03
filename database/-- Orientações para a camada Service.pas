# PharmaStock — Orientações para a camada Service

## 1. Objetivo

A camada **Service** será responsável por validar as informações recebidas da interface antes de executar as procedures do banco de dados.

O Service deve garantir que os dados enviados para o banco sejam consistentes e que as operações de entrada e venda sigam as regras de negócio do sistema.

A comunicação com o banco deve ocorrer por meio da camada **Repository**, mantendo a separação de responsabilidades:

- **UI:** recebe os dados do usuário.
- **Service:** valida regras de negócio e coordena o fluxo.
- **Repository:** executa as procedures e consultas no banco.
- **Banco de dados:** persiste os dados e executa triggers e procedures.

## 2. Validações gerais

Antes de registrar uma entrada ou venda, o Service deve validar:

### 2.1 Produto e funcionário

- Verificar se o produto informado existe.
- Verificar se o funcionário informado existe.
- Verificar se o funcionário está ativo, caso o sistema utilize esse controle.
- Não permitir que a operação seja iniciada com IDs inválidos.

### 2.2 Quantidade e valores

- A quantidade informada deve ser maior que zero.
- O custo unitário de uma entrada não pode ser negativo.
- O preço unitário de uma venda não pode ser negativo.
- Validar se os valores recebidos estão em formato numérico válido.
- Evitar que a quantidade ou os valores sejam enviados como zero ou negativos por erro de preenchimento.

## 3. Validação e obtenção do lote

Antes de registrar um item de entrada ou venda, o Service deve garantir que existe um lote válido para o produto.

Para isso, utilizar a procedure:

`prc_ObterOuCadastrarLote`

Essa procedure recebe os dados necessários para localizar ou cadastrar o lote e devolve o `id_lote`.

Comportamento esperado:

- Se o lote já existir para o produto informado, a procedure retorna o ID do lote existente.
- Se o lote não existir, a procedure cadastra o lote e retorna o ID recém-criado.
- O Service deve utilizar o ID retornado nas próximas operações, sem tentar cadastrar o lote diretamente.

### Dados necessários para o lote

- Produto associado.
- Número do lote.
- Data de validade.

### Validações complementares

- O número do lote deve ser informado.
- A validade deve ser informada quando for necessário cadastrar um novo lote.
- A validade não deve estar vencida para operações de entrada, conforme regra definida pelo projeto.
- Na venda, o Service deve garantir que o lote pertence ao produto que está sendo vendido.
- Na venda, o lote deve possuir quantidade suficiente para atender à operação.

> A procedure retorna o lote correspondente ao produto. O Service deve usar esse ID para registrar o item, evitando inconsistências entre produto e lote.

## 4. Fluxo de registro de entrada

1. Receber os dados da entrada e seus itens.
2. Validar a existência do funcionário.
3. Validar os produtos de cada item.
4. Validar quantidades e custos unitários.
5. Para cada item, executar `prc_ObterOuCadastrarLote` e obter o `id_lote`.
6. Executar a procedure de registro da entrada.
7. Executar a procedure de registro dos itens da entrada, utilizando os IDs dos lotes retornados.
8. Confirmar a operação somente se todas as etapas forem concluídas com sucesso.

O Service não deve atualizar diretamente a quantidade do lote. O estoque será atualizado pelas movimentações e pela trigger responsável por movimentar o estoque.

## 5. Fluxo de registro de venda

1. Receber os dados da venda e seus itens.
2. Validar a existência do funcionário.
3. Validar os produtos de cada item.
4. Validar quantidades e preços unitários.
5. Identificar o lote utilizado para cada produto.
6. Validar se o lote pertence ao produto informado.
7. Validar se o lote possui estoque suficiente.
8. Executar a procedure de registro da venda.
9. Executar a procedure de registro dos itens da venda, informando os IDs correspondentes.
10. Confirmar a operação somente se todas as etapas forem concluídas com sucesso.

O Service deve impedir que uma venda seja registrada com quantidade superior ao estoque disponível.

## 6. Tratamento de erros e transações

- Se alguma validação falhar, interromper o fluxo e retornar uma mensagem clara para a interface.
- Se uma procedure falhar durante o registro, a operação não deve ficar parcialmente gravada.
- A camada Repository deve coordenar a transação do banco quando o fluxo envolver várias chamadas relacionadas.
- Em caso de erro, executar rollback.
- Em caso de sucesso, executar commit.
- O Service deve tratar erros de forma que a interface receba uma mensagem compreensível, sem expor detalhes técnicos do banco.

## 7. Responsabilidades que permanecem no banco

O Service não deve duplicar operações que já são realizadas pelas procedures e triggers.

O banco continuará responsável por:

- Criar os registros de entrada e venda.
- Registrar os itens das operações.
- Registrar as movimentações de estoque.
- Atualizar a quantidade do lote por meio da trigger de movimentação.
- Recalcular o valor total da entrada e da venda por meio das triggers.
- Obter ou cadastrar lotes por meio de `prc_ObterOuCadastrarLote`.

O Service não deve atualizar diretamente:

- `Lote.quantidade`
- `Entrada.valor_total`
- `Venda.valor_total`
- `Movimentacao`

## 8. Pontos para avaliar futuramente

- Definir como será escolhido o lote na venda quando houver mais de um lote disponível para o mesmo produto.
- Considerar priorizar lotes com validade mais próxima (FEFO — First Expired, First Out).
- Garantir que a validação de estoque seja segura quando houver vendas simultâneas.
- Definir se uma entrada ou venda com vários itens será tratada como uma única transação.
- Avaliar se o sistema permitirá cancelar operações já registradas, utilizando movimentações de estorno em vez de apagar registros.
- Definir regras para impedir vendas de lotes vencidos.
