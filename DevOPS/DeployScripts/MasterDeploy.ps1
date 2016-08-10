param
(
	[string]$DataServerName = "Default server",
	[string]$AppServerName = "Default server",
	[string]$WebServerName = "Default server",
    
    [string]$DataServerIP = "Default ip",
	[string]$AppServerIP = "Default ip",
	[string]$WebServerIP = "Default ip",

    [string]$DataServerDeployFolder = "Default path",
	[string]$AppServerDeployFolder = "Default path",
	[string]$WebServerDeployFolder = "Default path",
    [string]$WebServerSiteDeployFolder = "Default path",
       
    [object]$ServerAccessDomainCredentials,
    [object]$ServerAccessLocalCredentials,
    [object]$ServerAccessLocalCredentialsForCopy,
    
    [string]$AssemblySourceFolder = "Default path",
    [string]$ExchangeShareFolder = "Default path",

    [string[]]$ExcludePaths,

	[switch]$OverwriteDataServerConfig,
	[switch]$OverwriteAppServerConfig,
	[switch]$OverwriteNotifyConsumerServerConfig,

	[switch]$OverwriteWebServerConfig,
	[switch]$OverwriteLayoutCshtml
)

echo 'Turning off services'

Invoke-Command -ComputerName $WebServerName -ScriptBlock { Import-Module WebAdministration; Stop-Website 'GdCame' } -credential $ServerAccessDomainCredentials
Invoke-Command -ComputerName $WebServerName -ScriptBlock { Stop-Service 'EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.WindowsSrv' } -credential $ServerAccessDomainCredentials
Invoke-Command -ComputerName $AppServerName -ScriptBlock { Stop-Service 'EntityFX.EconomicsArcade.Utils.ServiceHost.Manager.WindowsSrv' } -credential $ServerAccessLocalCredentials
Invoke-Command -ComputerName $DataServerName -ScriptBlock { Stop-Service 'EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv' } -credential $ServerAccessLocalCredentials

echo 'Services are turned off'

### Copy files to Data Server ###
& $WorkDir/TransferServiceFromAssembly.ps1 `
	-ServerName $DataServerName `
    -ServerIP $DataServerIP `
    -SourceFolder "$AssemblySourceFolder\DataServer" `
    -ExchangeShareFolder 'ExchangeShareFolder' `
    -DeployFolder $DataServerDeployFolder `
    -ExcludePaths $ExcludePaths `
	-Credentials $ServerAccessLocalCredentialsForCopy `
	-OverwriteConfig

### Copy files to App Server ###
& $WorkDir/TransferServiceFromAssembly.ps1 `
	-ServerName $AppServerName `
    -ServerIP $AppServerIP `
    -SourceFolder "$AssemblySourceFolder\AppServer" `
    -ExchangeShareFolder 'ExchangeShareFolder' `
    -DeployFolder $AppServerDeployFolder `
    -ExcludePaths $ExcludePaths `
	-Credentials $ServerAccessLocalCredentialsForCopy `
	-OverwriteConfig

### Copy files to Web Server (Notify Consumer) ###
& $WorkDir/TransferServiceFromAssembly.ps1 `
	-ServerName $WebServerName `
    -ServerIP $WebServerIP `
    -SourceFolder "$AssemblySourceFolder\WebServer\NotifyConsumer" `
    -ExchangeShareFolder 'ExchangeShareFolder' `
    -DeployFolder $WebServerDeployFolder `
    -ExcludePaths $ExcludePaths `
	-Credentials $ServerAccessDomainCredentials `
	-OverwriteConfig

### Copy files to Web Server (Web Client) ###
& $WorkDir/TransferServiceFromAssembly.ps1 `
	-ServerName $WebServerName `
    -ServerIP $WebServerIP `
    -SourceFolder "$AssemblySourceFolder\WebServer\GDCame" `
    -ExchangeShareFolder 'ExchangeShareFolder' `
    -DeployFolder $WebServerSiteDeployFolder `
    -ExcludePaths $ExcludePaths `
	-Credentials $ServerAccessDomainCredentials `
	-OverwriteConfig

#################################

echo 'Turning on services'

Invoke-Command -ComputerName $DataServerName -ScriptBlock { Start-Service 'EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv' } -credential $ServerAccessLocalCredentials
Invoke-Command -ComputerName $AppServerName -ScriptBlock { Start-Service 'EntityFX.EconomicsArcade.Utils.ServiceHost.Manager.WindowsSrv' } -credential $ServerAccessLocalCredentials
Invoke-Command -ComputerName $WebServerName -ScriptBlock { Start-Service 'EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.WindowsSrv' } -credential $ServerAccessDomainCredentials
Invoke-Command -ComputerName $WebServerName -ScriptBlock { Import-Module WebAdministration; Start-Website 'GdCame' } -credential $ServerAccessDomainCredentials

echo 'Services are turned on'