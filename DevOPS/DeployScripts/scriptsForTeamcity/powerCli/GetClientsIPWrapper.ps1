$WorkDir = Split-Path $script:MyInvocation.MyCommand.Path
. $WorkDir\Run-VSphere-Clients.ps1
. $WorkDir\GetIPFunc.ps1
. $WorkDir\CreateFile.ps1
$OutputPathForInternal = CreateFile -InputPath "$WorkDir\clientsInternalIpAddresses.json" -Force
$OutputPathForExternal = CreateFile -InputPath "$WorkDir\clientsExternalIpAddresses.json" -Force

Add-PSSnapin  VMware.VimAutomation.Core #&"C:\Program Files (x86)\VMware\Infrastructure\vSphere PowerCLI\Scripts\Initialize-PowerCLIEnvironment.ps1"
Connect-VIServer vcenter.russia.local | out-null

$ActiveFolder = Get-FolderByPath "KAZ03/Lab/AS-GDC-Gdcame/clients"

Start-VMsByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/clients" | out-null

Get-IPsByFolder -Path $ActiveFolder -MachineNameStartsAt "client" -OutputPath $OutputPathForInternal -IPStartsWith "169." -Timeout 240 -Verbose
Get-IPsByFolder -Path $ActiveFolder -MachineNameStartsAt "client" -OutputPath $OutputPathForExternal -IPStartsWith "10.10." -Timeout 240 -Verbose