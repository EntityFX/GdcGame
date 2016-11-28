function Get-IPsByFolder
{
    param(
        [CmdletBinding()]
        [parameter(Mandatory = $true)]
        [object[]]${Path},
        [char]${Separator} = '/',
        [string]$OutputPath = 'ipAddresses.json',
        [int]$Timeout = 120
    )

    $WorkDir = Split-Path $script:MyInvocation.MyCommand.Path

    $startedSrevers = 0
    $startTime = get-date
    $virtualMachines = $Path | get-vm
    while($startedSrevers -ne $virtualMachines.Count)
    {
        $virtualMachines = $Path | get-vm
        $ipAdresses = $virtualMachines.Guest.IPAddress | where-object {$_.Length -le 15}

        $startedSrevers = $ipAdresses.Count

        if($(New-TimeSpan $startTime $(Get-Date)).TotalSeconds -gt $Timeout)
        {
            break;
        }

        Write-Verbose "Getted ip's is:`r`n$ipAdresses"
        sleep 10
    }

    $ipAdresses | convertto-json | out-file $OutputPath -append
}