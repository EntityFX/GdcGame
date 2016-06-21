CREATE TABLE [dbo].[FundsDriver]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(40) NOT NULL,
	[Description] NVARCHAR(150) NOT NULL,
	[InitialValue] decimal NOT NULL,
	[UnlockValue] decimal NOT NULL,
	[InfltaionPercent] smallint NOT NULL
)
