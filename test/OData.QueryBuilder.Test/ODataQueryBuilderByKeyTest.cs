using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Test.Fakes;
using System.Linq;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryBuilderByKeyTest : IClassFixture<CommonFixture>
    {
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilder;

        public ODataQueryBuilderByKeyTest(CommonFixture commonFixture) =>
            _odataQueryBuilder = commonFixture.ODataQueryBuilder1;

        [Fact(DisplayName = "(ODataQueryBuilderKey) Expand simple => Success")]
        public void ODataQueryBuilderKey_Expand_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(s => s.ODataKind)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind");
        }

        [Fact(DisplayName = "(ODataQueryBuilderKey) Expand simple with key string => Success")]
        public void ODataQueryBuilderKey_Expand_Simple_With_Key_String_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey("223123123")
                .Expand(s => s.ODataKind)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType('223123123')?$expand=ODataKind");
        }

        [Fact(DisplayName = "(ODataQueryBuilderKey) Select simple => Success")]
        public void ODataQueryBuilderKey_Select_Simple_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Select(s => s.IdType)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$select=IdType");
        }

        [Fact(DisplayName = "(ODataQueryBuilderKey) Expand and Select => Success")]
        public void ODataQueryBuilderKey_Expand_Select_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f => f.ODataKind)
                .Select(s => new { s.IdType, s.Sum })
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind&$select=IdType,Sum");
        }

        [Fact(DisplayName = "(ODataQueryBuilderKey) Expand nested and Select => Success")]
        public void ODataQueryBuilderKey_ExpandNested_Select_Success()
        {
            var uri = _odataQueryBuilder
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind($expand=ODataCode($select=IdCode)),ODataKindNew($select=IdKind),ODataKindNew($select=IdKind)&$select=IdType,Sum");
        }


        [Fact(DisplayName = "(ODataQueryBuilderKey) Expand nested orderby => Success")]
        public void ODataQueryBuilderList_ExpandNested_OrderBy_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderBy(s => s.EndDate);
                })
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKindNew($select=IdKind;$orderby=EndDate asc)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderKey) Expand nested orderby desc => Success")]
        public void ODataQueryBuilderList_ExpandNested_OrderByDescending_Success()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f =>
                {
                    f.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Select(s => s.IdKind)
                        .OrderByDescending(s => s.EndDate);
                })
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKindNew($select=IdKind;$orderby=EndDate desc)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderKey) Expand nested Filter => Success")]
        public void ODataQueryBuilderKey_Expand_Nested_Filter_Success()
        {
            var uri = _odataQueryBuilder
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind($filter=IdKind eq 1;$select=IdKind)&$select=IdType,Sum");
        }

        [Fact(DisplayName = "(ODataQueryBuilderKey) ToDicionary => Success")]
        public void ToDicionaryTest()
        {
            var uri = _odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey("223123123")
                .Expand(s => s.ODataKind)
                .ToDictionary();
        }
    }
}
