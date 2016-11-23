$TEAMCITY_HOME = $env:TEAMCITY_HOME

$serverlist = Get-Content $TEAMCITY_HOME\scripts\server_list.txt

$mongoScripts = 'C:\TeamCity\buildAgent\work\47b557396722e8a\bin\mongo-scripts'

cd $mongoScripts

foreach ($server in $serverlist) {
	.\import.cmd $server
}
