$ErrorActionPreference = "Stop"

# This script syncs user secrets from a .csproj file to the user's secrets store.
Write-Host "Running script: $($MyInvocation.MyCommand.Source)" -ForegroundColor Cyan

$Location = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectName = "WorkflowsEx"
$Csproj = "$Location\..\$ProjectName.csproj"

Write-Host "Syncing secrets from $Csproj" -ForegroundColor Green

if (-not (Test-Path -IsValid $Csproj)) {
    Write-Host "Could not find csproj file at $Csproj" -ForegroundColor Red
}

[xml]$CsprojXml = Get-Content $Csproj
$SecretId = $CsprojXml.Project.PropertyGroup.UserSecretsId

Write-Host "UserSecretsId: $SecretId"

$SecretsConfig = @{};

$SecretsConfig.UserSecretsId = $SecretId;
$SecretsConfig.CsprojFileName = "$ProjectName";

$SecretsConfig.Config = @{};
$SecretsConfig.Config.Data1 = "Data1";
$SecretsConfig.Config.Data2 = "Data2";

$SecretsConfig.Config.Data3 = @("Data3", "Data4", "Data5");

$SecretsPath = "$env:APPDATA\Microsoft\UserSecrets\$SecretId\secrets.json"

Write-Host "Saving secrets to $SecretsPath"
$json = $SecretsConfig | ConvertTo-Json -Depth 10;
Set-Content -Path $SecretsPath -Value $json -Encoding UTF8

Write-Host "Secrets synced successfully." -ForegroundColor Green
