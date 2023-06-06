# Signal R client

## Introduction

This is backend service for Diol and works based on Signal R. 

You can use it if you don't want to handle aggregation and parsing logic. 

## Installation

By default you can use this app as a regular asp.net core app.

However, you can also install it as a windows service. 

To do it, you need to run the following command:

```ps
sc.exe create "DiolBackendService" binPath= \bin\Release\net6.0\Diol.applications.SignalrClient.exe DisplayName= "Diol Backend Service"
```

After you should see DiolBackendService in the list of services.

You can navigate to `http://locahost:9200` and use the app.

## Usage

### How to connect to the hub

coming soon...

### How to subscribe to a dotnet process and recieve logs

coming soon...

### List of events

### List of commands

## Additional things

Remove services:

```ps
sc.exe delete DiolBackendService
```