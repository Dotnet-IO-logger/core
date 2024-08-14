$packageName = 'DiolBackendService'
$packagePath = '.\distrib\release\DiolBackendService'

$csProjPath = '.\DiolBackendService.csproj'

# delete old one
dotnet tool uninstall -g $packageName

# pack and install
dotnet build -c Release
dotnet pack $csProjPath --output $packagePath
dotnet tool install --global --add-source $packagePath $packageName

# how to use
# from a terminal please run the following command:
# DiolBackendService --urls=http://localhost:62023/