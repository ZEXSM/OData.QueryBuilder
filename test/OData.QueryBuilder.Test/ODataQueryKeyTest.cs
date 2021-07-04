using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Fakes;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryKeyTest : IClassFixture<CommonFixture>
    {
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilderDefault;

        public ODataQueryKeyTest(CommonFixture commonFixture) =>
            _odataQueryBuilderDefault = new ODataQueryBuilder<ODataInfoContainer>(
                commonFixture.BaseUrl, new ODataQueryBuilderOptions());

        [Fact(DisplayName = "Expand simple => Success")]
        public void ODataQueryBuilderKey_Expand_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(s => s.ODataKind)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind");
        }

        [Fact(DisplayName = "Simple with key ints => Success")]
        public void ODataQueryBuilderKey_Simple_With_Key_Ints_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123, 223123124, 223123125)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123,223123124,223123125)");
        }

        [Fact(DisplayName = "Expand simple with key string => Success")]
        public void ODataQueryBuilderKey_Expand_Simple_With_Key_String_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey("223123123")
                .Expand(s => s.ODataKind)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType('223123123')?$expand=ODataKind");
        }

        [Fact(DisplayName = "Simple with key strings => Success")]
        public void ODataQueryBuilderKey_Simple_With_Key_Strings_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey("223123123", "223123124", "223123125")
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType('223123123','223123124','223123125')");
        }

        [Fact(DisplayName = "Expand simple with key guid => Success")]
        public void ODataQueryBuilderKey_Expand_Simple_With_Key_Guid_Success()
        {
            var id = Guid.NewGuid();

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(id)
                .Expand(s => s.ODataKind)
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType({id})?$expand=ODataKind");
        }

        [Fact(DisplayName = "Simple with key guids => Success")]
        public void ODataQueryBuilderKey_Simple_With_Key_Guids_Success()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(id1, id2)
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType({id1},{id2})");
        }

        [Fact(DisplayName = "Select simple => Success")]
        public void ODataQueryBuilderKey_Select_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Select(s => s.IdType)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)?$select=IdType");
        }

        [Fact(DisplayName = "Expand and Select => Success")]
        public void ODataQueryBuilderKey_Expand_Select_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .Expand(f => f.ODataKind)
                .Select(s => new { s.IdType, s.Sum })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)?$expand=ODataKind&$select=IdType,Sum");
        }

        [Fact(DisplayName = "ToDicionary => Success")]
        public void ToDicionaryTest()
        {
            var dictionary = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey("223123123")
                .Expand(s => s.ODataKind)
                .ToDictionary();

            var resultEquivalent = new Dictionary<string, string>
            {
                { "$expand", "ODataKind" },
            };

            dictionary.Should().BeEquivalentTo(resultEquivalent);
        }
    }
}
