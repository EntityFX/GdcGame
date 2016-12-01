function Get-IPsByFolder
{
    param(
        [CmdletBinding()]
        [parameter(Mandatory = $true)]
        [object[]]${Path},
        [char]${Separator} = '/',
        [string]$OutputPath = 'ipAddresses.json',
        [string]$MachineNameStartsAt = "",
        [string]$IPStartsWith,
        [int]$Timeout = 120
    )

    $WorkDir = Split-Path $script:MyInvocation.MyCommand.Path

    $startedSrevers = 0
    $startTime = get-date
    #$virtualMachines = $Path | get-vm | Sort-Object Name | Get-Unique | where-object {$_.Name.StartsWith($MachineNameStartsAt)} 
    $virtualMachines.name
    do
    {
        $virtualMachines = $Path | get-vm | Sort-Object Name | Get-Unique | where-object {$_.Name.StartsWith($MachineNameStartsAt)}
        $startedSrevers = 0#$($virtualMachines | where-object {($_.Guest.IPAddress -ne $null) -and $($($($_.Guest.IPAddress | where-object { $_.Length -le 15 }).Count) -gt 0) }).Count#$ipAdresses.Count #$_.Guest.IPAddress -ne $null

        $ipAdresses = $virtualMachines.Guest.IPAddress | where-object {$_.Length -le 15}
        $ipAdresses = $ipAdresses | select-object -unique
        $ipAdresses = $ipAdresses | where-object {$_.StartsWith($IPStartsWith)}

        if($(New-TimeSpan $startTime $(Get-Date)).TotalSeconds -gt $Timeout)
        {
            break;
        }

        Write-Verbose "Getted ip's is:`r`n$ipAdresses"
        Write-Verbose "Started srevers:`r`n$startedSrevers"
        Write-Verbose "Virtual machines:`r`n$($virtualMachines.Count)"
        sleep 10
    }
    while($startedSrevers -lt $virtualMachines.Count)
    
    $ipAdresses | convertto-json | out-file $OutputPath -append
}