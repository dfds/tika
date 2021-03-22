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

$responseServiceAccounts = (ccloud service-account list -o json) | Out-String | ConvertFrom-Json
$serviceAccounts = $responseServiceAccounts | Where-Object { $_.name -like $Prefix}

$responseApiKeys = (ccloud api-key list -o json) | Out-String | ConvertFrom-Json
$apiKeys = @()


$hashTableOfServiceAccountIds = @{}
foreach ($serviceAccount in $serviceAccounts) {
  $hashTableOfServiceAccountIds.Add($serviceAccount.id, $serviceAccount.name)
}

foreach ($apiKey in $responseApiKeys) {
  If ( $hashTableOfServiceAccountIds.ContainsKey($apiKey.owner) -eq $true ) {
    $apiKeys += $apiKey
  }
}

# Clear-Variable -name hashTableOfServiceAccountIds


if ($DisableDryRun) {
  Write-Host "`n"
} else {
  Write-Host "::DryRun::`n`n"

  Write-Host "ServiceAccounts to be deleted:`n"
  foreach ($ServiceAccount in $serviceAccounts) {
    Write-Host $ServiceAccount.name
    Write-Host "`n"
  }
  
  Write-Host "ApiKeys to be deleted:`n"
  foreach ($ApiKey in $apiKeys) {
    Write-Host "$($ApiKey.key) owned by $($hashTableOfServiceAccountIds[$ApiKey.owner])"
    Write-Host "`n"
  }

  # Commands that will be run if this is the case
  Write-Host "`nCommands that would've been run if this wasn't a dry-run:`n"

  Write-Host "Service accounts:`n"
  foreach ($ServiceAccount in $serviceAccounts) {
    Write-Host "ccloud service-account delete $($ServiceAccount.id) --cluster $($Cluster)"
  }

  Write-Host "Api keys:`n"
  foreach ($ApiKey in $apiKeys) {
    Write-Host "ccloud api-key delete $($ApiKey.key) --cluster $($Cluster)"
  }

}