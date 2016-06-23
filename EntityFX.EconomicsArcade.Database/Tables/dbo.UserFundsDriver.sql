CREATE TABLE [dbo].[UserFundsDriver]
(
	[UserId] INT NOT NULL,
	[FundsDriverId] INT NOT NULL,
	[Value] DECIMAL NOT NULL,
	[BuyCount] INT  NOT NULL,
	PRIMARY KEY ([UserId],[FundsDriverId]),
	CONSTRAINT FK_User FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User] ([Id]),
	CONSTRAINT FK_FundsDriver FOREIGN KEY ([FundsDriverId])
		REFERENCES [dbo].[FundsDriver] ([Id])
)
