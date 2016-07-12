########################   OPTIONS     ############################
$dataServerFolder = "DataServer"
$appServerFolder = "AppServer"
$webServerFolder = "WebServer\NotifyConsumer"

$appServerFilesFile = "filesAppServer.txt"
$dataServerFilesFile = "filesDataServer.txt"
$webServerFilesFile = "filesWebServer.txt"

###################################################################

# Determine script location for PowerShell
$ScriptDir = Split-Path $script:MyInvocation.MyCommand.Path
Write-Host "Current script directory is $ScriptDir"

$WorkDir = $ScriptDir

$appServerFiles = Get-Content $appServerFilesFile
$dataServerFiles = Get-Content $dataServerFilesFile 
$webServerFiles = Get-Content $webServerFilesFile 

New-Item -Type dir ..\Deploy\$dataServerFolder -force
foreach($fileName in $dataServerFiles)
{
Copy-Item -Path ..\..\bin\$fileName -Destination ..\Deploy\$dataServerFolder -recurse -force
}

New-Item -Type dir ..\Deploy\$appServerFolder -force
foreach($fileName in $appServerFiles)
{
Copy-Item -Path ..\..\bin\$fileName -Destination ..\Deploy\$appServerFolder -recurse -force
}

New-Item -Type dir ..\Deploy\$webServerFolder -force
foreach($fileName in $webServerFiles)
{
Copy-Item -Path ..\..\bin\$fileName -Destination ..\Deploy\$webServerFolder -recurse -force
}

Read-Host "Press Enter to continue"
