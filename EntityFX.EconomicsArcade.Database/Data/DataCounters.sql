INSERT [EntityFX.EconomicsArcade.Database].dbo.Counter (id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, type)
  VALUES (0, N'Communism', 0.00, CONVERT(BIT, 'False'), 0, NULL, 0)
INSERT [EntityFX.EconomicsArcade.Database].dbo.Counter (id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, type)
  VALUES (1, N'Production', 10.00, CONVERT(BIT, 'False'), 250, NULL, 1)
INSERT [EntityFX.EconomicsArcade.Database].dbo.Counter (id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, type)
  VALUES (2, N'Tax', 0.00, CONVERT(BIT, 'True'), 2000, NULL, 1)
INSERT [EntityFX.EconomicsArcade.Database].dbo.Counter (id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, type)
  VALUES (3, N'Five Year Plan', 5000000.00, CONVERT(BIT, 'False'), 1000, 240, 2)
GO