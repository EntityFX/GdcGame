########################   OPTIONS     ############################
$dataServerFolder = "DataServer"
$appServerFolder = "AppServer"
$webServerFolder = "WebServer\NotifyConsumer"

$binFolder = "..\..\bin"

###################################################################

# Determine script location for PowerShell
$ScriptDir = Split-Path $script:MyInvocation.MyCommand.Path
Write-Host "Current script directory is $ScriptDir"

$WorkDir = $ScriptDir


remove-item $binFolder\$dataServerFolder -force
remove-item $binFolder\$appServerFolder -force
remove-item $binFolder\$webServerFolder -force

new-item -itemtype directory -path $binFolder\$dataServerFolder
new-item -itemtype directory -path $binFolder\$appServerFolder
new-item -itemtype directory -path $binFolder\$webServerFolder

Read-Host "Press Enter to continue"