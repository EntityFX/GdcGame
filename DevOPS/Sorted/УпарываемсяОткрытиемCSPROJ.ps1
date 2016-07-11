cd "C:\Users\MubarakzyanovN\Source\Repos\GdcGame"
$csprojs = Get-ChildItem -Include "*.csproj" -recurse | Select-Object -ExpandProperty FullName

foreach($csproj in $csprojs)
{
	start "C:\Program Files (x86)\Notepad++\notepad++.exe" $csproj
}

Read-Host "Press Enter to continue"