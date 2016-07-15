INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES (0, N'GDC Points', 0.00, CONVERT(bit, 'False'), 1000, NULL, 0)
INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES (1, N'Профессионализм', 10.00, CONVERT(bit, 'False'), 1000, NULL, 1)
INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES (2, N'Зарплата', 0.00, CONVERT(bit, 'True'), 2000, NULL, 1)
INSERT GDCAME.dbo.Counter(Id, Name, InitialValue, UseInAutostep, InflationIncreaseSteps, MiningTimeSeconds, Type) VALUES (3, N'Квартальный план', 5000000.00, CONVERT(bit, 'False'), 1000, 240, 2)
GO