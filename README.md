# Welcome to Diol!

Diol is Dotnet IO logger.

## Project info

```
|- source
    |- Diol
        |- src
            |- core
            |- share
            |- applications
                |- ConsoleClient
                |- PlaygroundApi
                |- SignalrClient
                |- VsExtenstion
        |- tests
            |- units
            |- functional
            |- integrations
|- cicd (for future)
```

### Core functionality

[Share](/source/Diol/src/Diol.Share/) - a project for all DTO and consts (.net standard)

[Core](/source/Diol/src/Diol.Core/) - a project for mail logic

### Applications

In the repo you can find several applications. 

#### Playground api

This is an example of general [asp.net core api app](/source/Diol/src/applications/Diol.applications.PlaygroundApi/) with different scenarios of logs generation:

* Http call with default client
* Http call with named client
* Multiple calls
* etc.

You can use this app as an example how to setup consumer app (`appsettings.json` and `startup.cs` files.) 

#### Console client

This is [an example](/source/Diol/src/applications/Diol.applications.ConsoleClient/) how to read logs outside of a process in realtime.

You can use it as a reference to build your own solution.

#### Signalr client

This is [the SignalR project](/source/Diol/src/applications/Diol.applications.SignalrClient/). You can subscribe from your app to the hub and receive all required logs and make a really good UX. 

### Tests

Coming soon...

## How to build

No additional action is required. you can build it with VS.

## How to run

Sample demo:
1. Open [PlaygroundApi](/source/Diol/src/applications/Diol.applications.PlaygroundApi/) from a terminal and run it with command: `dotnet run`. You should see the link for a web app. 
2. Navigate to swagger page: `localhost:[your-port]/swagger`
3. Open [ConsoleClient](/source/Diol/src/applications/Diol.applications.ConsoleClient/) from a terminal and run it with command: `dotnet run`
4. From swagger page call any API. 
5. in `ConsoleClient` terminal you should see logs.

## How to contribute

Coming soon...