CREATE TABLE [dbo].[UserCounter]
(
	[UserId] INT NOT NULL,
	[CounterId] INT NOT NULL,
	[Value] MONEY NOT NULL,
	[CurrentStepsCount] INT NOT NULL,
	[BonusPercentage] INT NOT NULL,
	[Inflation] INT NOT NULL,
	[CreateDateTime] DATETIME NOT NULL,
	[MiningTimeSecondsEllapsed] INT NOT NULL,
	[DelayedValue] MONEY NOT NULL,
	PRIMARY KEY ([UserId],[CounterId]),
	CONSTRAINT FK_UserCounter_User FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User] ([Id]),
	CONSTRAINT FK_UserCounter_Counter FOREIGN KEY([CounterId])
		REFERENCES [dbo].[Counter] ([Id])
)
