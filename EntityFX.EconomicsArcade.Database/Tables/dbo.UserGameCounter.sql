CREATE TABLE [dbo].[UserGameCounter]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[TotalFunds] DECIMAL NOT NULL,
	[ManualStepsCount] INT NOT NULL,
	[AutomaticStepsCount] INT NOT NULL,
	[CategoryFunds] DECIMAL NOT NULL,
	[CreateDateTime] DATETIME NOT NULL,
	[DelayedFunds] DECIMAL NOT NULL,
	CONSTRAINT FK_User FOREIGN KEY ([Id])
		REFERENCES [dbo].[User] ([Id])
)
