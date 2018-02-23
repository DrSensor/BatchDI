# Batch Dependency Injection

| Status                                                                                                                                                                            | OS          |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------- |
| [![Build status](https://ci.appveyor.com/api/projects/status/g5qlityh97xukmv2/branch/master?svg=true)](https://ci.appveyor.com/project/DrSensor/batchdi-aspnetcore/branch/master) | Windows     |
| [![Build Status](https://travis-ci.org/DrSensor/BatchDI.AspNetCore.svg?branch=master)](https://travis-ci.org/DrSensor/BatchDI.AspNetCore)                                         | Linux, OS X |

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
</details>

## Installation

* .NET CLI

```bash
dotnet add package AspNet.DependencyInjection.Batch
```

* Package Manager

```powershell
PM> Install-Package AspNet.DependencyInjection.Batch
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

## API Reference

This library extend `IServiceCollection` usage by adding additional method for batch/multiple Dependency Injection in one method call.

| Method                                           | Description                                                                 |
| ------------------------------------------------ | --------------------------------------------------------------------------- |
| `BatchInject(injector =>{}, filter, blacklist?)` | implement custom dependency injection based on filter pattern and blacklist |
| `BatchSingleton(filter, blacklist?)`             | Batch/MultipleAdd version of `AddSingleton`                                 |
| `BatchTransient(filter, blacklist?)`             | Batch/MultipleAdd version of `AddTransient`                                 |
| `BatchScoped(filter, blacklist?)`                | Batch/MultipleAdd version of `AddScoped`                                    |

<details>
<summary><b>Arguments/Parameters</b></summary>

| Parameter              | Description                                                              | Type                                 |
| ---------------------- | ------------------------------------------------------------------------ | ------------------------------------ |
| `injector` (lambda)    | implement callback for custom DI                                         | `Action<Type>`, `Action<Type, Type>` |
| `filter`               | list or glob pattern for specify which class name to inject              | `string`, `string[]`                 |
| `blacklist` (optional) | list or glob pattern for specify which class name **not** to be injected | `string`, `string[]`                 |

</details>

---

## Usage

In `*.csproj`

```xml
  <ItemGroup>
    <PackageReference Include="AspNet.DependencyInjection.Batch" Version="1.0.0" />
  </ItemGroup>
```

Then in In `Startup.cs` import

```csharp
using AspNet.DependencyInjection.Batch;
```

In general, this library has 2 way of usage:

### Base Usage

This method use when you want to do custom Dependency Injection.

In `Startup.cs`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    service.BatchInject(
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

    service.BatchInject(
        filter: "I*Service",
        injector: (_interface, _class) => service.AddSingleton(_interface, _class),
    );
}
```

### Using Helper Method

This method have same functionality as [ASP.NET Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) but without doing repetitive typing.

```csharp
    service.BatchSingleton("*Service", new[] {"BlacklistOneService", "BlacklistTwoService"});
    service.BatchTransient("*Type", "BlacklistOneType");
    service.BatchScoped("*Query");
```

---

## License
MIT
