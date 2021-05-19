[CmdletBinding()]

param(    
  [Parameter(Mandatory = $False, Position = 0, ValueFromPipeline = $false)]
  [string]
  $TopicsPath = "",

  [Parameter(Mandatory = $False, Position = 1, ValueFromPipeline = $false)]
  [switch]
  $DisableDryRun = $false
)

$Topics = Get-Content $TopicsPath | Out-String | ConvertFrom-Csv


if ($DisableDryRun) {

  Write-Host "Topics:`n"
  foreach ($Topic in $topics) {
    Write-Host $Topic
    Invoke-Expression "ccloud kafka topic delete $($Topic.name) --cluster $($Topic.clusterId) --environment $($Topic.environmentId)"
  }
  
  Write-Host "`n"

} else {
  Write-Host "::DryRun::`n`n"  

  # Commands that will be run if this is the case

  Write-Host "`nCommands that would've been run if this wasn't a dry-run:`n"

  Write-Host "Topics:`n"
  foreach ($Topic in $Topics) {
    Write-Host "ccloud kafka topic delete $($Topic.name) --cluster $($Topic.clusterId) --environment $($Topic.environmentId)"
  }

}