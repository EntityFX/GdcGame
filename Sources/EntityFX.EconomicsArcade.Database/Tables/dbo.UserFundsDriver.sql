CREATE TABLE [dbo].[UserFundsDriver]
(
	[UserId] INT NOT NULL,
	[FundsDriverId] INT NOT NULL,
	[Value] MONEY NOT NULL,
	[BuyCount] INT  NOT NULL,
	PRIMARY KEY ([UserId],[FundsDriverId]),
	CONSTRAINT FK_UserFundsDriver_User FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User] ([Id]),
	CONSTRAINT FK_UserFundsDriver_FundsDriver FOREIGN KEY ([FundsDriverId])
		REFERENCES [dbo].[FundsDriver] ([Id])
)
