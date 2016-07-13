$exeFile = "EntityFX.EconomicsArcade.Utils.ServiceHost.Manager.WindowsSrv.exe"
$installPath = "C:\Users\Administrator\Desktop\AppServer"

net stop "EntityFX.EconomicsArcade.Utils.ServiceHost.Manager.WindowsSrv"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u $installPath\$exeFile