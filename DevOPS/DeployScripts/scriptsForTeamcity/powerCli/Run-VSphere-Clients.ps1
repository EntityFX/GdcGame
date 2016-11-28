function Get-FolderByPath{
  <# .SYNOPSIS Retrieve folders by giving a path .DESCRIPTION The function will retrieve a folder by it's path. The path can contain any type of leave (folder or datacenter). .NOTES Author: Luc Dekens .PARAMETER Path The path to the folder. This is a required parameter. .PARAMETER Path The path to the folder. This is a required parameter. .PARAMETER Separator The character that is used to separate the leaves in the path. The default is '/' .EXAMPLE PS> Get-FolderByPath -Path "Folder1/Datacenter/Folder2"
.EXAMPLE
  PS> Get-FolderByPath -Path "Folder1>Folder2" -Separator '>'
#>
 
  param(
  [CmdletBinding()]
  [parameter(Mandatory = $true)]
  [System.String[]]${Path},
  [char]${Separator} = '/'
  )
 
  process{
    if((Get-PowerCLIConfiguration).DefaultVIServerMode -eq "Multiple"){
      $vcs = $defaultVIServers
    }
    else{
      $vcs = $defaultVIServers[0]
    }
 
    foreach($vc in $vcs){
      foreach($strPath in $Path){
        $root = Get-Folder -Name Datacenters -Server $vc
        $strPath.Split($Separator) | %{
          $root = Get-Inventory -Name $_ -Location $root -Server $vc -NoRecursion
          if((Get-Inventory -Location $root -NoRecursion | Select -ExpandProperty Name) -contains "vm"){
            $root = Get-Inventory -Name "vm" -Location $root -Server $vc -NoRecursion
          }
        }
        $root | where {$_ -is [VMware.VimAutomation.ViCore.Impl.V1.Inventory.FolderImpl]}|%{
          Get-Folder -Name $_.Name -Location $root.Parent -NoRecursion -Server $vc
        }
      }
    }
  }
}

#Add-PSSnapin  VMware.VimAutomation.Core

function Start-VMsByPath {
    param(
        [CmdletBinding()]
        [parameter(Mandatory = $true)]
        [System.String[]]${Path},
        [char]${Separator} = '/'
    )
    
    Get-FolderByPath -Path $Path | Get-VM | Where-Object {$_.PowerState -eq "PoweredOff"} | Start-VM -RunAsync
}

function Stop-VMsByPath {
    param(
        [CmdletBinding()]
        [parameter(Mandatory = $true)]
        [System.String[]]${Path},
        [char]${Separator} = '/'
    )
    
    Get-FolderByPath -Path $Path | Get-VM | Where-Object {$_.PowerState -eq "PoweredOn"} | Stop-VM -Confirm:$false -RunAsync
}

function Reboot-NotRunningVMsByPath {
    param(
        [CmdletBinding()]
        [parameter(Mandatory = $true)]
        [System.String[]]${Path},
        [char]${Separator} = '/'
    )
    
    Get-FolderByPath -Path $Path | Get-VM | Where-Object {$_.PowerState -eq "PoweredOn"} | Stop-VM -Confirm:$false -RunAsync
}

function Restart-VMsByPath {
    param(
        [CmdletBinding()]
        [parameter(Mandatory = $true)]
        [System.String[]]${Path},
        [char]${Separator} = '/'
    )
    
    Get-FolderByPath -Path $Path | Get-VM | Where-Object {$_.PowerState -eq "PoweredOn" -and $_.Guest.State -ne "Running"} | Update-Tools -NoReboot -RunAsync
    Get-FolderByPath -Path $Path | Get-VM | Where-Object {$_.PowerState -eq "PoweredOn" -and $_.Guest.State -ne "Running"} | Restart-VM -Confirm:$false
}

#Connect-VIServer vcenter.russia.local

#Start-VMsByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/clients"

#Update-VMsToolsByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/clients"

#Get-FolderByPath -Path "KAZ03/Lab/AS-GDC-Gdcame/clients" | Get-VM | Where-Object {$_.PowerState -eq "PoweredOn"} | Update-Tools

#Invoke-VMScript -VM client6.gdcame-win8.1-x86-net -ScriptText "calc" -GuestUser sysadm -GuestPassword P@ssw0rd