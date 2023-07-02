# service information
$serviceName = 'DiolBackendService'
$serviceDisplayName = 'Diol Backend Service'
$exeName = 'DiolBackendService.exe'

# current user. <domain>/<username>
$username = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name

# service url
$url = "http://localhost:62023/"

# exe directory
$currentDir = (Get-Item .).FullName
$exePath = $currentDir + '\' + $exeName

# check if the service exist
$service = Get-Service -Name $serviceName -ErrorAction SilentlyContinue

# we stop and remove the service if it exists
if ($service.Length -gt 0) {
	Write-Host "Service $serviceName already exists. We are removing it" -ForegroundColor Yellow
	Stop-Service -Name $serviceName
	sc.exe delete $serviceName
	Write-Host "Service $serviceName ahs removed" -ForegroundColor Green
}

# create the service
Write-Host "Creating service $serviceName" -ForegroundColor Yellow

# before start we also need to provide a user credentials for the service
# user name should contains domain name (sometimes it is the same as computer name)
New-Service -Name $serviceName -BinaryPathName "$exePath --urls=$url --contentRoot $currentDir" -DisplayName $serviceDisplayName -StartupType Automatic -Credential $username

Write-Host "$serviceName has installed" -ForegroundColor Green

Start-Service -Name $serviceName

Write-Host "$serviceName has started" -ForegroundColor Green