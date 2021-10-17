using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Fakes;
using OData.QueryBuilder.Options;
using System;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryExpandTest : IClassFixture<CommonFixture>
    {
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilderDefault;

        public ODataQueryExpandTest(CommonFixture commonFixture)
        {
            _odataQueryBuilderDefault = new ODataQueryBuilder<ODataInfoContainer>(
                commonFixture.BaseUri, new ODataQueryBuilderOptions());
        }

        [Fact(DisplayName = "Nested expand by key with select should be success")]
        public void Should_nested_expand_by_key_with_select_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKind)
                        .Expand(ff => ff
                            .For<ODataCodeEntity>(s => s.ODataCode)
                                .Select(s => s.IdCode));
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Expand(ff => ff.ODataCode)
                        .Select(s => s.IdKind);
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind);
                })
                .Select(s => new { s.IdType, s.Sum })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind($expand=ODataCode($select=IdCode)),ODataKindNew($expand=ODataCode;$select=IdKind),ODataKindNew($select=IdKind)&$select=IdType,Sum");
        }


        [Fact(DisplayName = "Nested expand by key with select and orderby should be success")]
        public void Should_nested_expand_by_key_with_select_and_orderby_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderBy(s => s.EndDate);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKindNew($select=IdKind;$orderby=EndDate asc)");
        }

        [Fact(DisplayName = "Nested expand by key with select and orderbydescending should be success")]
        public void Should_nested_expand_by_key_with_select_and_orderbydescending_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderByDescending(s => s.EndDate);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKindNew($select=IdKind;$orderby=EndDate desc)");
        }

        [Fact(DisplayName = "Nested expand by key with select, orderbydescending, top should be success")]
        public void Should_nested_expand_by_key_with_select_orderbydescending_top_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderByDescending(s => s.EndDate)
                        .Top(1);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKindNew($select=IdKind;$orderby=EndDate desc;$top=1)");
        }

        [Fact(DisplayName = "Nested expand by key with select and filter should be success - 1")]
        public void Should_nested_expand_by_key_with_select_and_filter_success_1()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKind)
                        .Filter((s, y, u) => s.IdKind == 1 && y.Date(s.EndDate) == DateTime.Today && u.In(s.IdKind, new[] { 1 }))
                        .Select(s => s.IdKind);
                })
                .Select(s => new { s.IdType, s.Sum })
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType(223123123)?$expand=ODataKind($filter=IdKind eq 1 and date(EndDate) eq {DateTime.Today:s}Z and IdKind in (1);$select=IdKind)&$select=IdType,Sum");
        }

        [Fact(DisplayName = "Nested expand by key with select and filter should be success - 2")]
        public void Should_nested_expand_by_key_with_select_and_filter_success_2()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKind)
                        .Filter((s, y) => s.IdKind == 1 && y.Date(s.EndDate) == DateTime.Today)
                        .Select(s => s.IdKind);
                })
                .Select(s => new { s.IdType, s.Sum })
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType(223123123)?$expand=ODataKind($filter=IdKind eq 1 and date(EndDate) eq {DateTime.Today:s}Z;$select=IdKind)&$select=IdType,Sum");
        }

        [Fact(DisplayName = "Nested expand by key with select and filter should be success - 3")]
        public void Should_nested_expand_by_key_with_select_and_filter_success_3()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKind)
                        .Filter(s => s.IdKind == 1)
                        .Select(s => s.IdKind);
                })
                .Select(s => new { s.IdType, s.Sum })
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType(223123123)?$expand=ODataKind($filter=IdKind eq 1;$select=IdKind)&$select=IdType,Sum");
        }

        [Fact(DisplayName = "Nested expand by list with select and expand repeat should be success")]
        public void Should_nested_expand_by_list_with_select_and_expand_repeat_success()
        {
            var uri = _odataQueryBuilderDefault
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

            uri = _odataQueryBuilderDefault
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

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKind($expand=ODataCode($select=IdCode);$select=IdKind),ODataKindNew($select=IdKind),ODataKindNew($select=IdKind)");
        }

        [Fact(DisplayName = "Nested expand by list with select and orderby should be success")]
        public void Should_nested_expand_by_list_with_select_and_orderby_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderBy(s => s.EndDate);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$orderby=EndDate asc)");
        }

        [Fact(DisplayName = "Nested expand by list with select, orderby top should be success")]
        public void Should_nested_expand_by_list_with_select_orderby_top_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .Top(1)
                        .OrderBy(s => s.EndDate);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$top=1;$orderby=EndDate asc)");
        }

        [Fact(DisplayName = "Nested expand by list with select and orderby multiple sort should be success")]
        public void Should_nested_expand_by_list_with_select_and_orderby_multiple_sort_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderBy((entity, sort) => sort
                            .Ascending(entity.OpenDate)
                            .Descending(entity.ODataCode.Code)
                            .Ascending(entity.IdKind));
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$orderby=OpenDate asc,ODataCode/Code desc,IdKind asc)");
        }

        [Fact(DisplayName = "Nested expand by list with select and orderbydescending should be success")]
        public void Should_nested_expand_by_list_with_select_and_orderbydescending_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderByDescending(s => s.EndDate);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$orderby=EndDate desc)");
        }

        [Fact(DisplayName = "Nested expand by list with select, skip, top should be success")]
        public void Should_nested_expand_by_list_with_select_skip_top_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .Skip(10)
                        .Top(10);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$skip=10;$top=10)");
        }

        [Fact(DisplayName = "Nested expand by list with select, skip, top, count should be success")]
        public void Should_nested_expand_by_list_with_select_skip_top_count_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .Skip(10)
                        .Top(10)
                        .Count();
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$skip=10;$top=10;$count=true)");
        }

        [Fact(DisplayName = "Nested expand by list without all should be success")]
        public void Should_nested_expand_by_list_without_all_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(e =>
                {
                    e.For<ODataKindEntity>(ee => ee.ODataKindNew);
                    e.For<ODataKindEntity>(ee => ee.ODataKind);
                })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew,ODataKind");
        }
    }
}
