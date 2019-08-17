# OData.QueryBuilder
Library provides linq syntax and allows you to build OData queries based on the data model

[![Build Status](https://travis-ci.com/ZEXSM/OData.QueryBuilder.svg?branch=master)](https://travis-ci.com/ZEXSM/OData.QueryBuilder)
[![Coverage Status](https://coveralls.io/repos/github/ZEXSM/OData.QueryBuilder/badge.svg?branch=master)](https://coveralls.io/github/ZEXSM/OData.QueryBuilder?branch=master)
[![Nuget Status](https://img.shields.io/nuget/dt/OData.QueryBuilder.svg)](https://www.nuget.org/packages/OData.QueryBuilder)

## Benefits
* Expression is not used to `Compile()`, which generates `MSIL` code, which leads to memory leaks
* Support for nested `OData` extenders with a choice of filtering
* Support `OData` functions `Date`, `Any`, `All`
* Support `OData` operator `IN`

## Installation
To install `OData.QueryBuilder` from` Visual Studio`, find `OData.QueryBuilder` in the` NuGet` package manager user interface or enter the following command in the package manager console:
```
Install-Package OData.QueryBuilder -Version 1.0.0
```

To add a link to the main dotnet project, run the following command line:
```
dotnet add -v 1.0.0 OData.QueryBuilder
```

## Usage

1. Build instance

    As soon as possible, create a new instance of the OData.QueryBuilder object indicating the data models and the base path:

    ```csharp
    var odataQueryBuilder = new ODataQueryBuilder<Your entity model>(<base_url>);
    ```

2. Specify the resource for which the request will be built

    ```csharp
    odataQueryBuilder.For<Your data model>(s => s.<Your resource model>)
    ```

3. Select request type

    The builder allows you to build queries on the key and the list:
    * ByKey(<Key>)
      * Expand
      * Select
      * ToUri 
    * ByList()
      * Expand
      * Filter
      * Select
      * OrderBy
      * OrderByDescending
      * Top
      * Skip
      * Count
      * ToUri 
4. Get a Uri request from the builder
    ```csharp
    odataQueryBuilder.ToUri()
    ```

## Examples

#### By key
1. Key request with a simple `Expand`
```csharp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByKey(223123123)
    .Expand(s => s.ODataKind)
    .ToUri();
```
> http://mock/odata/ODataType(223123123)?$expand=ODataKind

2. Key request with nested `Expand` и `Select`
```csharp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByKey(223123123)
    .Expand(f =>
    {
        f.For<ODataKindEntity>(s => s.ODataKind)
            .Expand(ff => ff.For<ODataCodeEntity>(s => s.ODataCode)
            .Select(s => s.IdCode));
        f.For<ODataKindEntity>(s => s.ODataKindNew)
            .Select(s => s.IdKind);
        f.For<ODataKindEntity>(s => s.ODataKindNew)
            .Select(s => s.IdKind);
    })
    .Select(s => new { s.IdType, s.Sum })
    .ToUri();
```
> http://mock/odata/ODataType(223123123)?$expand=ODataKind($expand=ODataCode($select=IdCode)),ODataKindNew($select=IdKind),ODataKindNew($select=IdKind)&$select=IdType,Sum

#### By list
1. Query list with a simple `Expand`
```csharp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByList()
    .Expand(s => new { s.ODataKind })
    .ToUri();
```
> http://mock/odata/ODataType?$expand=ODataKind
2.
```csharp
var uri = _odataQueryBuilder
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByList()
    .Expand(f =>
    {
        f.For<ODataKindEntity>(s => s.ODataKind)
            .Expand(ff => ff.For<ODataCodeEntity>(s => s.ODataCode).Select(s => s.IdCode))
            .Select(s => s.IdKind);
        f.For<ODataKindEntity>(s => s.ODataKindNew)
            .Select(s => s.IdKind);
        f.For<ODataKindEntity>(s => s.ODataKindNew)
            .Select(s => s.IdKind);
    })
    .ToUri();
```
> http://mock/odata/ODataType?$expand=ODataKind($expand=ODataCode($select=IdCode);$select=IdKind),ODataKindNew($select=IdKind),ODataKindNew($select=IdKind)