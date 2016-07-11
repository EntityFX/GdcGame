CREATE TABLE [dbo].[Counter]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50) NOT NULL,
	[InitialValue] MONEY NOT NULL,
	[UseInAutostep] bit NOT NULL,
	[InflationIncreaseSteps] int NOT NULL,
	[MiningTimeSeconds] int,
	[Type] [int] NOT NULL
)
