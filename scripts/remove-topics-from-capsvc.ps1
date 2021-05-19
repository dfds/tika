[CmdletBinding()]

param(    
  [Parameter(Mandatory = $False, Position = 0, ValueFromPipeline = $false)]
  [string]
  $TopicsPath = "",

  [Parameter(Mandatory = $False, Position = 1, ValueFromPipeline = $false)]
  [switch]
  $DisableDryRun = $false,

  [Parameter(Mandatory = $False, Position = 2, ValueFromPipeline = $false)]
  [string]
  $Token = "",

  [Parameter(Mandatory = $False, Position = 3, ValueFromPipeline = $false)]
  [string]
  $CapsvcEndpoint = ""
)

$Topics = Get-Content $TopicsPath | Out-String | ConvertFrom-Csv

if ($DisableDryRun) {

  Write-Host "Topics:`n"
  foreach ($Topic in $topics) {
    Write-Host $Topic
    $headers=@{}
    $headers.Add("authorization", "Bearer $($Token)")
    Invoke-WebRequest -Uri "$($CapsvcEndpoint)/api/v1/topics/$($Topic.name)?clusterId=$($Topic.clusterUUID)" -Method DELETE -Headers $headers    
  }
  
  Write-Host "`n"

} else {
  Write-Host "::DryRun::`n"

  Write-Host "CapSvc endpoint: $($CapsvcEndpoint)"

  Write-Host "Topics to be deleted:"
  foreach ($Topic in $Topics) {
    Write-Host "$($Topic.name) - $($Topic.clusterUUID)"
  }

}