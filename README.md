# Batch Dependency Injection

![version](https://img.shields.io/nuget/v/BatchDI.svg)
![download](https://img.shields.io/nuget/dt/BatchDI.svg)

| Status                                                                                                                                                                            | OS          |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------- |
| [![Build status](https://ci.appveyor.com/api/projects/status/g5qlityh97xukmv2/branch/master?svg=true)](https://ci.appveyor.com/project/DrSensor/batchdi-aspnetcore/branch/master) | Windows     |
| [![Build Status](https://travis-ci.org/DrSensor/BatchDI.svg?branch=master)](https://travis-ci.org/DrSensor/BatchDI.AspNetCore)                                                    | Linux, OS X |

<img align="right" src="./icon.svg" width="150" height="150" />

This package/library use for doing multiple dependency injection in easy way. For more info about DI (Dependency Injection) see [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).

<details>
<summary><sup>The real reason is I want to avoid this when using GraphQL</sup></summary>

> copas from [this repo](https://github.com/glennblock/orders-graphql/blob/master/Server/Startup.cs#L23)

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IOrderService, OrderService>();
        services.AddSingleton<OrdersSchema>();
        services.AddSingleton<OrdersQuery>();
        services.AddSingleton<OrderType>();
        services.AddSingleton<OrderCreateInputType>();
        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddSingleton<CustomerType>();
        services.AddSingleton<OrderStatusesEnum>();
        services.AddSingleton<OrdersMutation>();
        services.AddSingleton<OrderSubscription>();
        services.AddSingleton<OrderEventType>();
        services.AddSingleton<IOrderEventService, OrderEventService>();
        services.AddSingleton<IEventAggregator, SimpleEventAggregator>();
        services.AddSingleton<IDependencyResolver>(c =>
            new FuncDependencyResolver(type => c.GetRequiredService(type))); services.AddGraphQLHttp();
        services.AddGraphQLWebSocket<OrdersSchema>();
        services.AddMvc();
    }
```

So then I can write it like this

```c#
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IOrderService, OrderService>();
        services.BatchSingleton(new[] {
            "I*Service",
            "Order*",
            "Customer*"
        });

        services.AddSingleton<IEventAggregator, SimpleEventAggregator>();
        services.AddSingleton<IDependencyResolver>(c =>
            new FuncDependencyResolver(type => c.GetRequiredService(type))); services.AddGraphQLHttp();
        services.AddGraphQLWebSocket<OrdersSchema>();
        services.AddMvc();
    }
```

</details>

## Installation

*   .NET CLI

```bash
dotnet add package BatchDI
# Just only install this if using ASP.NET Core
dotnet add package BatchDI.AspNetCore
```

*   Package Manager

```powershell
PM> Install-Package BatchDI
PM> Install-Package BatchDI.AspNetCore
```

<details>
<summary>for nightly build (if really needed)</summary>

In `nuget.config` before installing

```xml
<configuration>
  <packageSources>
    <add key="BatchDI Package" value="https://ci.appveyor.com/nuget/batch-di" />
  </packageSources>
</configuration>
```

</details>

## Usage

> See [Example folder](./Example) for more complete example usage.

In `*.csproj`

```xml
  <ItemGroup>
    <PackageReference Include="BatchDI" Version="1.1.0" />
    <!-- Just only add this if using ASP.NET Core -->
    <PackageReference Include="BatchDI.AspNetCore" Version="1.1.0" />
  </ItemGroup>
```

Then to import

```csharp
using BatchDI;
using BatchDI.AspNetCore;
```

In general, this library has 2 way of usage:

### Base Usage

<details>
<summary>Use this method if you want to do custom Dependency Injection.</summary>

```c#
using BatchDI;
.
.
BatchDI.BatchInject(
    filter: "*Service",
    injector: _implementation =>
    {
        if (_implementation.Name.Contains("My"))
        {
            service.AddSingleton(_class, new MyBaseService(Configuration["MyConfig"])));
        }
    }
);

// or

BatchDI.BatchInject(
    filter: "I*Service",
    injector: (_interface, _class) => service.AddSingleton(_interface, _class),
);
```

</details>

### Using Helper Method

#### ASP․NET Core

This method have same functionality as [ASP.NET Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) but without doing repetitive typing.

*   In `Startup.cs`

```c#
using Microsoft.Extensions.DependencyInjection;
using BatchDI.AspNetCore;
.
.
public void ConfigureServices(IServiceCollection services)
{
    service.BatchSingleton("*Service", new[] {"BlacklistOneService", "BlacklistTwoService"})
           .BatchTransient("*Type", "BlacklistOneType");

    service.BatchScoped("*Query");

    // For custom dependency injection
    service.BatchInject(lambdaFunction, "*Service", new[] {"BlacklistOneService", "BlacklistTwoService"});
}
```

<details>
<summary>Workaround for fix error in Integration Test.</summary>

Use `SetBatchDIEntryPoint` to set the EntryAssembly

```c#
_server = new TestServer(new WebHostBuilder().SetBatchDIEntryPoint<Startup>().UseStartup<Startup>());
```

</details>

---

## API Reference

<details>
<summary><b>Arguments/Parameters</b></summary>

| Parameter                       | Description                                                              | Type                                     | Default value |
| ------------------------------- | ------------------------------------------------------------------------ | ---------------------------------------- | ------------- |
| `injector` (lambda)             | implement callback for custom DI                                         | `Action<Type>`, <br>`Action<Type, Type>` |
| `filter`                        | list or glob pattern for specify which class name to inject              | `string`, <br>`string[]`                 |
| `blacklist` (optional)          | list or glob pattern for specify which class name **not** to be injected | `string`, <br>`string[]`                 |
| `parallel` (optional)           | if the startup time become slower, try to set this `true`                | `bool`                                   | `false`       |
| `nested` (optional) | choose if also to inject nested class                                    | `bool`                                   | `true`        |

</details>

### BatchDI

| Method                                                                           | Description                                                                 | Return |
| -------------------------------------------------------------------------------- | --------------------------------------------------------------------------- | ------ |
| `BatchInject(injector =>{}, filter, blacklist?, parallel?, nested?)` | implement custom dependency injection based on filter pattern and blacklist |
| `SetEntryAssembly(Assembly assembly)`                                            | Try to set this this in case there is an error                              |

### BatchDI.AspNetCore

This library extend `IServiceCollection` usage by adding additional method for batch/multiple Dependency Injection in one method call.

| Method                                                               | Description                                 | Return               |
| -------------------------------------------------------------------- | ------------------------------------------- | -------------------- |
| `BatchSingleton(filter, blacklist?, parallel?, nested?)` | Batch/MultipleAdd version of `AddSingleton` | `IServiceCollection` |
| `BatchTransient(filter, blacklist?, parallel?, nested?)` | Batch/MultipleAdd version of `AddTransient` | `IServiceCollection` |
| `BatchScoped(filter, blacklist?, parallel?, nested?)`    | Batch/MultipleAdd version of `AddScoped`    | `IServiceCollection` |

---

## Support

See [CONTRIBUTING.md](./CONTRIBUTING.md) for contributing directly via:

*   [Create Issues](./CONTRIBUTING.md/#create-issues)
*   [Pull Requests](./CONTRIBUTING.md/#pull-requests) or
*   [Info about the project structure](./CONTRIBUTING.md/#project-structure)

## License

[MIT](./LICENSE) License
