# Welcome to Diol!

![image](/content/images/diol-main-banner.png)

`D`otnet `I`nput-`O`utput `L`ogger (`Diol`) is a free and open-source tool you can easily see and explore logs during debugging your `dotnet` application in real time.

## Features

Diol handles:

* Http request and response metadata
* Aspnet core request and response metadata
* Database call with Entity Framework (coming soon)
* Web sockets (coming soon)

### Additional feature

Our backend service is available for you to integrate with your own UI app. It's free and easy to use, and it offers many features and benefits. To learn more, please visit [the link](https://github.com/Dotnet-IO-logger/core/wiki/2.-Diol-backend-service).

## How to use

It's a super easy to use Diol:

1. Install `DiolBackendService` and run it *(1)*
2. Install `Diol` from Visual studio market to your Visual Studio
3. Start debugging your dotnet application *(2)*
4. Call a function with http activities
5. Watch your logs!

*(1)* You can download `DiolBackendService` from the [link](https://github.com/Dotnet-IO-logger/core/releases). Unzip it to your folder and run `Diol.applications.SignalrClient.exe`

*(2)* To see http logs you need to inject `HttpClient` and modify `appsettings.json`. You can find more information [here](https://github.com/Dotnet-IO-logger/core/wiki/1.-Getting-started-guide)

## Feedback and contribution

Please feel free to reach out our Wiki to find more information

We are open for new ideas and help from community!