# OData.QueryBuilder
The library for creating complex OData queries based on data models with linq syntax.

> Targets OData version 4.01

test

[![Build Status](https://travis-ci.com/ZEXSM/OData.QueryBuilder.svg?branch=master)](https://travis-ci.com/ZEXSM/OData.QueryBuilder)
[![Coverage Status](https://coveralls.io/repos/github/ZEXSM/OData.QueryBuilder/badge.svg?branch=master)](https://coveralls.io/github/ZEXSM/OData.QueryBuilder?branch=master)
[![Nuget Status](https://img.shields.io/nuget/dt/OData.QueryBuilder.svg)](https://www.nuget.org/packages/OData.QueryBuilder)

## Benefits
* Expression is not used to `Compile()`, which generates `MSIL` code, which leads to memory leaks
* Support:
  * nested extenders with a choice of filtering
  * operators
    * [`in`](#in)
    * [`any`](#any)
    * [`all`](#all)
  * functions
    * date
        * [`date`](#date)
    * string and collection
        * [`contains`](#contains)
        * [`substringof (deprecated)`](#substringof)
        * [`toupper`](#toupper)
        * [`tolower`](#tolower)
        * [`concat`](#concat)
        * [`indexof`](#indexof)
  * sorting by several fields with indication of direction

## Installation
To install `OData.QueryBuilder` from `Visual Studio`, find `OData.QueryBuilder` in the `NuGet` package manager user interface or enter the following command in the package manager console:
```
Install-Package OData.QueryBuilder
```

To add a link to the main dotnet project, run the following command line:
```
dotnet add package OData.QueryBuilder
```

## Usage

1. Build instance

    As soon as possible, create a new instance of the OData.QueryBuilder object indicating the data models and the optional base path:

    ```csharp
    var odataQueryBuilder = new ODataQueryBuilder<Your OData root model>(<Your base url>?);
    
    // specify the resource for which the request will be built
    odataQueryBuilder.For<Your OData entity model>(s => s.ODataEntity)
    ```
    :information_source: __OData.Query Builder assumes you are using [OData Connected Service](https://marketplace.visualstudio.com/items?itemName=marketplace.ODataConnectedService) and you have a root model otherwise use:__
    
    ```csharp
    var odataQueryBuilder = new ODataQueryBuilder(<Your base url>?);
    
    // specify the resource for which the request will be built
    odataQueryBuilder.For<Your OData entity model>("ODataEntity")
    ```

2. Select request type

    The builder allows you to build queries on the key and the list:
    * [ByKey](#ByKey)
      * [expand](#expand)
        * [filter](#filter)
        * [select](#select)
        * [orderby](#orderby)
        * [orderby desc](#orderbydesc)
        * [skip](#skip)
        * [top](#top)
        * [count](#count)
      * [select](#select) 
    * [ByList](#ByList)
      * [expand](#expand)
        * [filter](#filter)
        * [select](#select)
        * [orderby](#orderby)
        * [orderby desc](#orderbydesc)
        * [top](#top)
      * [filter](#filter)
      * [select](#select)
      * [orderby](#orderby)
      * [orderby desc](#orderbydesc)
      * [skip](#skip)
      * [top](#top)
      * [count](#count)
3. Get Uri request or collection of operators from the builder
    ```csharp
    odataQueryBuilder.ToUri()
    odataQueryBuilder.ToDictionary()
    ```
## Fluent api

#### <a name="ByKey"/> ByKey
```csharp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByKey(223123123)
    .ToUri()
```
> http://mock/odata/ODataType(223123123)
```csharp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByKey("223123123")
    .ToUri()
```
> http://mock/odata/ODataType("223123123")
#### <a name="ByList"/> ByList
```csharp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByList()
    .ToUri()
```
> http://mock/odata/ODataType

## Usage options

#### <a name="select"/> select
```csharp
.Select(s => s.Id)
```
> $select=Id
```csharp
.Select(s => new { s.Id, s.Sum, s.Type })
```
> $select=Id,Sum,Type
#### <a name="expand"/> expand
```csharp
.Expand(s => s.BaseType)
```
> $expand=BaseType
```csharp
.Expand(s => new { s.BaseType, s.ODataKind })
```
> $expand=BaseType,ODataKind
```csharp
.Expand(f => f
    .For<ODataKindEntity>(s => s.ODataKind)
    .Expand(s=> s
        .For<ODataCodeEntity>(s => s.ODataCode)
        .Filter(s => s.IdKind == 1)
        .OrderBy(s => s.IdKind)
        .Top(1)
        .Select(s => s.IdCode));
})
```
> $expand=ODataKind($expand=ODataCode($filter=IdKind eq 1;$orderby=IdKind;$top=1;$select=IdCode))
#### <a name="filter"/> filter
```csharp
.Filter(s => s.ODataKind.ODataCode.Code >= "test_code" || s.IdType >= 5)
```
> $filter=ODataKind/ODataCode/IdCode eq 'test_code' or IdType ge 5
```csharp
.Filter(s => s.IdRule != default(int?) && s.IdRule == null)
```
> $filter=IdRule ne null and IdRule eq null 
```csharp
.Filter(s => s.ODataKind.OpenDate == DateTime.Today || s.ODataKind.OpenDate == new DateTime(2019, 7, 9)) || s.ODataKind.OpenDate == DateTime.Now)
```
> $filter=ODataKind/OpenDate eq 2019-02-09T00:00:00Z or ODataKind/OpenDate eq 2019-02-09T00:00:00Z or ODataKind/OpenDate eq 2019-02-09T15:10:20Z
```csharp
.Filter(s => s.IsActive && s.IsOpen == true && !s.ODataKind.ODataCode.IdActive)
```
> $filter=IsActive and IsOpen eq true and not ODataKind/ODataCode/IdActive
```csharp
.Filter(s => s.ODataKind.Color == ColorEnum.Blue)
```
> $filter=ODataKind/Color eq 2

:information_source: *Use parenthesis in filter*
```csharp
var constStrIds = new[] { "123", "512" };
var constValue = 3;
...
.Filter((s, f, o) => s.IdRule == constValue
    && s.IsActive
    && (f.Date(s.EndDate.Value) == default(DateTimeOffset?) || s.EndDate > DateTime.Today)
    && (f.Date((DateTimeOffset)s.BeginDate) != default(DateTime?) || f.Date((DateTime)s.BeginDate) <= DateTime.Now)
    && o.In(s.ODataKind.ODataCode.Code, constStrIds), useParenthesis: true)
```

> $filter=(((IdRule eq 3 and IsActive) and (date(EndDate) eq null or EndDate gt 2020-08-29T00:00:00Z)) and (date(BeginDate) ne null or date(BeginDate) le 2020-08-29T18:09:15Z)) and ODataKind/ODataCode/Code in ('123','512')
#### <a name="orderby"/> orderby
```csharp
.OrderBy(s => s.IdType)
```
> $orderby=IdType asc
```csharp
.OrderBy(s => new { s.IdType, s.Sum })
```
> $orderby=IdType,Sum asc
```csharp
.OrderBy((entity, sort) => sort
    .Ascending(entity.BeginDate)
    .Descending(entity.EndDate)
    .Ascending(entity.IdRule)
    .Ascending(entity.Sum)
    .Descending(entity.ODataKind.OpenDate))
```
> $orderby=BeginDate asc,EndDate desc,IdRule asc,Sum asc,ODataKind/OpenDate desc
#### <a name="orderbydesc"/> orderby desc
```csharp
.OrderByDescending(s => s.IdType)
```
> $orderby=IdType desc
```csharp
.OrderByDescending(s => new { s.IdType, s.Sum })
```
> $orderby=IdType,Sum desc
#### <a name="count"/> count
```csharp
.Count()
```
> $count=true
```csharp
.Count(false)
```
> $count=false
#### <a name="skip"/> skip
```csharp
.Skip(12)
```
> $skip=12
#### <a name="top"/> top
```csharp
.Top(100)
```
> $top=100

## Usage operators

#### <a name="in"/> in

```csharp
.Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, new[] { "123", "512" }) && o.In(s.IdType, new[] { 123, 512 }))
```
> $filter=ODataKind/ODataCode/Code in ('123','512') and IdType in (123,512)

#### <a name="any"/> any

```csharp
.Filter((s, f, o) => o.Any(s.ODataKind.ODataCodes, v => v.IdCode == 1)
```
> $filter=ODataKind/ODataCodes/any(v:v/IdCode eq 1)

#### <a name="all"/> all

```csharp
.Filter((s, f, o) => o.All(s.ODataKind.ODataCodes, v => v.IdActive))
```
> $filter=ODataKind/ODataCodes/all(v:v/IdActive)

## Usage date functions

#### <a name="date"/> date

```csharp
.Filter((s, f) => f.Date(s.Open) == DateTime.Today)
```
> $filter=date(Open) eq 2019-02-09T00:00:00Z

## Usage string and collections functions

#### <a name="contains"/> contains

```csharp
.Filter((s, f) => f.Contains(s.ODataKind.ODataCode.Code, "W"))
```
> $filter=contains(ODataKind/ODataCode/Code,'W')

#### <a name="substringof"/> substringof

```csharp
.Filter((s, f) => f.SubstringOf("W", s.ODataKind.ODataCode.Code))
```
> $filter=substringof('W',ODataKind/ODataCode/Code)

#### <a name="toupper"/> toupper

```csharp
.Filter((s, f) => f.ToUpper(s.ODataKind.ODataCode.Code) == "TEST_CODE")
```
> $filter=toupper(ODataKind/ODataCode/Code) eq 'TEST_CODE'

#### <a name="tolower"/> tolower

```csharp
.Filter((s, f) => f.ToLower(s.ODataKind.ODataCode.Code) == "test_code")
```
> $filter=tolower(ODataKind/ODataCode/Code) eq 'test_code'

#### <a name="concat"/> concat

```csharp
.Filter((s, f) => f.Concat(s.ODataKind.ODataCode.Code, "_1") == "test_code_1")
```
> $filter=concat(ODataKind/ODataCode/Code, '_1') eq 'test_code_1'

#### <a name="indexof"/> indexof

```csharp
.Filter((s, f) => f.IndexOf(s.ODataKind.ODataCode.Code, "testCode") == 1)
```
> $filter=indexof(ODataKind/ODataCode/Code,'testCode') eq 1

## Usage other functions

#### ConvertEnumToString
```csharp
.Filter((s, f) => s.ODataKind.Color == f.ConvertEnumToString(ColorEnum.Blue))
```
> $filter=ODataKind/Color eq 'Blue'

#### ConvertDateTimeToString
```csharp
.Filter((s, f) => s.ODataKind.OpenDate == f.ConvertDateTimeToString(new DateTime(2019, 2, 9), "yyyy-MM-dd"))
```
> $filter=ODataKind/OpenDate eq 2019-02-09

#### ConvertDateTimeOffsetToString
```csharp
.Filter((s, f) => s.ODataKind.OpenDate == f.ConvertDateTimeToString(new DateTimeOffset(new DateTime(2019, 2, 9)), "yyyy-MM-dd"))
```
> $filter=ODataKind/OpenDate eq 2019-02-09

#### ReplaceCharacters
```csharp
var dictionary = new Dictionary<string, string> { { "&", "%26" } });
var constValue = "3 & 4";
...
.Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(
    constValue,
    dictionary)
```
> $filter=ODataKind/ODataCode/Code eq '3 %26 4'

```csharp
var strings = new string[] {
    "test\\YUYYUT",
    "test1\\YUYY123"
};
...
.Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, f.ReplaceCharacters(
    strings,
    new Dictionary<string, string>() { { @"\", "%5C" } })))
```
> $filter=ODataKind/ODataCode/Code in ('test%5C%5CYUYYUT','test1%5C%5CYUYY123')

## Suppress exceptions

:warning: __May result in loss of control over the expected result.__

> Suppression of inclusion excludes the whole expression from the request

By default, suppression of function and operator argument exceptions is disabled. To enable it, you must pass the configuration `ODataQueryBuilderOptions` when initializing the builder
with initialization of the `SuppressExceptionOfNullOrEmptyFunctionArgs` and` SuppressExceptionOfNullOrEmptyOperatorArgs` properties.

```csharp
var builder = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata", new ODataQueryBuilderOptions {
    SuppressExceptionOfNullOrEmptyFunctionArgs = true,
    SuppressExceptionOfNullOrEmptyOperatorArgs = true,
});

var uri = builder
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByList()
    .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, new string[0]) || o.In(s.ODataKind.ODataCode.Code, null)
        && f.Contains(s.ODataKind.ODataCode.Code, default(string)) 
        && o.In(s.IdType, new[] { 123, 512 }))
    .ToUri()
```
> http://mock/odata/ODataType?$filter=IdType in (123,512)