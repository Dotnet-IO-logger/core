# See Your .NET Logs Clearly with DIOL: Easy Logging for HttpClient

For new .NET developers, debugging HTTP requests can be tricky. 
How long does it take to figure out what’s wrong? 
And how far can that journey take us?

Let me tell you a story.

## Backstory

> Disclaimer: This story is a work of fiction intended for humorous purposes. Any resemblance to actual events, locales, or persons, living or dead, is purely coincidental. The scenarios and conversations are entirely fictional and should not be taken as technical advice or factual representation.

```

*A random day...*

JD: Hey, I'm stuck! 
Your service does not work.

Support engineer: What does the error message say?

JD: It tells me to 'Please send a request with no headers'.

Support engineer: And what do you send?

JD: A request with no headers, just like it asked.

Support engineer: Can you show me an example of how you send the request?

JD: Sure, here's what I did: 
httpClient.DefaultRequestHeaders.Add("no", "no");
var response = await httpClient.GetAsync("https://www.xxxxx.xx/api/yyyyy");

Support engineer: Ah, I see the problem. 
By adding a header with 'no', you're actually sending a header. 
You need to send the request without adding any headers at all. 
Just leave them empty.

*The next day...*

JD: Hey, I removed the ‘no’ header, and it worked! 
But now I’m lost with the response headers.

Support engineer: What headers do get back in the response?

JD: The documentation says I should get a ‘your-key’ header with a key. 
I see the key, but the value is… ‘System.String[]’? 
That can’t be right, can it?

Support engineer: How do you read the headers?

JD: Like this:

foreach (var item in response.Headers) 
{
    // I expected something like [your-key, magic_value], 
    // but I got this instead: [your-key, System.String[]]
    Debug.WriteLine(item);
}

Support engineer: 🤦

Support engineer: That’s the type, not the value! 
You need to access the actual value, not just the type of the object.

JD: So you’re telling me ‘System.String[]’ isn’t some newfangled encryption I need to decode?

Support engineer: No, JD, it’s just the type indicating it’s an array of strings. 
Here, let me show you how to get the actual value:

foreach (var item in response.Headers) 
{
    Debug.WriteLine($"[Key: {item.Key}, Value: {item.Value}]");
}

JD: Oh, so the real value was hiding in plain sight! 
And here I was ready to dive into the world of string arrays as if it was a treasure map…

Support engineer: Sometimes, the treasure is not in the ‘X’ that marks the spot, but in understanding the map itself.

```

## Understanding HttpClient Logging

In the .NET ecosystem, `HttpClient` is the go-to class for sending **HTTP requests** and receiving **HTTP responses** from a resource identified by a **URI**. A common pattern is to register `HttpClient` instances using the `.AddHttpClient()` extension method, which simplifies the process of dependency injection and provides built-in logging functionality.

```c#
// In your Startup.cs or Program.cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

// In your service or controller where you want to use HttpClient
public class MyService
{
    private readonly HttpClient httpClient;

    public MyService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<string> GetExternalDataAsync(string requestUri)
    {
        var response = await this.httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
```

`HttpClient` logs:

* **Request Information**: The HTTP method, URL, and other request-related data.
* **Request Headers**: All the HTTP headers included in the request.
* **Response Headers**: The headers returned by the server in the response.
* **Response Information**: Status code, reason phrase, and other response details.

If you need to track any of this information, there’s no need to introduce new logging statements; the built-in features have you covered.

## Setting Up Project Logging

To see the logs produced by `HttpClient`, you need to set up your project correctly. 
This involves configuring the logging level and knowing where to look for the logs.

Configuring Logging Levels: To capture detailed logs from `HttpClient`, you need to adjust the logging level in your `appsettings.json` or `appsettings.Development.json` file. Here’s what you should include:

```json
"Logging": {
    "LogLevel": {
        "Default": "Information",
        // Trace level shows you headers
        "System.Net.Http.HttpClient": "Trace"
    }
}
```

This configuration ensures that you get the most detailed logs possible, including all the HTTP request and response information.

Once you’ve set up the logging levels, you can view the logs in the following places:

* **Console**: If you’re running your application from the command line or a terminal, the logs will be displayed there.
* **Visual Studio Debug Console**: When running your application in Visual Studio, you can view the logs in the Debug Console. This is particularly useful as it allows you to see the logs in real-time while debugging.

