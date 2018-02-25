# Contributing to BatchDI

BatchDI is a community-driven project. As such, we welcome and encourage all sorts of
contributions. They include, but are not limited to:

- Constructive feedback
- Questions about usage
- Documentation changes
- Feature requests
- [Pull requests](#pull-requests)
- [Create issues](#create-issues)

We strongly suggest that to **search through the existing issues** before filing an issue to see if it has already been filed by someone else or there is an active discussion on it.
For pull-request and commit message, it's **mandatory** to follow this [guide](#pull-requests) but can became a good practice for contributing to this project.

## Contribution suggestions

We use the label [`help wanted`](https://github.com/DrSensor/BatchDI/issues?q=is%3Aopen+is%3Aissue+label%3A%22help+wanted%22) in the issue tracker to denote fairly-well-scoped-out bugs or feature requests that the community can pick up and work on. If any of those labeled issues do not have enough information, please feel free to ask constructive questions. (This applies to any open issue.)

-----

## Project Structure

In general, the folder structure of this project follow:

```bash
.
├── Example                         # Contain specific implementation depend on framework to use
│   └── <Framework of Choice>
│       └── Example.<Framework of Choice>.csproj
│
├── Framework                       # Contain extension method of specific framework
│   └── <Framework of Choice>
│       ├── BatchDI.<Framework of Choice>.csproj
│       └── <Class/Interface to Extend>Extensions.cs
│
└── Library                         # Contain main implementation of BatchDI
    ├── BatchDI.csproj
    ├── BatchDI_API.cs
    └── BatchDI.cs
```

Test on new Framework extension heavily depend on smoke testing in Example folder.

- To make **Example** test, append this in the `Example.<Framework of Choice>.csproj` file:

```xml
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.*" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="../../Framework/AspNetCore/BatchDI.<Framework of Choice>.csproj" />
  </ItemGroup>
```

- To create [extension method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) based on specific **Framework**, add and modify this in the `BatchDI.<Framework of Choice>.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <PropertyGroup>
    <Version />
    <PackageId>BatchDI.[Framework of Choice]</PackageId>
    <Authors>Fahmi Akbar Wildana, [Your Name]</Authors>
  </PropertyGroup>
  <PropertyGroup>
    <AssemblyName>BatchDI.[Framework of Choice]</AssemblyName>
    <RootNamespace>BatchDI.[Framework of Choice]</RootNamespace>
    <Company />
    <Product>Batch Dependency Injection for [Framework of Choice]</Product>
    <Description>This library is used to simplify multiple Dependency Injection in [Framework of Choice].</Description>
    <PackageProjectUrl>https://github.com/DrSensor/BatchDI</PackageProjectUrl>
    <RepositoryUrl>https://github.com/DrSensor/BatchDI.git</RepositoryUrl>
    <PackageTags>[Framework of Choice];Dependency Injection</PackageTags>
    <Copyright>Fahmi Akbar Wildana, [Your Name]</Copyright>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="../../Library/BatchDI.csproj" />
    <Compile Include="<Class/Interface to Extend>Extensions.cs" />

    <PackageReference Include="LIBRARY_TO_EXTEND" Version="[MINIMUM_VERSION,]" />
  </ItemGroup>
</Project>
```

## Pull Requests

Here are some things to keep in mind as you file pull requests to fix bugs, add new features, etc.:

- AppVeyor and Travis-CI is used to make sure that the project builds packages as expected on the supported
  platforms, using supported .NET Core CLI versions.
- Make sure your commits are rebased onto the latest commit in the master branch
- This is **optional** but it's good if you can limit/squash the number of commits created to a "feature"-level. For instance:

bad:

```git
commit 1: add foo option
commit 2: standardize code
commit 3: add test
commit 4: add docs
commit 5: add bar option
commit 6: add test + docs
```

good:

```git
commit 1: add foo option
commit 2: add bar option
```

### Git Commit Messages

- Use the present tense (`add feature` not `added feature`)
- Use the imperative mood (`move cursor to...` not `moves cursor to...`)
- Try to limit the length of commit message
- For long commit message, make it per point and use `-` in commit description
- Reference issues and pull requests liberally after the first line, if applicable
- When need emoticon, use it at the end of sentence (`add explosion :boom:`)

-----