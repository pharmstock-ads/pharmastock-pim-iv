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
