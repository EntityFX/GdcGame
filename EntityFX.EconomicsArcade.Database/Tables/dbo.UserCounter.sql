CREATE TABLE [dbo].[UserCounter]
(
	[UserId] INT NOT NULL,
	[CounterId] INT NOT NULL,
	[Value] DECIMAL NOT NULL,
	[BonusPercentage] INT NOT NULL,
	[Bonus] DECIMAL NOT NULL,
	[Inflation] INT NOT NULL,
	[MiningTimeSecondsEllapsed] INT NOT NULL,
	[DelayedValue] DECIMAL NOT NULL,
	PRIMARY KEY ([UserId],[CounterId]),
	CONSTRAINT FK_User FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User] ([Id]),
	CONSTRAINT [FK_Counter] FOREIGN KEY([CounterId])
		REFERENCES [dbo].[Counter] ([Id])
)
