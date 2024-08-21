# Welcome to Diol!

![diol-main-banner](/content/images/diol-main-banner.png)

**Dotnet Input-Output Logger (DIOL)** is a free and open-source tool created for .NET developers. With DIOL, you can easily see and explore logs during debugging your .NET application in real time. 

Say **goodbye** to tons of log messages in your code!

## Features

DIOL offers a **99.9% zero-touch approach**. Just plug and play! There’s no need to install any external dependencies to your project or modify your existing code. DIOL supports the following log types:

* **HTTP** requests and responses
* **ASP.NET Core** (controllers and minimal API)
* **SQL** (Entity Framework)
* **WebSockets & SignalR** (coming soon)
* **gRPC** (coming soon)

DIOL can also be integrated with third-party SDKs that have standard internal implementation and use the default .NET logging system

## How to use

Start debugging your .NET application, call your endpoint, and watch your logs in the DIOL tool window! [See more](https://github.com/Dotnet-IO-logger/core/wiki)

![main](/content/screenshots/main.png)

### DEMOs

We have prepared DEMOs on how to set up and use DIOL, and we’ve divided them by areas.

| Name | Duration | Description |
| :--- | :--- | :--- |
| [HttpClient demo](https://github.com/Dotnet-IO-logger/Playground/tree/main/source/Diol.Demo/src/Example1HttpLoggingSample) | ~5 minutes | How to setup DIOL to for HttpClient logs |
| [AspNET endpoints demo](https://github.com/Dotnet-IO-logger/Playground/tree/main/source/Diol.Demo/src/Example2AspnetEndpointLoggingSample) | ~5 minutes | How to setup DIOL for asp.net endpoints and controllers logs |
| [EntityFramework demo](https://github.com/Dotnet-IO-logger/Playground/tree/main/source/Diol.Demo/src/Example3EntityFrameworkLoggingSample) | ~15 minutes | How to setup DIOL for EntityFramework logs |

## Additional feature

In case if you want to reuse DIOL logic for your application you may want to check [diol backend service doc](https://github.com/Dotnet-IO-logger/core/wiki/1.-Diol-backend-service).

Highlights:

* **Easy to install**: Distributed as a dotnet cli command. Can be installed via `dotnet isntall` command
* **SignalR service**: Supports realtime notification for each log

## Feedback and contribution

We are open to new ideas and help from the community! 

Please feel free to check out our **Wiki** for more information.