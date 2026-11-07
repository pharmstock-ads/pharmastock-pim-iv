CREATE OR ALTER TRIGGER dbo.trg_RecalculaVenda
ON dbo.VendaItem
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE v
       SET valor_total = COALESCE((
            SELECT SUM(vi.preco_unitario * vi.quantidade)
            FROM dbo.VendaItem vi
            WHERE vi.id_venda = v.id_venda
       ), 0)
    FROM dbo.Venda v
    WHERE v.id_venda IN (
        SELECT id_venda FROM deleted
        UNION
        SELECT id_venda FROM inserted
    );
END;
GO
