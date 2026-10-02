# Check-BodyTracking.ps1
# Verifies BodyTracking CSV files: filename fix, header, duration, tracking dropouts.
# Usage:  .\Check-BodyTracking.ps1 -Folder "C:\path\to\folder"

param(
    [string]$Folder = "C:\Users\user\AppData\LocalLow\DefaultCompany\FoG_Walkways\_LocalLogs"
)

$files = Get-ChildItem -Path $Folder -Filter "BodyTracking_*.csv" -ErrorAction SilentlyContinue |
         Sort-Object LastWriteTime

if (-not $files) {
    Write-Host "No BodyTracking_*.csv found in:" -ForegroundColor Yellow
    Write-Host "  $Folder"
    exit
}

foreach ($f in $files) {
    Write-Host ""
    Write-Host $f.Name -ForegroundColor Cyan

    # (3) filename carries milliseconds -> new build
    $hasMs = $f.BaseName -match '^BodyTracking_\d{8}_\d{6}_\d{3}$'
    $msMsg = if ($hasMs) { "OK  (new build)" } else { "NO  (old build - millisecond fix missing)" }
    Write-Host ("  filename _fff  : {0}" -f $msMsg) -ForegroundColor $(if ($hasMs) {"Green"} else {"Red"})

    $lines = Get-Content $f.FullName
    if ($lines.Count -lt 2) { Write-Host "  (empty)" -ForegroundColor Red; continue }

    # header intact
    $hasHeader = $lines[0].StartsWith("frame_index")
    Write-Host ("  header         : {0}" -f $(if ($hasHeader) {"OK"} else {"MISSING (duplicate logger?)"})) `
        -ForegroundColor $(if ($hasHeader) {"Green"} else {"Red"})

    $rows = if ($hasHeader) { $lines | Select-Object -Skip 1 } else { $lines }

    # duration
    $dur = [double]($rows[-1] -split ',')[1]
    Write-Host ("  duration       : {0:N1}s  ({1} rows)" -f $dur, $rows.Count)

    # (2) tracking dropouts: all 12 accel values exactly zero
    $zero = 0
    foreach ($r in $rows) {
        $c = $r -split ','
        if ($c.Count -lt 16) { continue }
        $allZero = $true
        for ($i = 4; $i -lt 16; $i++) { if ([double]$c[$i] -ne 0) { $allZero = $false; break } }
        if ($allZero) { $zero++ }
    }
    $pct = [math]::Round(100 * $zero / $rows.Count, 1)
    $col = if ($pct -lt 5) { "Green" } elseif ($pct -lt 20) { "Yellow" } else { "Red" }
    Write-Host ("  tracking lost  : {0}% ({1} of {2} rows all-zero)" -f $pct, $zero, $rows.Count) -ForegroundColor $col

    # per-body-part sanity: is each part actually moving?
    $parts = @{ "Head" = 4; "Hips" = 7; "LeftFoot" = 10; "RightFoot" = 13 }
    $dead = @()
    foreach ($p in $parts.GetEnumerator()) {
        $moved = $false
        foreach ($r in $rows) {
            $c = $r -split ','
            if ($c.Count -lt 16) { continue }
            if ([double]$c[$p.Value] -ne 0 -or [double]$c[$p.Value + 1] -ne 0 -or [double]$c[$p.Value + 2] -ne 0) { $moved = $true; break }
        }
        if (-not $moved) { $dead += $p.Key }
    }
    if ($dead.Count -gt 0) {
        Write-Host ("  ALL-ZERO parts : {0}" -f ($dead -join ", ")) -ForegroundColor Red
    } else {
        Write-Host "  body parts     : all 4 have data" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host ("Files found: {0}" -f $files.Count) -ForegroundColor Cyan
Write-Host "(one file = one scenario run)"
