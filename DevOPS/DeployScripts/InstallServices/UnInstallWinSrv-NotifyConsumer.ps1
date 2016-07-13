$exeFile = "EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.WindowsSrv.exe"
$installPath = "C:\Users\Administrator\Desktop\NotifyConsumer"

net stop "EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.WindowsSrv"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u $installPath\$exeFile