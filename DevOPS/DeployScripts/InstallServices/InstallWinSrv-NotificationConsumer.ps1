$exeFile = "EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.WindowsSrv.exe"
$installPath = "C:\Users\Administrator\Desktop\"

C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe $installPath\$exeFile
net start "EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv"

Read-Host "Press Enter to continue"