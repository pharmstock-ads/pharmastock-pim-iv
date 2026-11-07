-- PharmaStock PIM IV
-- Banco: SQL Server
-- Executar em um banco previamente criado.

CREATE TABLE dbo.Fornecedor (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome_empresa VARCHAR(100) NOT NULL,
    cnpj VARCHAR(14) NOT NULL UNIQUE,
    telefone VARCHAR(30) NULL
);
GO

CREATE TABLE dbo.Funcionario (
    id_funcionario INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(11) NULL UNIQUE,
    cargo VARCHAR(50) NULL,
    email VARCHAR(50) NULL,
    senha_hash VARCHAR(25) NOT NULL,
    data_admissao DATE NOT NULL DEFAULT SYSDATETIME(),
    ativo CHAR(1) NOT NULL CHECK (ativo IN ('s', 'n'))
);
GO

CREATE TABLE dbo.Categoria (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL
);
GO

CREATE TABLE dbo.Produto (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    codigo INT NOT NULL UNIQUE,
    codbarra VARCHAR(25) NULL UNIQUE,
    nome VARCHAR(100) NOT NULL,
    id_categoria INT NULL,
    id_fornecedor INT NULL,
    FOREIGN KEY (id_fornecedor) REFERENCES dbo.Fornecedor(id),
    FOREIGN KEY (id_categoria) REFERENCES dbo.Categoria(id)
);
GO

CREATE TABLE dbo.Lote (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    id_produto INT NOT NULL,
    numero_lote VARCHAR(50) NOT NULL,
    validade DATE NOT NULL,
    quantidade DECIMAL(15,2) NOT NULL,
    FOREIGN KEY (id_produto) REFERENCES dbo.Produto(id)
);
GO

CREATE TABLE dbo.Venda (
    id_venda INT IDENTITY(1,1) PRIMARY KEY,
    id_funcionario INT NOT NULL,
    data_venda DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    valor_total DECIMAL(10,2) DEFAULT 0,
    FOREIGN KEY (id_funcionario) REFERENCES dbo.Funcionario(id_funcionario)
);
GO

CREATE TABLE dbo.VendaItem (
    id_venda_item INT IDENTITY(1,1) PRIMARY KEY,
    id_venda INT NOT NULL,
    id_produto INT NOT NULL,
    id_lote INT NOT NULL,
    quantidade DECIMAL(15,2) NOT NULL CHECK (quantidade > 0),
    preco_unitario DECIMAL(12,2) NOT NULL CHECK (preco_unitario >= 0),
    FOREIGN KEY (id_venda) REFERENCES dbo.Venda(id_venda),
    FOREIGN KEY (id_produto) REFERENCES dbo.Produto(id),
    FOREIGN KEY (id_lote) REFERENCES dbo.Lote(id)
);
GO

CREATE TABLE dbo.Entrada (
    id_entrada INT IDENTITY(1,1) PRIMARY KEY,
    id_funcionario INT NOT NULL,
    data_entrada DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    valor_total DECIMAL(12,2) DEFAULT 0,
    FOREIGN KEY (id_funcionario) REFERENCES dbo.Funcionario(id_funcionario)
);
GO

CREATE TABLE dbo.EntradaItem (
    id_entrada_item INT IDENTITY(1,1) PRIMARY KEY,
    id_entrada INT NOT NULL,
    id_produto INT NOT NULL,
    id_lote INT NOT NULL,
    quantidade DECIMAL(15,2) NOT NULL CHECK (quantidade > 0),
    preco_unitario DECIMAL(12,2) NOT NULL CHECK (preco_unitario >= 0),
    FOREIGN KEY (id_entrada) REFERENCES dbo.Entrada(id_entrada),
    FOREIGN KEY (id_produto) REFERENCES dbo.Produto(id),
    FOREIGN KEY (id_lote) REFERENCES dbo.Lote(id)
);
GO

CREATE TABLE dbo.Movimentacao (
    id_movimentacao INT IDENTITY(1,1) PRIMARY KEY,
    id_funcionario INT NOT NULL,
    id_produto INT NOT NULL,
    id_lote INT NOT NULL,
    id_entrada_item INT NULL,
    id_venda_item INT NULL,
    valor_total DECIMAL(15,2) NOT NULL,
    preco_unitario DECIMAL(15,2) NOT NULL,
    quantidade DECIMAL(15,2) NOT NULL CHECK (quantidade > 0),
    data_mov DATE NOT NULL DEFAULT SYSDATETIME(),
    pagamento VARCHAR(30) NULL,
    tipo_mov CHAR(1) NOT NULL CHECK (tipo_mov IN ('e', 's')),
    FOREIGN KEY (id_funcionario) REFERENCES dbo.Funcionario(id_funcionario),
    FOREIGN KEY (id_produto) REFERENCES dbo.Produto(id),
    FOREIGN KEY (id_lote) REFERENCES dbo.Lote(id),
    FOREIGN KEY (id_entrada_item) REFERENCES dbo.EntradaItem(id_entrada_item) ON DELETE CASCADE,
    FOREIGN KEY (id_venda_item) REFERENCES dbo.VendaItem(id_venda_item) ON DELETE CASCADE
);
GO
