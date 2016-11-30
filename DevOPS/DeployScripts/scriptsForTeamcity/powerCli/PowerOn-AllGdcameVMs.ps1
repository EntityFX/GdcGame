. $WorkDir\Run-VSphere-Clients.ps1

Add-PSSnapin  VMware.VimAutomation.Core
Connect-VIServer vcenter.russia.local


Start-VMsByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/app"
Start-VMsByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/clients"
Start-VMsByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/dev"