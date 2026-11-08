-- Triggers do PharmaStock PIM IV (SQL Server).
CREATE OR ALTER TRIGGER dbo.trg_MovimentaItens
ON dbo.Movimentacao
AFTER INSERT, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Aplica as movimentações inseridas.
    UPDATE l
       SET l.quantidade =
               l.quantidade +
               CASE
                   WHEN i.tipo_mov = 'e' THEN i.quantidade
                   WHEN i.tipo_mov = 's' THEN -i.quantidade
               END
      FROM dbo.Lote l
      INNER JOIN inserted i
        ON i.id_lote = l.id;

    -- Desfaz o efeito das movimentações excluídas.
    UPDATE l
       SET l.quantidade =
               l.quantidade -
               CASE
                   WHEN d.tipo_mov = 'e' THEN d.quantidade
                   WHEN d.tipo_mov = 's' THEN -d.quantidade
               END
      FROM dbo.Lote l
      INNER JOIN deleted d
        ON d.id_lote = l.id;
END;
GO

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
