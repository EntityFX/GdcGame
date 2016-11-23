$Username = 'Administrator'
$Password = 'P@ssw0rd'
$pass = ConvertTo-SecureString -AsPlainText $Password -Force
$Cred = New-Object System.Management.Automation.PSCredential -ArgumentList $Username,$pass

$binsVariableName = 'GDCame_Bins'
$TEAMCITY_HOME = $env:TEAMCITY_HOME

$serverlist = Get-Content $TEAMCITY_HOME\scripts\server_list.txt

foreach ($server in $serverlist) {

	$remoteBinsPath = @(gwmi win32_environment -computername $server | Where-Object {$_.Name -eq $binsVariableName} | select VariableValue).VariableValue
	$importScriptPath = echo $remoteBinsPath'\mongo-scripts\import.cmd'
	$scriptBlock = [Scriptblock]::Create($importScriptPath)

	Invoke-command -Computername $server -Credential $Cred -scriptblock $scriptBlock -ArgumentList {$server}
}
