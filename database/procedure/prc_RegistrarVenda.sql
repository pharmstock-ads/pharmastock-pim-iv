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
