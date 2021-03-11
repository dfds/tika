[CmdletBinding()]

param(    
  [Parameter(Mandatory = $False, Position = 0, ValueFromPipeline = $false)]
  [string[]]
  $Topics = "",

  [Parameter(Mandatory = $False, Position = 1, ValueFromPipeline = $false)]
  [string]
  $Cluster = "",

  [Parameter(Mandatory = $False, Position = 2, ValueFromPipeline = $false)]
  [switch]
  $DisableDryRun = $false
)

if ($DisableDryRun) {

  Write-Host "Topics:`n"
  foreach ($Topic in $topics) {
    Write-Host $Topic
    Invoke-Expression "ccloud kafka topic delete $($Topic) --cluster $($Cluster)"
  }
  
  Write-Host "`n"

} else {
  Write-Host "::DryRun::`n`n"

  Write-Host "Topics to be deleted:`n"
  foreach ($Topic in $Topics) {
    Write-Host $Topic
    Write-Host "`n"
  }
  

  # Commands that will be run if this is the case

  Write-Host "`nCommands that would've been run if this wasn't a dry-run:`n"

  Write-Host "Topics:`n"
  foreach ($Topic in $Topics) {
    Write-Host "ccloud kafka topic delete $($Topic) --cluster $($Cluster)"
  }

}