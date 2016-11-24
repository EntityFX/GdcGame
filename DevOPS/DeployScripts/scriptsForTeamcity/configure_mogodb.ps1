$TEAMCITY_HOME = $env:TEAMCITY_HOME

$serverlist = Get-Content $TEAMCITY_HOME\scripts\server_list.txt

foreach ($server in $serverlist) {
	.\import.cmd $server
}
