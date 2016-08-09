param
(
	[string]$ServerName = "Default server",
    [string]$ServerIP = "Default ip",
    
    [string]$SourceFolder = "Default path s",
    [string]$ExchangeShareFolder = "Default path ex",
    [string]$DeployFolder = "Default path f",

    [string[]]$ExcludePaths,

    [object]$Credentials,

	[switch]$OverwriteConfig
)

$scriptBlock =
{
    param ($ExchangeShareFolder,$DeployFolder,$ExcludePaths);
    $Source = "\\localhost\$ExchangeShareFolder";
    $Dest   = $DeployFolder;

    if($Dest -ne '')
        { remove-item -path $Dest\* -Exclude $ExcludePaths -Recurse -Force }
    copy-item -path $Source\* -destination $Dest -Exclude $ExcludePaths -Recurse -Force;

    if($Source -ne '')
        { remove-item -path $Source\* -Recurse -Force }
}

$Source = "$SourceFolder\*"
$Dest   = "\\$ServerName\$ExchangeShareFolder"

    New-PSDrive –Name TempShare –PSProvider FileSystem –Root $Dest -Credential $Credentials
copy-item -path $Source -destination $Dest -Exclude $ExcludePaths -Recurse -Force
    Remove-PSDrive -Name TempShare

Invoke-Command -ComputerName $ServerName -ScriptBlock $scriptBlock -ArgumentList ($ExchangeShareFolder,$DeployFolder,$ExcludePaths) -credential $Credentials