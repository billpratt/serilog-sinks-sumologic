# Serilog.Sinks.SumoLogic

[![NuGet][main-nuget-badge]][main-nuget] [![NuGet][nuget-dl-badge]][main-nuget]

[main-nuget]: https://www.nuget.org/packages/Serilog.Sinks.SumoLogic/
[main-nuget-badge]: https://img.shields.io/nuget/v/Serilog.Sinks.SumoLogic.svg?style=flat-square&label=nuget
[nuget-dl-badge]: https://img.shields.io/nuget/dt/Serilog.Sinks.SumoLogic.svg?style=flat-square

A [Serilog](https://github.com/serilog/serilog) sink that writes events to [Sumo Logic](http://www.sumologic.com).

**Package** - [Serilog.Sinks.SumoLogic](http://nuget.org/packages/serilog.sinks.sumologic)
| **Platforms** - .NET 4.5, .NET Core, .NETStandard 2.1


### Usage

#### Basic

```csharp
// basic usage writes to Sumo Logic with the default source name 'Serilog'
var log = new LoggerConfiguration()
    .WriteTo.SumoLogic("[YOUR SUMO COLLECTOR URL]")
    .CreateLogger();

// override default Sumo Logic source name
var log = new LoggerConfiguration()
    .WriteTo.SumoLogic("[YOUR SUMO COLLECTOR URL]", "FancyPantsSourceName")
    .CreateLogger();
```

#### ASP.NET Core

```powershell
Install-Package Serilog.Extensions.Logging -DependencyVersion Highest
````

```csharp
using Serilog;
using Serilog.Sinks.SumoLogic;

public class Startup
{
  public Startup(IHostingEnvironment env)
  {
    Log.Logger = new LoggerConfiguration()
      .WriteTo.SumoLogic("http://localhost")  //replace with your SumoLogic endpoint
      .CreateLogger();
      
    // Other startup code
```

```csharp
public void Configure(IApplicationBuilder app,
                        IHostingEnvironment env,
                        ILoggerFactory loggerfactory,
                        IApplicationLifetime appLifetime)
  {
      loggerfactory.AddSerilog();
      
      // Ensure any buffered events are sent at shutdown
      appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
```
