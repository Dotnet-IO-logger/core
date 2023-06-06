$serviceName = 'DiolBackendService'
$serviceDisplayName = 'Diol Backend Service'

$exeName = 'Diol.applications.SignalrClient.exe'

# check if the service exist
$service = Get-Service -Name $serviceName -ErrorAction SilentlyContinue

if ($service.Length -gt 0) {
	Write-Host "Service $serviceName already exists. We are removing it" -ForegroundColor Yellow
	sc.exe delete $serviceName
	Write-Host "Service $serviceName ahs removed" -ForegroundColor Green
}

Write-Host "Creating service $serviceName" -ForegroundColor Yellow
sc.exe create $serviceName binPath= $exeName DisplayName= $serviceDisplayName
Write-Host "$serviceName has installed" -ForegroundColor Green