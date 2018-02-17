# Batch Dependency Injection

This package/library use for doing multiple dependency injection in easy way. For more info about DI (Dependency Injection) see [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).

## Installation

```bash
dotnet add package AspNet.DependencyInjection.Batch
```

## Usage

```xml
  <ItemGroup>
    <PackageReference Include="AspNet.DependencyInjection.Batch" Version="1.0.0-alpha1" />
  </ItemGroup>
```

```csharp
using AspNet.DependencyInjection.Batch;
```

### Base Usage
In `Startup.cs`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    service.BatchInject(
        filter: "*Service",
        blacklist: new[] { "NoSingletonService", "AnotherService" },
        injector: _class => service.AddSingleton(_class)
    );
    service.BatchInject(
        filter: new[] { "*Service", "*Controllers" },
        injector: _class => service.AddSingleton(_class),
        blacklist: new[] { "NoSingletonService", "AnotherService" },
    );
    service.BatchInject(
        injector: (_interface, _class) => service.AddSingleton(_interface, _class),
        filter: "I*Service",
        blacklist: new[] { "NoSingletonService", "AnotherService" }
    );
}
```

### Using Helper Method

```csharp
    service.BatchSingleton("*Service", new[] {"MorningService", "NightService"});
    service.BatchTransient("*Type", "YourType");
    service.BatchScoped("*Query");
```

## API Reference

This library extend `IServiceCollection` usage by adding additional method for batch/multiple Dependency Injection in one method call.

| Method | Description |
|------- |------------ |
| `BatchInject(injector =>{}, filter, blacklist?)` | implement custom dependency injection based on filter pattern and blacklist |
| `BatchSingleton(filter, blacklist?)` | Batch/MultipleAdd version of `AddSingleton` |
| `BatchTransient(filter, blacklist?)` | Batch/MultipleAdd version of `AddTransient` |
| `BatchScoped(filter, blacklist?)` | Batch/MultipleAdd version of `AddScoped` |

<details>

| Parameter | Description | Type |
|---------- |-------- |---------- |
| `injector` (lambda) | implement callback for custom DI | `Action<Type>`, `Action<Type, Type>` |
| `filter` | list or glob pattern for specify which class name to inject | `string`, `string[]` |
| `blacklist` (optional) | list or glob pattern for specify which class name **not** to be injected | `string`, `string[]` |
</details>


## TODO/Need-Help

* [ ] Implement array of glob pattern in blacklist. Example

```csharp
    service.BatchSingleton(
        filter: new[] { "I*Service", "*Type", },
        blacklist: new[] { "*NotNeeded*", "JustFine*" }
    );
```

## License
MIT