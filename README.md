# Welcome to Diol!

![diol-main-banner](/content/images/diol-main-banner.png)

Dotnet Input-Output Logger (`Diol`) is a **free** and **open-source** Visual Studio extension created by dotnet.

With `Diol` you can easily see and explore logs during debugging your `dotnet` application in real time.

**Stop** making tons of log messages in our code!

## Features

* **99.9%** zero touch approach. Just plug and play!
* **No need** to install any external dependencies to your project or modify your source code
* Supports next log types:
    * **HttpClient** requests and responces
    * **Aspnet core** controllers and minimal API
    * **SQL** via Entity Framework
    * **Web sockets & SignalR** (comming soon)
    * **gRPC** (comming soon)
* 3rd party **SDK** can be integrated as well (_1_)

## How to use

We have a [guide](https://github.com/Dotnet-IO-logger/core/wiki/1.-Getting-started-guide) how to install and use `Diol` but in short:

1. Start debugging your dotnet application
2. Call your endpoint
3. Watch your logs in `Diol tool window`!

![main](/content/screenshots/main.png)

### DEMOs

We have prepared DEMOs on how to set up and use DIOL, and we’ve divided them by areas.

The DEMOs are pre-set and straightforward.

You don’t need to create anything from scratch or write any code.

Please feel free to use them for your personal learning or for making DEMO for your audience.

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

Please feel free to reach out our Wiki to find more information

We are open for new ideas and help from community!

## Appendix

_1_. If they have standard internal implementation and use default logging dotnet system.