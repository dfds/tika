[CmdletBinding()]

param(    
  [Parameter(Mandatory = $False, Position = 0, ValueFromPipeline = $false)]
  [string]
  $Prefix = "",

  [Parameter(Mandatory = $False, Position = 1, ValueFromPipeline = $false)]
  [string]
  $Cluster = "",

  [Parameter(Mandatory = $False, Position = 2, ValueFromPipeline = $false)]
  [switch]
  $DisableDryRun = $false
)

$responseTopics = (ccloud kafka topic list --cluster $($Cluster) -o json) | Out-String | ConvertFrom-Json
$topics = $responseTopics | Where-Object { $_.name -like $Prefix}

if ($DisableDryRun) {

  Write-Host "Topics:`n"
  foreach ($Topic in $topics) {
    Write-Host $Topic
    Invoke-Expression "ccloud kafka topic delete $($Topic.name) --cluster $($Cluster)"
  }
  
  Write-Host "`n"

} else {
  Write-Host "::DryRun::`n`n"

  Write-Host "Topics to be deleted:`n"
  foreach ($Topic in $topics) {
    Write-Host $Topic.name
    Write-Host "`n"
  }
  

  # Commands that will be run if this is the case

  Write-Host "`nCommands that would've been run if this wasn't a dry-run:`n"

  Write-Host "Topics:`n"
  foreach ($Topic in $topics) {
    Write-Host "ccloud kafka topic delete $($Topic.name) --cluster $($Cluster)"
  }

}