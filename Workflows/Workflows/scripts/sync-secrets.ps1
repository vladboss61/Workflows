$ErrorActionPreference = "Stop"

function EnsureSecretsPathExists {
    <#
    .SYNOPSIS
    Ensures that the directory for the secrets file exists.

    .DESCRIPTION
    This function ensures that the directory for the secrets file exists.

    .PARAMETER SecretsFilePath
    The full path to the secrets file.

    .EXAMPLE
    EnsureSecretsPathExists -SecretsFilePath "C:\Path\To\Your\Secrets.json"
    #>
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
$ProjectName = "WorkflowsEx"
$CsprojPath = "$Location\..\$ProjectName.csproj"

Write-Host "Syncing secrets from $CsprojPath" -ForegroundColor Green

if (-not (Test-Path $CsprojPath)) {
    Write-Host "Could not find csproj file at $CsprojPath" -ForegroundColor Red
    return
}

[xml]$CsprojPathXml = Get-Content $CsprojPath
$SecretId = $CsprojPathXml.Project.PropertyGroup.UserSecretsId

if ($null -eq $SecretId) {
    Write-Host "UserSecretsId not found in csproj file." -ForegroundColor Red
    return
}

Write-Host "UserSecretsId: $SecretId"

$SecretsConfig = @{};

$SecretsConfig.UserSecretsId = $SecretId;
$SecretsConfig.CsprojFileName = "$ProjectName";

$SecretsConfig.Config = @{};
$SecretsConfig.Config.Data1 = "Data1";
$SecretsConfig.Config.Data2 = "Data2";

$SecretsConfig.Config.Data3 = @("Data3", "Data4", "Data5");

$SecretsDirectory = "$env:APPDATA\Microsoft\UserSecrets\$SecretId"
$SecretsFilePath = "$SecretsDirectory\secrets.json"

EnsureSecretsPathExists -SecretsFilePath $SecretsFilePath

Write-Host "Saving secrets to $SecretsFilePath"

$json = $SecretsConfig | ConvertTo-Json -Depth 10;
Set-Content -Path $SecretsFilePath -Value $json -Encoding UTF8

Write-Host "Secrets synced successfully." -ForegroundColor Green
