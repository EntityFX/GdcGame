$TEAMCITY_HOME = $env:TEAMCITY_HOME
$DESTINATION = "SharedTest"
$serverlist = Get-Content $TEAMCITY_HOME\scripts\server_list.txt

foreach ($server in $serverlist) {
	net use X: "\\$server\$DESTINATION" /user:Administrator P@ssw0rd
	Remove-Item -Path 'X:' -recurse -Filter * -exclude 'X:\'
	net use /delete 'X:'
}

