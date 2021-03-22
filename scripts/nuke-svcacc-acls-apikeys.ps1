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

$acls = @()
$responseAcls = (ccloud kafka acl list --cluster $($Cluster) -o json) | Out-String | ConvertFrom-Json
foreach ($acl in $responseAcls) {
  if ( $hashTableOfServiceAccountIds.ContainsKey($acl.service_account_id.substring(5)) -eq $true ) {
    $acls += $acl
  }
}



if ($DisableDryRun) {
  Write-Host "`n"

  Write-Host "Acls:`n"
  foreach($Acl in $acls) {
    $serviceAccountId = $Acl.service_account_id.substring(5)
    Write-Host "$($serviceAccountId) $($Acl.operation) $($Acl.permission) $($Acl.resource) $($Acl.type) $($Acl.name)"
    $additionalArgs = ""

    if ( $Acl.type -eq 'PREFIXED' )
    {
      $additionalArgs += "--prefix"
    } 

    Switch ($Acl.resource)
    {
      {$_ -match 'TOPIC'} {
        $topicName = $Acl.name

        if ( $topicName -eq "'*'" )
        {
          $topicName = '''""'''
        } 

        Write-Host "ccloud kafka acl delete --cluster $($Cluster) --$($Acl.permission.ToLower()) --service-account $($serviceAccountId) --operation $($Acl.operation.ToLower()) --topic $($topicName) $($additionalArgs)"
        
        Invoke-Expression "ccloud kafka acl delete --cluster $($Cluster) --$($Acl.permission.ToLower()) --service-account $($serviceAccountId) --operation $($Acl.operation.ToLower()) --topic $($topicName) $($additionalArgs)"
      }
      {$_ -match 'GROUP'} {
        Invoke-Expression "ccloud kafka acl delete --cluster $($Cluster) --$($Acl.permission.ToLower()) --service-account $($serviceAccountId) --operation $($Acl.operation) --consumer-group $($Acl.name) $($additionalArgs)"
      }
      {$_ -match 'CLUSTER'} {
        Invoke-Expression "ccloud kafka acl delete --cluster $($Cluster) --$($Acl.permission.ToLower()) --service-account $($serviceAccountId) --operation $($Acl.operation) --cluster-scope"
      }
    }
  }

  Write-Host "Api keys:`n"
  foreach ($ApiKey in $apiKeys) {
    Invoke-Expression "ccloud api-key delete $($ApiKey.key)"
  }


  Write-Host "Service accounts:`n"
  foreach ($ServiceAccount in $serviceAccounts) {
    Invoke-Expression "ccloud service-account delete $($ServiceAccount.id)"
  }


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

  Write-Host "Acls:`n"
  foreach($Acl in $acls) {
    $serviceAccountId = $Acl.service_account_id.substring(5)
    Write-Host "$($serviceAccountId) $($Acl.operation) $($Acl.permission) $($Acl.resource) $($Acl.type) $($Acl.name)"
    $additionalArgs = ""

    if ( $Acl.type -eq 'PREFIXED' )
    {
      $additionalArgs += "--prefix"
    }

    Switch ($Acl.resource)
    {
      {$_ -match 'TOPIC'} {
        $topicName = $Acl.name

        if ( $topicName -eq "'*'" )
        {
          $topicName = '''""'''
        } 

        Write-Host "ccloud kafka acl delete --cluster $($Cluster) --$($Acl.permission.ToLower()) --service-account $($serviceAccountId) --operation $($Acl.operation.ToLower()) --topic $($topicName) $($additionalArgs)"
      }
      {$_ -match 'GROUP'} {
        Write-Host "ccloud kafka acl delete --cluster $($Cluster) --$($Acl.permission.ToLower()) --service-account $($serviceAccountId) --operation $($Acl.operation) --consumer-group $($Acl.name) $($additionalArgs)"
      }
      {$_ -match 'CLUSTER'} {
        Write-Host "ccloud kafka acl delete --cluster $($Cluster) --$($Acl.permission.ToLower()) --service-account $($serviceAccountId) --operation $($Acl.operation) --cluster-scope"
      }
    }
  }

}