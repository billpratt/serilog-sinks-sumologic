# Serilog.Sinks.SumoLogic

A [Serilog](https://github.com/serilog/serilog) sink that writes events to [Sumo Logic](http://www.sumologic.com).

**Package** - [Serilog.Sinks.SumoLogic](http://nuget.org/packages/serilog.sinks.sumologic)
| **Platforms** - .NET 4.5, .NET Core, .NETStandard 1.5


### Usage

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
