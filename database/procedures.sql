-- Procedures do PharmaStock PIM IV (SQL Server).
-- Ordem: lote/movimentação antes das procedures que as chamam.
CREATE OR ALTER PROCEDURE dbo.prc_ObterOuCadastrarLote
    @pIdProduto INT,
    @pNumeroLote VARCHAR(50),
    @pValidade DATE,
    @pIdLote INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @pIdLote = NULL;

    SELECT @pIdLote = id
    FROM dbo.Lote
    WHERE id_produto = @pIdProduto
      AND numero_lote = @pNumeroLote;

    IF @pIdLote IS NULL
    BEGIN
        INSERT INTO dbo.Lote (id_produto, numero_lote, validade, quantidade)
        VALUES (@pIdProduto, @pNumeroLote, @pValidade, 0);

        SET @pIdLote = CONVERT(INT, SCOPE_IDENTITY());
    END
END;
GO

CREATE OR ALTER PROCEDURE dbo.prc_MovimentaItens
    @pIdFuncionario INT,
    @pIdProduto INT,
    @pIdLote INT,
    @pPrecoUnitario DECIMAL(15,2),
    @pQuantidade DECIMAL(15,2),
    @pTipoMovimento CHAR(1),
    @pIdEntradaItem INT = NULL,
    @pIdVendaItem INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Movimentacao
        (id_funcionario, id_produto, id_lote,
         valor_total, preco_unitario, quantidade,
         data_mov, tipo_mov, id_entrada_item, id_venda_item)
    VALUES
        (@pIdFuncionario, @pIdProduto, @pIdLote,
         @pPrecoUnitario * @pQuantidade, @pPrecoUnitario,
         @pQuantidade, CAST(GETDATE() AS DATE),
         @pTipoMovimento, @pIdEntradaItem, @pIdVendaItem);
END;
GO

CREATE OR ALTER PROCEDURE dbo.prc_RegistrarEntrada
    @pIdFuncionario INT,
    @pIdEntrada INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Entrada (id_funcionario, data_entrada, valor_total)
    VALUES (@pIdFuncionario, SYSDATETIME(), 0);

    SET @pIdEntrada = CONVERT(INT, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE dbo.prc_RegistrarEntradaItem
    @pIdEntrada INT,
    @pIdProduto INT,
    @pIdLote INT,
    @pQuantidade DECIMAL(15,2),
    @pPrecoUnitario DECIMAL(12,2)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @pIdEntradaItem INT;
    DECLARE @pIdFuncionario INT;

    SELECT @pIdFuncionario = id_funcionario
    FROM dbo.Entrada
    WHERE id_entrada = @pIdEntrada;

    INSERT INTO dbo.EntradaItem
        (id_entrada, id_produto, id_lote, quantidade, preco_unitario)
    VALUES
        (@pIdEntrada, @pIdProduto, @pIdLote, @pQuantidade, @pPrecoUnitario);

    SET @pIdEntradaItem = CONVERT(INT, SCOPE_IDENTITY());

    EXEC dbo.prc_MovimentaItens
        @pIdFuncionario = @pIdFuncionario,
        @pIdProduto = @pIdProduto,
        @pIdLote = @pIdLote,
        @pPrecoUnitario = @pPrecoUnitario,
        @pQuantidade = @pQuantidade,
        @pTipoMovimento = 'e',
        @pIdEntradaItem = @pIdEntradaItem;
END;
GO

CREATE OR ALTER PROCEDURE dbo.prc_RegistrarVenda
    @pIdFuncionario INT,
    @pIdVenda INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Venda (id_funcionario, data_venda, valor_total)
    VALUES (@pIdFuncionario, SYSDATETIME(), 0);

    SET @pIdVenda = CONVERT(INT, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE dbo.prc_RegistrarVendaItem
    @pIdVenda INT,
    @pIdProduto INT,
    @pIdLote INT,
    @pQuantidade DECIMAL(15,2),
    @pPrecoUnitario DECIMAL(12,2)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @pIdVendaItem INT;
    DECLARE @pIdFuncionario INT;

    SELECT @pIdFuncionario = id_funcionario
    FROM dbo.Venda
    WHERE id_venda = @pIdVenda;

    INSERT INTO dbo.VendaItem
        (id_venda, id_produto, id_lote, quantidade, preco_unitario)
    VALUES
        (@pIdVenda, @pIdProduto, @pIdLote, @pQuantidade, @pPrecoUnitario);

    SET @pIdVendaItem = CONVERT(INT, SCOPE_IDENTITY());

    EXEC dbo.prc_MovimentaItens
        @pIdFuncionario = @pIdFuncionario,
        @pIdProduto = @pIdProduto,
        @pIdLote = @pIdLote,
        @pPrecoUnitario = @pPrecoUnitario,
        @pQuantidade = @pQuantidade,
        @pTipoMovimento = 's',
        @pIdVendaItem = @pIdVendaItem;
END;
GO
