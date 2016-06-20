CREATE TABLE [dbo].[Incrementor]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Type] smallint NOT NULL,
	[Value] decimal NOT NULL,
	CONSTRAINT FK_FundsDriver FOREIGN KEY ([Id])
    REFERENCES [dbo].[FundsDriver] ([Id])
)
