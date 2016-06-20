CREATE TABLE [dbo].[Counter]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(40) NOT NULL,
	[InitialValue] decimal NOT NULL,
	[UseInAutostep] bit NOT NULL,
	[InflationIncreaseSteps] int NOT NULL,
	[MiningTimeSeconds] int NOT NULL,
	[DelayedValue] decimal NOT NULL
)
