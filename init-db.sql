-- Attendre que SQL Server soit prêt
WAITFOR DELAY '00:00:30'

-- Créer la base de données
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ProductCatalog')
BEGIN
    CREATE DATABASE ProductCatalog;
END
GO

USE ProductCatalog;
GO

-- Créer la table Products si elle n'existe pas
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500),
        Price DECIMAL(10,2) NOT NULL,
        Stock INT NOT NULL DEFAULT 0,
        ImageUrl NVARCHAR(255)
    );
END
GO

-- Insérer des données si la table est vide
IF NOT EXISTS (SELECT * FROM Products)
BEGIN
    INSERT INTO Products (Name, Description, Price, Stock, ImageUrl) VALUES
    ('Laptop', 'High-performance laptop', 999.99, 10, 'https://picsum.photos/280/200?random=1'),
    ('Smartphone', 'Latest smartphone', 699.99, 15, 'https://picsum.photos/280/200?random=2'),
    ('Headphones', 'Wireless headphones', 199.99, 25, 'https://picsum.photos/280/200?random=3'),
    ('Tablet', '10-inch tablet', 399.99, 8, 'https://picsum.photos/280/200?random=4'),
    ('Smartwatch', 'Fitness smartwatch', 299.99, 12, 'https://picsum.photos/280/200?random=5'),
    ('Gaming Mouse', 'RGB gaming mouse', 79.99, 30, 'https://picsum.photos/280/200?random=6'),
    ('Keyboard', 'Mechanical keyboard', 149.99, 20, 'https://picsum.photos/280/200?random=7'),
    ('Monitor', '27-inch 4K monitor', 449.99, 5, 'https://picsum.photos/280/200?random=8');
END
GO