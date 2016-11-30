$WorkDir = Split-Path $script:MyInvocation.MyCommand.Path
. $WorkDir\Run-VSphere-Clients.ps1
. $WorkDir\GetIPFunc.ps1
. $WorkDir\CreateFile.ps1
$OutputPathForInternal = CreateFile -InputPath "$WorkDir\nodesInternalIpAddresses.json" -Force
$OutputPathForExternal = CreateFile -InputPath "$WorkDir\nodesExternalIpAddresses.json" -Force

Add-PSSnapin  VMware.VimAutomation.Core #&"C:\Program Files (x86)\VMware\Infrastructure\vSphere PowerCLI\Scripts\Initialize-PowerCLIEnvironment.ps1"
Connect-VIServer vcenter.russia.local

$ActiveFolder = Get-FolderByPath "KAZ03/Lab/AS-GDC-Gdcame/app"
$($ActiveFolder | get-vm).Name

Start-VMsByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/app"

Get-IPsByFolder -Path $ActiveFolder -MachineNameStartsAt "node" -OutputPath $OutputPathForInternal -IPStartsWith "10.10." -Timeout 240 -Verbose
Get-IPsByFolder -Path $ActiveFolder -MachineNameStartsAt "node" -OutputPath $OutputPathForExternal -IPStartsWith "169." -Timeout 240 -Verbose