param(
    [string]$RootPath = (Get-Location).Path,
    [switch]$DryRun
)

Write-Host "Searching for bin and obj directories in: $RootPath" -ForegroundColor Cyan

$foldersToDelete = Get-ChildItem -LiteralPath $RootPath -Directory -Recurse -Force |
    Where-Object { $_.Name -eq "bin" -or $_.Name -eq "obj" } |
    Sort-Object FullName -Descending

if (-not $foldersToDelete) {
    Write-Host "No bin or obj directories found." -ForegroundColor Yellow
    exit 0
}

Write-Host "Found $($foldersToDelete.Count) directories to delete:" -ForegroundColor Yellow

foreach ($folder in $foldersToDelete) {
    Write-Host $folder.FullName
}

if ($DryRun) {
    Write-Host ""
    Write-Host "Dry run mode enabled. No directories were deleted." -ForegroundColor Cyan
    exit 0
}

Write-Host ""
Write-Host "Deleting directories..." -ForegroundColor Red

foreach ($folder in $foldersToDelete) {
    try {
        Remove-Item -LiteralPath $folder.FullName -Recurse -Force -ErrorAction Stop
        Write-Host "Deleted: $($folder.FullName)" -ForegroundColor Green
    }
    catch {
        Write-Host "Failed to delete: $($folder.FullName)" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor DarkRed
    }
}

Write-Host ""
Write-Host "Done." -ForegroundColor Green