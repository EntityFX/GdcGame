CREATE TABLE [dbo].[FundsDriver]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(40) NOT NULL,
	[Description] NVARCHAR(150) NOT NULL,
	[InitialValue] money NOT NULL,
	[UnlockValue] money NOT NULL,
	[InflationPercent] smallint NOT NULL
)
