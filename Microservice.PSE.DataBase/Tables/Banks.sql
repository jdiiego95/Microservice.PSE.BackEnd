CREATE TABLE [dbo].[Banks]
(
    [BankId] INT NOT NULL IDENTITY(1,1),
    [BankCode] NVARCHAR(50) NOT NULL,
    [BankName] NVARCHAR(255) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT(1),
    [ApiUrl] NVARCHAR(500) NOT NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_Banks] PRIMARY KEY ([BankId]),
    CONSTRAINT [UK_Banks_BankCode] UNIQUE ([BankCode])
);