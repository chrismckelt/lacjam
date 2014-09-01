#Administrator privileges check
If (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
    [Security.Principal.WindowsBuiltInRole] "Administrator"))
{
    Write-Warning "You do not have Administrator rights!`nPlease run the build shell as administrator!"
    exit
}


$scriptPath = $MyInvocation.MyCommand.Path
$scriptDirectory = Split-Path $scriptPath
$ravenDir = Resolve-Path (Join-Path ($scriptDirectory) "..\")
cd $ravenDir

Write-Host "Welcome to Lacjams Build Shell!"
Write-Host 
Write-Host "From here you can..."
Write-Host "---------------------------------"
get-item "$*.bat"
Write-Host "---------------------------------"
Write-Host 
