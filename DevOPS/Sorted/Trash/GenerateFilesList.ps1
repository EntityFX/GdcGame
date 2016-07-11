$DestinationDir = ".."
$OutputFileName = "files.txt"

# Determine script location for PowerShell
$ScriptDir = Split-Path $script:MyInvocation.MyCommand.Path
Write-Host "Current script directory is $ScriptDir"

$WorkDir = $ScriptDir

$files = dir $DestinationDir | Where-Object {$_.FullName -notlike $WorkDir} | Select-Object -ExpandProperty Name

$files | Out-File $OutputFileName 


Read-Host "Press Enter to continue"