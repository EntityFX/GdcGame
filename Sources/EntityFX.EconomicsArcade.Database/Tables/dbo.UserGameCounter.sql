CREATE TABLE [dbo].[UserGameCounter]
(
	[UserId] INT NOT NULL PRIMARY KEY,
	[TotalFunds] MONEY NOT NULL,
	[CurrentFunds] MONEY NOT NULL,
	[ManualStepsCount] INT NOT NULL,
	[AutomaticStepsCount] INT NOT NULL,
	[CreateDateTime] DATETIME NOT NULL,
	[UpdateDateTime] DATETIME NULL,
	[DelayedFunds] MONEY NOT NULL,
	CONSTRAINT FK_UserGameCounter_User FOREIGN KEY ([UserId])
		REFERENCES [dbo].[User] ([Id])
)
