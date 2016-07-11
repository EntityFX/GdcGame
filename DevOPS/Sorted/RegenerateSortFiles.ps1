########################   OPTIONS     ############################
$dataServerFolder = "AppServer"
$appServerFolder = "DataServer"
$webServerFolder = "WebServer\NotifyConsumer"

$appServerFilesFile = "filesAppServer.txt"
$dataServerFilesFile = "filesDataServer.txt"
$webServerFilesFile = "filesWebServer.txt"

###################################################################

$filesAppServer = dir .\$dataServerFolder | Select-Object -ExpandProperty Name
$filesAppServer | Out-File $appServerFilesFile

$filesDataServer = dir .\$appServerFolder | Select-Object -ExpandProperty Name
$filesDataServer | Out-File $dataServerFilesFile

$filesNotifyConsumer = dir .\$webServerFolder | Select-Object -ExpandProperty Name
$filesNotifyConsumer | Out-File $webServerFilesFile

Read-Host "Press Enter to continue"