By following these steps, you’ll be able to see the HttpClient logs and gain insights into the HTTP traffic of your application.

```
info: System.Net.Http.HttpClient.MyClient.LogicalHandler[100]
      Start processing HTTP request GET https://example.com/api/data

info: System.Net.Http.HttpClient.MyClient.ClientHandler[101]
      Sending HTTP request GET https://example.com/api/data
      Request Headers:
      Accept: application/json
      User-Agent: MyHttpClient

info: System.Net.Http.HttpClient.MyClient.ClientHandler[102]
      Received HTTP response headers after 123.4567ms - OK
      Response Headers:
      Content-Type: application/json
      Server: ExampleServer

info: System.Net.Http.HttpClient.MyClient.LogicalHandler[103]
      End processing HTTP request after 123.4567ms - OK
```

## The Trouble with Too Many Logs

So, you’ve got your `HttpClient` logging all set up and it’s working great. 
But here’s the thing: when you look at the console, it’s like a flood of logs from everywhere in your app. Trying to find the log you need is like looking for your favorite sock in a messy room—frustrating, right?

And there’s another hitch: When your code talks to the internet with lots of requests at the same time, it’s super tricky to figure out which response goes with which request. It’s like listening to a bunch of people talking all at once and trying to follow each conversation.

## Introducing DIOL

Meet `DIOL`, the new Visual Studio extension that’s here to bring the way you work with logs in your .NET applications in the next level. 

It’s **free**, **open-source**, and designed to make your life as a .NET engineer a whole lot easier.

### DIOL at a Glance

* **No Hassle Setup**: Just install the extension, and you’re good to go. DIOL seamlessly integrates with your existing logging setup.
* **Modern User Experience**: DIOL provides a sleek interface that lets you navigate and understand your logs like never before.

### Using DIOL

Once you have `DIOL` installed, it automatically picks up the logs generated by your application. 

Here’s how it enhances your logging experience:

* **Clarity in Chaos**: DIOL filters and organizes your logs, so you can focus on what matters without the noise of unrelated log entries.
* **Match Made in Heaven**: Say goodbye to the headache of matching requests with responses during parallel calls. DIOL smartly correlates them for you.

### DIOL’s Impact on .NET Engineers

`DIOL` is more than just a tool; it’s a game-changer. It transforms the tedious task of log analysis into a smooth and enjoyable part of your debugging process. With `DIOL`, you spend less time wrestling with logs and more time crafting brilliant code.

![Diol-aspnet-example](https://github.com/Dotnet-IO-logger/core/raw/main/content/screenshots/main.png)

Ready to elevate your logging game? Give `DIOL` a try and see the difference for yourself!

## Call to Action

Fantastic! Here’s the updated call to action with an additional step inviting readers to engage with the prepared guide:

* **Try DIOL**: Download the `DIOL` extension from the Visual Studio Marketplace. It’s free, easy to install, and integrates with your .NET projects without a fuss.
* **Explore and Learn**: Visit [the DIOL GitHub repository](https://github.com/Dotnet-IO-logger/core) to discover its full potential and how it can streamline your logging process.
* **Follow the Guide**: Check out our [step-by-step HTTP Logging Guide](https://github.com/Dotnet-IO-logger/Playground/tree/main/source/Diol.Demo/src/Example1HttpLoggingSample). It’s designed to help you see DIOL in action and understand how it can make your logging tasks a breeze.
* **Spread the Word**: If `DIOL` makes a difference in your work, let others know about it. Your recommendation can help fellow developers.
* **Show Your Support**: 👍 the extension on the **Visual Studio Marketplace** and give us a ⭐ on **GitHub**. Your support is invaluable for the growth of `DIOL`.

By engaging with `DIOL` and completing the guide, you’re not only enhancing your own skills but also joining a community of developers committed to improving .NET logging. 

Let’s get started and make the most of `DIOL` together!

## Links

* [Diol GitHub repository link](https://github.com/Dotnet-IO-logger/core)
* [Diol in Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=Diol.diol)
* [Diol HttpClient guide](https://github.com/Dotnet-IO-logger/Playground/tree/main/source/Diol.Demo/src/Example1HttpLoggingSample)
* [Dotnet http client](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0)
* [Dotnet logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0)
