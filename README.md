# OData.QueryBuilder
Library provides linq syntax and allows you to build OData queries based on the data model

[![Build Status](https://travis-ci.com/ZEXSM/OData.QueryBuilder.svg?branch=master)](https://travis-ci.com/ZEXSM/OData.QueryBuilder)
[![Coverage Status](https://coveralls.io/repos/github/ZEXSM/OData.QueryBuilder/badge.svg?branch=master)](https://coveralls.io/github/ZEXSM/OData.QueryBuilder?branch=master)
[![Nuget Status](https://img.shields.io/nuget/dt/OData.QueryBuilder.svg)](https://www.nuget.org/packages/OData.QueryBuilder)

## Benefits
* Expression is not used to `Compile()`, which generates `MSIL` code, which leads to memory leaks
* Support for nested `OData` extenders with a choice of filtering
* Support:
  * `all`, `any`
  * date functions `date`
  * string functions `substringof`, `toupper`
  * operator `in`

## Installation
To install `OData.QueryBuilder` from `Visual Studio`, find `OData.QueryBuilder` in the `NuGet` package manager user interface or enter the following command in the package manager console:
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
    * [ByKey](#ByKey)
      * [Expand](#Expand)
        * [Filter](#Filter)
        * [Select](#Select)
      * [Select](#Select) 
    * [ByList](#ByList)
      * [Expand](#Expand)
        * [Filter](#Filter)
        * [Select](#Select)
      * [Filter](#Filter)
      * [Select](#Select)
      * [OrderBy](#OrderBy)
      * [OrderByDescending](#OrderByDescending)
      * [Top](#Top)
      * [Skip](#Skip)
      * [Count](#Count)
4. Get Uri request or collection of operators from the builder
    ```csharp
    odataQueryBuilder.ToUri()
    odataQueryBuilder.ToDictionary()
    ```
## <a name="ByKey"/> ByKey
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
## <a name="ByList"/> ByList
```csharp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByList()
    .ToUri()
```
> http://mock/odata/ODataType
## <a name="Select"/> Select
```csharp
.Select(s => s.Id)
```
> $select=Id
```csharp
.Select(s => new { s.Id, s.Sum, s.Type })
```
> $select=Id,Sum,Type
## <a name="Expand"/> Expand
```csharp
.Expand(s => s.BaseType)
```
> $expand=BaseType
```csharp
.Expand(s => new { s.BaseType, s.ODataKind })
```
> $expand=BaseType,ODataKind
```csharp
.Expand(f =>
{
    f.For<ODataKindEntity>(s => s.ODataKind)
        .Expand(s=> s.For<ODataCodeEntity>(s => s.ODataCode)
        .Filter(s => s.IdKind == 1)
        .Select(s => s.IdCode));
    f.For<ODataKindEntity>(s => s.ODataKindNew)
        .Select(s => s.IdKind);
})
```
> $expand=ODataKind($expand=ODataCode($filter=IdKind eq 1;$select=IdCode)),ODataKindNew($select=IdKind)
## <a name="Filter"/> Filter
```csharp
.Filter(s => s.ODataKind.ODataCode.IdCode >= 3 || s.IdType == 5)
```
> $filter=ODataKind/ODataCode/IdCode ge 3 or IdType eq 5
```csharp
private static string IdCodeStatic => "testCode";
...
var constValue = "3";
.Filter(s =>
    s.ODataKind.ODataCode.Code == constValue || s.ODataKind.ODataCode.Code == "5"
    && s.ODataKind.ODataCode.Code == IdCodeStatic)
```
> $filter=ODataKind/ODataCode/Code eq '3' or ODataKind/ODataCode/Code eq '5' and ODataKind/ODataCode/Code eq 'testCode'
```csharp
.Filter(s => s.ODataKind.ODataCodes.Any(a => a.IdCode == 1) 
    && s.ODataKind.ODataCodes.All(v => v.IdActive))
```
> $filter=ODataKind/ODataCodes/any(a:a/IdCode eq 1) and ODataKind/ODataCodes/all(v:v/IdActive)
```csharp
var constValue = 2;
var constCurrentDate = DateTime.Today.ToString("yyyy-MM-dd");

.Filter(s =>
    (s.IdType < constValue && s.ODataKind.ODataCode.IdCode >= 3)
    || s.IdType == 5
    && s.IdRule != default(int?)
    && s.IdRule == null)
```
> $filter=IdType lt 2 and ODataKind/ODataCode/IdCode ge 3 or IdType eq 5 and IdRule ne null and IdRule eq null 
```csharp
var constCurrentDateToday = new DateTime(2019, 2, 9);
var constCurrentDateNow = new DateTime(2019, 2, 9, 1, 2, 4);
var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { EndDate = constCurrentDateToday } };

.Filter(s =>
    s.ODataKind.OpenDate.Date == constCurrentDateNow
    && s.ODataKind.OpenDate == constCurrentDateToday
    && s.ODataKind.OpenDate == DateTime.Today
    && s.Open.Date == DateTime.Today
    && s.Open == DateTime.Today
    && s.Open == constCurrentDateToday
    && s.Open.Date == newObject.ODataKind.EndDate
    && s.ODataKind.OpenDate.Date == new DateTime(2019, 7, 9)
    && ((DateTime)s.BeginDate).Date == DateTime.Today)
```
> $filter=date(ODataKind/OpenDate) eq 2019-02-09 and ODataKind/OpenDate eq 2019-02-09T00:00:00.0000000 and ODataKind/OpenDate eq 2019-08-18T00:00:00.0000000+03:00 and date(Open) eq 2019-08-18 and Open eq 2019-08-18T00:00:00.0000000+03:00 and Open eq 2019-02-09T00:00:00.0000000 and date(Open) eq 2019-02-09 and date(ODataKind/OpenDate) eq 2019-07-09 and date(BeginDate) eq 2019-08-18
```csharp
var constStrIds = new[] { "123", "512" };
var constStrListIds = new[] { "123", "512" }.ToList();
var constIntIds = new[] { 123, 512 };
var constIntListIds = new[] { 123, 512 }.ToList();
var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { Sequence = constIntListIds } };
var newObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = constIntIds } };

.Filter(s => constStrIds.Contains(s.ODataKind.ODataCode.Code)
    && constStrListIds.Contains(s.ODataKind.ODataCode.Code)
    && constIntIds.Contains(s.IdType)
    && constIntListIds.Contains(s.IdType)
    && constIntIds.Contains((int)s.IdRule)
    && constIntListIds.Contains((int)s.IdRule)
    && newObject.ODataKind.Sequence.Contains(s.ODataKind.IdKind)
    && newObjectSequenceArray.ODataKind.SequenceArray.Contains(s.ODataKind.ODataCode.IdCode))
```
> $filter=ODataKind/ODataCode/Code in ('123','512') and ODataKind/ODataCode/Code in ('123','512') and IdType in (123,512) and IdType in (123,512) and IdRule in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512) and ODataKind/ODataCode/IdCode in (123,512)
```csharp
var constStrIds = default(IEnumerable<string>);
var constStrListIds = new string[] { }.ToList();
var constIntIds = default(int[]);
var constIntListIds = new[] { 123, 512 }.ToList();
var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { Sequence = constIntListIds } };
var newObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = constIntIds } };

.Filter(s => constStrIds.Contains(s.ODataKind.ODataCode.Code)
    && constStrListIds.Contains(s.ODataKind.ODataCode.Code)
    && constIntIds.Contains(s.IdType)
    && constIntListIds.Contains(s.IdType)
    && constIntIds.Contains((int)s.IdRule)
    && constIntListIds.Contains((int)s.IdRule)
    && newObject.ODataKind.Sequence.Contains(s.ODataKind.IdKind)
    && newObjectSequenceArray.ODataKind.SequenceArray.Contains(s.ODataKind.ODataCode.IdCode))
```
> $filter=IdType in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512)
```csharp
.Filter(s => s.IsActive && s.IsOpen == true && s.ODataKind.ODataCode.IdActive == false)
```
> $filter=IsActive and IsOpen eq true and ODataKind/ODataCode/IdActive eq false
```csharp
var constStrIds = new[] { "123", "512" };
var constValue = 3;

.Filter(s => s.IdRule == constValue
     && s.IsActive
     && (((DateTimeOffset)s.EndDate).Date == default(DateTimeOffset?) || s.EndDate > DateTime.Today)
     && (((DateTime)s.BeginDate).Date != default(DateTime?) || ((DateTime)s.BeginDate).Date <= DateTime.Today)
     && constStrIds.Contains(s.ODataKind.ODataCode.Code))
```
> $filter=IdRule eq 3 and IsActive and date(EndDate) eq null or EndDate gt 2019-08-18T00:00:00.0000000+03:00 and date(BeginDate) ne null or date(BeginDate) le 2019-08-18 and ODataKind/ODataCode/Code in ('123','512')
```csharp
.Filter(s => s.ODataKind.Color.ToString() == ColorEnum.Blue.ToString()
    && s.ODataKind.Color == ColorEnum.Blue)
```
> $filter=ODataKind/Color eq 'Blue' and ODataKind/Color eq 2
```csharp
var constValue = "p".ToUpper();
var newObject = new ODataTypeEntity { TypeCode = "TypeCodeValue".ToUpper() };

.Filter(s => s.ODataKind.ODataCode.Code.ToUpper().Contains("W")
    || s.ODataKind.ODataCode.Code.Contains(constValue)
    || s.ODataKindNew.ODataCode.Code.Contains(newObject.TypeCode)
    || s.ODataKindNew.ODataCode.Code.Contains("55"))
```
> $filter=substringof('W',toupper(ODataKind/ODataCode/Code)) or substringof('P',ODataKind/ODataCode/Code) or substringof('TYPECODEVALUE',ODataKindNew/ODataCode/Code) or substringof('55',ODataKindNew/ODataCode/Code)
## <a name="OrderBy"/> OrderBy
```csharp
.OrderBy(s => s.IdType)
```
> $orderby=IdType asc
```csharp
.OrderBy(s => new { s.IdType, s.Sum })
```
> $orderby=IdType,Sum asc
## <a name="OrderByDescending"/> OrderByDescending
```csharp
.OrderByDescending(s => s.IdType)
```
> $orderby=IdType desc
```csharp
.OrderByDescending(s => new { s.IdType, s.Sum })
```
> $orderby=IdType,Sum desc
## <a name="Count"/> Count
```csharp
.Count()
```
> $count=true
```csharp
.Count(false)
```
> $count=false
## <a name="Skip"/> Skip
```csharp
.Skip(12)
```
> $skip=12
## <a name="Top"/> Top
```csharp
.Top(100)
```
> $top=100