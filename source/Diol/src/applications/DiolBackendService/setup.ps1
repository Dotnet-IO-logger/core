$serviceName = 'DiolBackendService'
$serviceDisplayName = 'Diol Backend Service'
$exeName = 'DiolBackendService.exe'
$url = "http://localhost:62023/"

$currentDir = (Get-Item .).FullName

$exePath = $currentDir + '\' + $exeName

# check if the service exist
$service = Get-Service -Name $serviceName -ErrorAction SilentlyContinue

if ($service.Length -gt 0) {
	Write-Host "Service $serviceName already exists. We are removing it" -ForegroundColor Yellow
	Stop-Service -Name $serviceName
	sc.exe delete $serviceName
	Write-Host "Service $serviceName ahs removed" -ForegroundColor Green
}

Write-Host "Creating service $serviceName" -ForegroundColor Yellow

# before start we also need to provide a user credentials for the service
# user name should contains domain name (sometimes it is the same as computer name)
New-Service -Name $serviceName -BinaryPathName "$exePath --urls=$url --contentRoot $currentDir" -DisplayName $serviceDisplayName -StartupType Automatic

Write-Host "$serviceName has installed" -ForegroundColor Green

Start-Service -Name $serviceName

Write-Host "$serviceName has started" -ForegroundColor Green