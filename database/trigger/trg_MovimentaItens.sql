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
