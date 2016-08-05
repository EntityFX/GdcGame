CREATE TABLE [dbo].[Incrementor]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Type] smallint NOT NULL,
	[Value] money NOT NULL,
	[FundsDriverId] INT NOT NULL,
	[CounterId] [int] NULL,
	CONSTRAINT FK_Incrementor_FundsDriver FOREIGN KEY ([FundsDriverId])
		REFERENCES [dbo].[FundsDriver] ([Id]),
	CONSTRAINT FK_Incrementor_Counter FOREIGN KEY([CounterId])
		REFERENCES [dbo].[Counter] ([Id])
)
