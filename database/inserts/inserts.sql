-- Dados demonstrativos iniciais do PharmaStock.
-- Os lotes começam com quantidade zero; o estoque é atualizado pelas movimentações.

INSERT INTO dbo.Fornecedor (nome_empresa, cnpj, telefone)
VALUES
    ('Distribuidora FarmaSul', '12345678000190', '(16) 3333-1000'),
    ('Medicamentos Brasil', '23456789000101', '(16) 3333-2000'),
    ('SaudeMais Distribuidora', '34567890000112', '(16) 3333-3000');
GO

INSERT INTO dbo.Funcionario
    (nome, cpf, cargo, email, senha_hash, data_admissao, ativo)
VALUES
    ('Ana Paula Souza', '12345678901', 'Farmaceutica', 'ana.souza@pharmastock.local', 'hash_demo_ana', '2025-02-10', 's'),
    ('Carlos Henrique Lima', '23456789012', 'Estoquista', 'carlos.lima@pharmastock.local', 'hash_demo_carlos', '2025-04-15', 's'),
    ('Mariana Costa Alves', '34567890123', 'Atendente', 'mariana.alves@pharmastock.local', 'hash_demo_mariana', '2025-06-01', 's');
GO

INSERT INTO dbo.Categoria (nome)
VALUES
    ('Analgesicos'),
    ('Antibioticos'),
    ('Vitaminas'),
    ('Higiene');
GO

INSERT INTO dbo.Produto
    (codigo, codbarra, nome, id_categoria, id_fornecedor)
VALUES
    (1001, '7891000100011', 'Paracetamol 500mg - 20 comprimidos',
     (SELECT id FROM dbo.Categoria WHERE nome = 'Analgesicos'),
     (SELECT id FROM dbo.Fornecedor WHERE cnpj = '12345678000190')),
    (1002, '7891000100028', 'Dipirona 500mg - 10 comprimidos',
     (SELECT id FROM dbo.Categoria WHERE nome = 'Analgesicos'),
     (SELECT id FROM dbo.Fornecedor WHERE cnpj = '23456789000101')),
    (1003, '7891000100035', 'Amoxicilina 500mg - 21 capsulas',
     (SELECT id FROM dbo.Categoria WHERE nome = 'Antibioticos'),
     (SELECT id FROM dbo.Fornecedor WHERE cnpj = '23456789000101')),
    (1004, '7891000100042', 'Vitamina C 1g - 10 comprimidos',
     (SELECT id FROM dbo.Categoria WHERE nome = 'Vitaminas'),
     (SELECT id FROM dbo.Fornecedor WHERE cnpj = '34567890000112')),
    (1005, '7891000100059', 'Sabonete antisseptico 90g',
     (SELECT id FROM dbo.Categoria WHERE nome = 'Higiene'),
     (SELECT id FROM dbo.Fornecedor WHERE cnpj = '12345678000190'));
GO

INSERT INTO dbo.Lote (id_produto, numero_lote, validade, quantidade)
VALUES
    ((SELECT id FROM dbo.Produto WHERE codigo = 1001), 'PARA-2026-01', '2027-12-31', 0),
    ((SELECT id FROM dbo.Produto WHERE codigo = 1002), 'DIP-2026-01', '2027-10-31', 0),
    ((SELECT id FROM dbo.Produto WHERE codigo = 1003), 'AMOX-2026-01', '2027-08-31', 0),
    ((SELECT id FROM dbo.Produto WHERE codigo = 1004), 'VITC-2026-01', '2028-01-31', 0),
    ((SELECT id FROM dbo.Produto WHERE codigo = 1005), 'SAB-2026-01', '2028-06-30', 0);
GO
