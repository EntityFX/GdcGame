$DESTINATION = "SharedTest"

$serverlist = Get-Content server_list.txt
$sourceslist = Get-Content server_sources_list.txt

foreach ($server in $serverlist) {
	net use X: "\\$server\$DESTINATION" /user:Administrator P@ssw0rd

	foreach ($source in $sourceslist) {
		Copy-Item $source -destination 'X:' -recurse

	}
	
	net use /delete 'X:'
}

