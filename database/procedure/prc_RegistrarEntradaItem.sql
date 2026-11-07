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
