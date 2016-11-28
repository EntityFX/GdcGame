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
    $virtualMachines = $Path | get-vm #| Sort-Object Name | Get-Unique | where-object {$_.Name.StartsWith($MachineNameStartsAt)} 
    $virtualMachines.name
    while($startedSrevers -lt $virtualMachines.Count)
    {
        $virtualMachines = $Path | get-vm #| Sort-Object Name | Get-Unique | where-object {$_.Name.StartsWith($MachineNameStartsAt)} 
        $ipAdresses = $virtualMachines.Guest.IPAddress | where-object {$_.Length -le 15}
        $ipAdresses = $ipAdresses | select-object -unique
        $ipAdresses = $ipAdresses | where-object {$_.StartsWith($StartsWith)}

        $startedSrevers = $ipAdresses.Count

        if($(New-TimeSpan $startTime $(Get-Date)).TotalSeconds -gt $Timeout)
        {
            break;
        }

        Write-Verbose "Getted ip's is:`r`n$ipAdresses"
        Write-Verbose "Started srevers:`r`n$startedSrevers"
        Write-Verbose "Virtual machines:`r`n$($virtualMachines.Count)"
        sleep 10
    }
    
    $ipAdresses | convertto-json | out-file $OutputPath -append
}