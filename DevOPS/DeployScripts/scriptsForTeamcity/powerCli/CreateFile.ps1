Function CreateFile
{
    param
    (
        [Parameter(Mandatory=$true)][string]$InputPath = "Default input path", 
        [string]$DefaultLoggingFileName = "DefaultLogName.log",
	[switch]$Force = $False
    )

    if([System.IO.Path]::GetExtension($InputPath) -ne "")
    {
        $pathIsFile = $True
    }
    else
    {
        $pathIsFile = $False
    }

    if($pathIsFile -eq $False)
    {
        $InputPath = Join-Path $InputPath $DefaultLoggingFileName
    }

    $pathExist = Test-Path $InputPath

    if($pathExist -eq $False -or $Force)
    {
        New-Item $InputPath -type file -Force | Out-Null
    }

	return $InputPath
}