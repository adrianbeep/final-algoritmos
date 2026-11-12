<#
  Arma el instalador de Sistema Coworking.

  Son dos pasos:

    1. "dotnet publish" en self-contained: deja el ejecutable, las
       dependencias (MySqlConnector, ReaLTaiizor, Microsoft.Extensions.*), el
       appsettings.json y el runtime de .NET en la carpeta publish. En
       self-contained la app arranca en cualquier PC, sin que haya que
       instalar .NET antes.

    2. WiX arma el MSI con esa carpeta adentro.

  El MSI queda en output\Instalador.msi.

  Uso:
      pwsh -File Instalador\build-installer.ps1
#>

$ErrorActionPreference = 'Stop'

$installerDir = $PSScriptRoot
$repoRoot = Split-Path $installerDir -Parent
$project = Join-Path $repoRoot 'SistemaCoworking\SistemaCoworking.csproj'
$publishDir = Join-Path $installerDir 'publish'
$outputDir = Join-Path $installerDir 'output'

Write-Host '1/2 Publicando la app en self-contained...' -ForegroundColor Cyan
dotnet publish $project `
    --configuration Release `
    --runtime win-x64 `
    --self-contained true `
    --output $publishDir `
    --nologo

if ($LASTEXITCODE -ne 0) {
    throw 'El publish fallo.'
}

Write-Host '2/2 Armando el MSI...' -ForegroundColor Cyan
dotnet build (Join-Path $installerDir 'Instalador.wixproj') `
    --configuration Release `
    -p:PublishDir="$publishDir\" `
    --output $outputDir `
    --nologo

if ($LASTEXITCODE -ne 0) {
    throw 'El build del instalador fallo.'
}

$msi = Join-Path $outputDir 'Instalador.msi'
if (-not (Test-Path $msi)) {
    throw "No se encontro el MSI en $msi"
}

$size = [math]::Round((Get-Item $msi).Length / 1MB, 1)
Write-Host ''
Write-Host "Listo: $msi ($size MB)" -ForegroundColor Green
Write-Host 'Para instalarlo: doble click en el MSI, o:' -ForegroundColor Gray
Write-Host "  msiexec /i `"$msi`"" -ForegroundColor Gray
