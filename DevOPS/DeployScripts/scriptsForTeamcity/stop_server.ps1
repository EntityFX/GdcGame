$Username = 'Administrator'
$Password = 'P@ssw0rd'
$pass = ConvertTo-SecureString -AsPlainText $Password -Force
$Cred = New-Object System.Management.Automation.PSCredential -ArgumentList $Username,$pass

$TEAMCITY_HOME = $env:TEAMCITY_HOME
#$scriptPath = echo $env:GDCame_Bins'\cmds\stop_server.cmd'
$scriptPath = echo $env:GDCame_Bins'\EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.exe'
$scriptBlock = [Scriptblock]::Create($scriptPath)

$serverlist = Get-Content $TEAMCITY_HOME\scripts\server_list.txt

foreach ($server in $serverlist) {

	#Invoke-command -Computername $server -Credential $Cred -scriptblock $scriptBlock -ArgumentList {$server}
	Invoke-command -Computername $server -Credential $Cred -scriptblock $scriptBlock -ArgumentList {"stop"}
	Invoke-command -Computername $server -Credential $Cred -scriptblock $scriptBlock -ArgumentList {"uninstall"}
}