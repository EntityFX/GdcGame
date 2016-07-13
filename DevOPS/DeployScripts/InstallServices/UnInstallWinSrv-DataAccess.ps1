$exeFile = "EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv.exe"
$installPath = "C:\Users\Administrator\Desktop\DataServer"

net stop "EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u $installPath\$exeFile

Read-Host "Press Enter to continue"