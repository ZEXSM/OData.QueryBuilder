using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Test.Fakes;
using System;
using System.Linq;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryBuilderTest
    {
        [Fact(DisplayName = "ODataQueryBuilderKey")]
        public void ODataQueryBuilderKey_Success()
        {
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata/");

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKind)
                        .Expand(ff => ff.For<ODataCodeEntity>(s => s.ODataCode).Select(s => s.IdCode));
                    f.For<ODataKindEntity>(s => s.ODataKindNew).Select(s => s.IdKind);
                    f.For<ODataKindEntity>(s => s.ODataKindNew).Select(s => s.IdKind);
                })
                .Select(s => new { s.IdType, s.Sum })
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind($expand=ODataCode($select=IdCode)),ODataKindNew($select=IdKind),ODataKindNew($select=IdKind)&$select=IdType,Sum");
        }

        [Fact(DisplayName = "ODataQueryBuilderList")]
        public void ODataQueryBuilderList_Success()
        {
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>("http://mock/odata/");

            var constValue = 2;
            var currentDate = DateTime.Today.ToString("yyyy-MM-dd");
            var ids = new[] { "123", "512", "4755" }.ToList();

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(s => new { s.ODataKind })
                .Filter((f, s) =>
                    (s.IdType < constValue && s.ODataKind.ODataCode.IdCode >= 3)
                    || s.IdType == 5
                    && f.Date(s.Open) == currentDate
                    && ids.Contains(s.TypeCode))
                .Select(s => new { s.ODataKind, s.Sum })
                .OrderBy(s => new { s.IdType })
                .OrderByDescending(s => s.IdType)
                .Skip(1)
                .Top(1)
                .Count()
                .ToUri();

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$expand=ODataKind&$filter=IdType lt 2 and ODataKind/ODataCode/IdCode ge 3 or IdType eq 5 and date(Open) eq {DateTime.Today.ToString("yyyy-MM-dd")} and TypeCode in ('123','512','4755')&$select=ODataKind,Sum&$orderby=IdType asc&$orderby=IdType desc&$skip=1&$top=1&$count=true");
        }
    }
}
