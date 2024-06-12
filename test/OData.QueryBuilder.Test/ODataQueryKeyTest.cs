﻿using FluentAssertions;
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

        [Fact(DisplayName = "Expand union => Success")]
        public void ODataQueryBuilderKey_Expand_Union_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(333)
                .Expand(e =>
                {
                    e.For<ODataKindEntity>(s => s.ODataKind)
                        .Expand(a =>
                        {
                            a.For<ODataCodeEntity>(f => f.ODataCode)
                                .Filter(v => v.Code == "test")
                                .Select(v => v.Created)
                                .Filter(v => v.IdActive);
                        })
                        .Filter(s => s.EndDate == DateTime.Today)
                        .Select(s => s.OpenDate)
                        .Filter(s => s.IdKind == 1)
                        .Count(false);
                })
                .Expand(e =>
                {
                    e.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Filter(s => s.EndDate == DateTime.Today)
                        .Select(s => s.OpenDate)
                        .Filter(s => s.IdKind == 1)
                        .Count(false);
                })
                .Select(s => s.IdRule)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(333)?" +
                "$expand=" +
                    "ODataKind(" +
                        "$expand=" +
                            "ODataCode(" +
                                "$filter=Code eq 'test' and IdActive;" +
                                "$select=Created" +
                            ");" +
                        $"$filter=EndDate eq {DateTime.Today:s}Z and IdKind eq 1;" +
                        "$select=OpenDate;" +
                        "$count=false" +
                    ")," +
                    "ODataKindNew(" +
                        $"$filter=EndDate eq {DateTime.Today:s}Z and IdKind eq 1;" +
                        "$select=OpenDate;" +
                        "$count=false" +
                    ")" +
                "&" +
                "$select=IdRule");
        }
        
        [Fact(DisplayName = "Filter not bool => Success")]
        public void ODataQueryBuilderKey_Filter_Not__Bool_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(123)
                .Filter(s => s.IsActive && !(bool)s.IsOpen)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(123)?$filter=IsActive and not IsOpen");
        }
        
        [Fact(DisplayName = "Filter string with ReplaceCharacters => Success")]
        public void ODataQueryBuilderKey_Filter_string_with_ReplaceCharacters_Success()
        {
            var dictionary = new Dictionary<string, string>()
            {
                { "%", "%25" },
                { "/", "%2f" },
                { "?", "%3f" },
                { "#", "%23" },
                { "&", "%26" }
            };

            var constValue = "3 & 4 / 7 ? 8 % 9 # 1";

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(123)
                .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(constValue, dictionary))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(123)?$filter=ODataKind/ODataCode/Code eq '3 %26 4 %2f 7 %3f 8 %25 9 %23 1'");
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

        [Fact(DisplayName = "Navigation properties => Success")]
        public void Navigation_properties_test_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey(223123123)
                .For<ODataKindEntity>(s => s.ODataKind)
                .ByKey(223123124)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType(223123123)/ODataKind(223123124)");
        }

        [Fact(DisplayName = "Navigation properties with select => Success")]
        public void Navigation_properties_test_with_select_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByKey("223123123")
                .For<ODataKindEntity>(s => s.ODataKind)
                .ByKey("223123124")
                .Select(s => s.Color)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType('223123123')/ODataKind('223123124')?$select=Color");
        }
    }
}
