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
