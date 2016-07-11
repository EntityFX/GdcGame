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

New-Item -Type dir $WorkDir\$dataServerFolder -force
foreach($fileName in $dataServerFiles)
{
Copy-Item -Path ..\$fileName -Destination $WorkDir\$dataServerFolder -recurse -force
}

New-Item -Type dir $WorkDir\$appServerFolder -force
foreach($fileName in $appServerFiles)
{
Copy-Item -Path ..\$fileName -Destination $WorkDir\$appServerFolder -recurse -force
}

New-Item -Type dir $WorkDir\$webServerFolder -force
foreach($fileName in $webServerFiles)
{
Copy-Item -Path ..\$fileName -Destination $WorkDir\$webServerFolder -recurse -force
}

Read-Host "Press Enter to continue"
