# OData.QueryBuilder
Библиотека позволяющая собрать OData запрос на основе модели данных

[![Build Status](https://travis-ci.com/ZEXSM/OData.QueryBuilder.svg?branch=master)](https://travis-ci.com/ZEXSM/OData.QueryBuilder)
[![Coverage Status](https://coveralls.io/repos/github/ZEXSM/OData.QueryBuilder/badge.svg?branch=master)](https://coveralls.io/github/ZEXSM/OData.QueryBuilder?branch=master)

## Установка
Чтобы установить `OData.QueryBuilder` из `Visual Studio`, найдите `OData.QueryBuilder` в пользовательском интерфейсе диспетчера пакетов `NuGet` или выполните следующую команду в консоли диспетчера пакетов:
```
Install-Package OData.QueryBuilder -Version 1.0.0
```

Чтобы добавить ссылку на основной проект `dotnet`, выполните в командной строке следующее:

```
dotnet add -v 1.0.0 OData.QueryBuilder
```

## Использование

1. Создать экземпляр билдера

    Преждем чем начать строить `OData` запрос, необходимо создать новый экземпляр объекта `OData.QueryBuilder` с указанием модели данных и базового пути:

    ```charp
    var odataQueryBuilder = new ODataQueryBuilder<Модель данных сущностей>(<base_url>);
    ```

2. Указать ресурс для которого будет строиться запрос

    ```charp
    odataQueryBuilder.For<Модель сущности>(s => s.<ресурс>)
    ```

3. Выборать тип запроса

    Билдер позволяет строить запросы по ключу и списку.
    * ByKey(<ключ>)
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
4. Получить Uri запроса от билдера
    ```charp
    odataQueryBuilder.ToUri()
    ```

## Примеры

#### ByKey - по ключу
1. Запрос по ключу с простым `Expand`
```charp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByKey(223123123)
    .Expand(s => s.ODataKind)
    .ToUri();
```
> http://mock/odata/ODataType(223123123)?$expand=ODataKind

2. Запрос по ключу с вложенными `Expand` и `Select`
```charp
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

#### ByList - по списку
1. Запрос по списку с простым `Expand`
```charp
var uri = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata")
    .For<ODataTypeEntity>(s => s.ODataType)
    .ByList()
    .Expand(s => new { s.ODataKind })
    .ToUri();
```
> http://mock/odata/ODataType?$expand=ODataKind
