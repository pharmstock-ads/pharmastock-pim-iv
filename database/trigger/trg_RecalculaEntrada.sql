CREATE OR ALTER TRIGGER dbo.trg_RecalculaEntrada
ON dbo.EntradaItem
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE e
       SET valor_total = COALESCE((
            SELECT SUM(ee.preco_unitario * ee.quantidade)
            FROM dbo.EntradaItem ee
            WHERE ee.id_entrada = e.id_entrada
       ), 0)
    FROM dbo.Entrada e
    WHERE e.id_entrada IN (
        SELECT id_entrada FROM deleted
        UNION
        SELECT id_entrada FROM inserted
    );
END;
GO
