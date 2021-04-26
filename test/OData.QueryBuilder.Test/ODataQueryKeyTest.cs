﻿using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Fakes;
using OData.QueryBuilder.Options;
using System;
using System.Linq;
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

        [Fact(DisplayName = "Expand nested and Select => Success")]
        public void ODataQueryBuilderKey_ExpandNested_Select_Success()
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


        [Fact(DisplayName = "Expand nested orderby => Success")]
        public void ODataQueryBuilderList_ExpandNested_OrderBy_Success()
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

        [Fact(DisplayName = "Expand nested orderby desc => Success")]
        public void ODataQueryBuilderList_ExpandNested_OrderByDescending_Success()
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

        [Fact(DisplayName = "Expand nested top => Success")]
        public void ODataQueryBuilderList_ExpandNested_Top_Success()
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

        [Fact(DisplayName = "Expand nested Filter1 => Success")]
        public void ODataQueryBuilderKey_Expand_Nested_Filter1_Success()
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

        [Fact(DisplayName = "Expand nested Filter2 => Success")]
        public void ODataQueryBuilderKey_Expand_Nested_Filter2_Success()
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

        [Fact(DisplayName = "Expand nested Filter3 => Success")]
        public void ODataQueryBuilderKey_Expand_Nested_Filter3_Success()
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

        [Fact(DisplayName = "ToDicionary => Success")]
        public void ToDicionaryTest()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey("223123123")
                .Expand(s => s.ODataKind)
                .ToDictionary();
        }
    }
}
