########################   OPTIONS     ############################
$dataServerFolder = "AppServer"
$appServerFolder = "DataServer"
$webServerFolder = "WebServer\NotifyConsumer"

$appServerFilesFile = "filesAppServer.txt"
$dataServerFilesFile = "filesDataServer.txt"
$webServerFilesFile = "filesWebServer.txt"

###################################################################

$filesAppServer = dir ..\..\bin\$dataServerFolder | Select-Object -ExpandProperty Name
$filesAppServer | Out-File $appServerFilesFile

$filesDataServer = dir ..\..\bin\$appServerFolder | Select-Object -ExpandProperty Name
$filesDataServer | Out-File $dataServerFilesFile

$filesNotifyConsumer = dir ..\..\bin\$webServerFolder | Select-Object -ExpandProperty Name
$filesNotifyConsumer | Out-File $webServerFilesFile

Read-Host "Press Enter to continue"