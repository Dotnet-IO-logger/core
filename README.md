# Welcome to Diol!

![diol-main-banner](/content/images/diol-main-banner.png)

`D`otnet `I`nput-`O`utput `L`ogger (`Diol`) is a free and open-source tool created by dotnet.

With `Diol` you can easily see and explore logs during debugging your `dotnet` application in real time.

Let's say **stop** for tons of log messages in our code!

## Features

* **99.9%** zero touch approach. Just plug and play!
* **No need** to install any external dependencies to your project
* **No need** to modify your existing code
* Supports next log types:
    * **Http** requests and responces
    * **Aspnet core** (controllers and minimal API)
    * **SQL** (Entity Framework) 
    * **Web sockets & SignalR** (comming soon)
    * **gRPC** (comming soon)
* 3rd party **SDK** can be integrated as well (_1_)

### Additional feature

Our backend service is available for you to integrate with your own UI app. It's free and easy to use, and it offers many features and benefits. To learn more, please visit [diol backend service doc](https://github.com/Dotnet-IO-logger/core/wiki/1.-Diol-backend-service).

## How to use

We have a [guide](https://github.com/Dotnet-IO-logger/core/wiki/1.-Getting-started-guide) how to install and use `Diol` but in short:

1. Start debugging your dotnet application
2. Call your endpoint
3. Watch your logs in `Diol tool window`!

![main](/content/screenshots/main.png)

### DEMOs

We prepared **5 minute** demo projects for each DIOL feature you may want to check:

* [HttpClient demo](https://github.com/Dotnet-IO-logger/Playground/tree/main/source/Diol.Demo/src/Example1HttpLoggingSample)
* [AspNET endpoints demo](https://github.com/Dotnet-IO-logger/Playground/tree/main/source/Diol.Demo/src/Example2AspnetEndpointLoggingSample)
* **EntityFramework demo** is comming soon...

## Feedback and contribution

Please feel free to reach out our Wiki to find more information

We are open for new ideas and help from community!

## Appendix

_1_. If they have standard internal implementation and use default logging dotnet system.