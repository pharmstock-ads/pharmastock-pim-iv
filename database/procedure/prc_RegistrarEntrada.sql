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
