# Gera as pastas de plataforma do Flutter (android/, windows/, ios/, web/)
# e libera HTTP sem TLS no Android (a Api roda em http://...:5152 no Dev).
#
# Uso (dentro de mobile/):
#   powershell -ExecutionPolicy Bypass -File .\configurar.ps1

$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

flutter create . --project-name pharmastock_mobile --org br.unip.pharmastock --platforms android,ios,windows,web
flutter pub get

$manifest = Join-Path $PSScriptRoot 'android\app\src\main\AndroidManifest.xml'
if (Test-Path $manifest) {
    $xml = [IO.File]::ReadAllText($manifest)

    if ($xml -notmatch 'android.permission.INTERNET') {
        $xml = $xml -replace '(<manifest[^>]*>)', ('$1' + "`r`n" + '    <uses-permission android:name="android.permission.INTERNET"/>')
    }
    if ($xml -notmatch 'usesCleartextTraffic') {
        $xml = $xml -replace '<application', '<application android:usesCleartextTraffic="true"'
    }

    [IO.File]::WriteAllText($manifest, $xml)
    Write-Host 'AndroidManifest.xml ajustado (INTERNET + cleartext HTTP).'
}

Write-Host ''
Write-Host 'Pronto. Suba a Api e rode, por exemplo:'
Write-Host '  flutter run                         (emulador Android)'
Write-Host '  flutter run -d windows --dart-define=API_BASE_URL=http://localhost:5152/api'
