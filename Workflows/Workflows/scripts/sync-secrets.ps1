$ErrorActionPreference = "Stop"

function EnsureSecretsPathExists {
    param (
         [Parameter(Mandatory=$true)]$SecretsFilePath
    )

    if (-not (Test-Path -Path $SecretsFilePath)) {
        Write-Host "Creating secrets file at $SecretsFilePath"
        New-Item -Path $SecretsFilePath -ItemType File -Force | Out-Null
    }
}

# This script syncs user secrets from a .csproj file to the user's secrets store.
Write-Host "Running script: $($MyInvocation.MyCommand.Source)" -ForegroundColor Green

$Location = Split-Path -Parent $MyInvocation.MyCommand.Path

$CsprojFile = Get-ChildItem -Path "$Location\..\*.csproj" -Recurse -File | Select-Object -First 1

# $CsprojFile.Name | Split-Path -LeafBase # -- only if you want the base name without extension

$ProjectName = $CsprojFile.Name
$CsprojPath = $CsprojFile.FullName

Write-Host "Syncing secrets from $CsprojPath" -ForegroundColor Green

if (-not (Test-Path $CsprojPath)) {
    Write-Host "Could not find csproj file at $CsprojPath" -ForegroundColor Red
    return
}

[Xml]$CsprojPathXml = Get-Content $CsprojPath

$SecretId = $CsprojPathXml.Project.PropertyGroup.UserSecretsId

if ($null -eq $SecretId) {
    Write-Host "UserSecretsId not found in csproj file." -ForegroundColor Red
    return
}

Write-Host "UserSecretsId: $SecretId"

# Ordered dictionary to maintain the order of keys in resulting JSON
$SecretsConfig = [Ordered]@{}

$SecretsConfig.Version = "1.0.0"

$SecretsConfig.Metadata = @{}
$SecretsConfig.Metadata.UserSecretsId = $SecretId
$SecretsConfig.Metadata.CsprojFileName = $ProjectName 

$SecretsConfig.Configuration = @{}
$SecretsConfig.Configuration.Data1 = "Data1"
$SecretsConfig.Configuration.Data2 = "Data2"

$SecretsConfig.Configuration.Data3 = @("Data3", "Data4", "Data5")

$SecretsDirectory = "$env:APPDATA\Microsoft\UserSecrets\$SecretId"
$SecretsFilePath = "$SecretsDirectory\secrets.json"

EnsureSecretsPathExists -SecretsFilePath $SecretsFilePath

Write-Host "Saving secrets to $SecretsFilePath"

$json = $SecretsConfig | ConvertTo-Json -Depth 10;
Set-Content -Path $SecretsFilePath -Value $json -Encoding UTF8

Write-Host "Secrets synced successfully." -ForegroundColor Green