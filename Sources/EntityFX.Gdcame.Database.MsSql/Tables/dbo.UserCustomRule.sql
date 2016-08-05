CREATE TABLE dbo.UserCustomRule (
  UserId INT NOT NULL
 ,CustomRuleId INT NOT NULL
 ,FundsDriverId INT NOT NULL
 ,CurrentIndex INT NULL
 ,CONSTRAINT PK_UserCustomRule PRIMARY KEY CLUSTERED (UserId, CustomRuleId, FundsDriverId)
 ,CONSTRAINT FK_UserCustomRule FOREIGN KEY (UserId, FundsDriverId) REFERENCES dbo.UserFundsDriver (UserId, FundsDriverId)
 ,CONSTRAINT FK_UserCustomRule_CustomRule_Id FOREIGN KEY (CustomRuleId) REFERENCES dbo.CustomRule (Id)
 ,CONSTRAINT FK_UserCustomRule_FundsDriver_Id FOREIGN KEY (FundsDriverId) REFERENCES dbo.FundsDriver (Id)
 ,CONSTRAINT FK_UserCustomRule_User_Id FOREIGN KEY (UserId) REFERENCES dbo.[USER] (Id)
) ON [PRIMARY]
GO