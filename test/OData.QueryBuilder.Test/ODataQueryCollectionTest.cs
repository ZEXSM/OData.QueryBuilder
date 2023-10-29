using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Fakes;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryCollectionTest : IClassFixture<CommonFixture>
    {
        private readonly CommonFixture _commonFixture;
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilderDefault;

        public static string IdCodeStatic => "testCode";

        public ODataQueryCollectionTest(CommonFixture commonFixture)
        {
            _commonFixture = commonFixture;
            _odataQueryBuilderDefault = new ODataQueryBuilder<ODataInfoContainer>(
                commonFixture.BaseUri, new ODataQueryBuilderOptions());
        }

        [Fact(DisplayName = "Expand simple => Success")]
        public void ODataQueryBuilderList_Expand_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(s => new { s.ODataKind })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKind");
        }

        [Fact(DisplayName = "Expand dynamic property => Success")]
        public void ODataQueryBuilderList_Expand_DynamicProperty_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(s => new { ODataKind = ODataProperty.FromPath<ODataKindEntity>("ODataKind") })
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$expand=ODataKind");
        }

        [Fact(DisplayName = "Select simple => Success")]
        public void ODataQueryBuilderList_Select_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Select(s => s.IdType)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$select=IdType");
        }

        [Fact(DisplayName = "Select dynamic property => Success")]
        public void ODataQueryBuilderList_Select_DynamicProperty_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Select(s => ODataProperty.FromPath<int>("IdType"))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$select=IdType");
        }

        [Fact(DisplayName = "OrderBy simple => Success")]
        public void ODataQueryBuilderList_OrderBy_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderBy(s => s.IdType)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$orderby=IdType asc");
        }

        [Fact(DisplayName = "OrderBy dynamic property => Success")]
        public void ODataQueryBuilderList_OrderBy_DynamicProperty_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderBy(s => ODataProperty.FromPath<int>("IdType"))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$orderby=IdType asc");
        }

        [Fact(DisplayName = "Filter orderBy multiple sort => Success")]
        public void ODataQueryBuilderList_Filter_OrderBy_Multiple_Sort_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderBy((entity, sort) => sort
                    .Ascending(entity.BeginDate)
                    .Descending(entity.EndDate)
                    .Ascending(entity.IdRule)
                    .Ascending(entity.Sum)
                    .Descending(entity.ODataKind.OpenDate))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$orderby=BeginDate asc,EndDate desc,IdRule asc,Sum asc,ODataKind/OpenDate desc");
        }

        [Fact(DisplayName = "Filter orderBy multiple sort => NotSupportedException")]
        public void ODataQueryBuilderList_Filter_OrderBy_Multiple_Sort_NotSupportedException()
        {
            var uri = _odataQueryBuilderDefault
                .Invoking(i => i
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .OrderBy((entity, sort) => sort.Equals(1))
                    .ToUri()
                )
                .Should().Throw<NotSupportedException>().WithMessage($"Method {nameof(string.Equals)} not supported");
        }

        [Fact(DisplayName = "OrderByDescending simple => Success")]
        public void ODataQueryBuilderList_OrderByDescending_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderByDescending(s => s.IdType)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$orderby=IdType desc");
        }

        [Fact(DisplayName = "Count simple => Success")]
        public void ODataQueryBuilderList_Count_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Count()
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$count=true");
        }

        [Fact(DisplayName = "Skip and Top simple => Success")]
        public void ODataQueryBuilderList_Skip_Top_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Skip(1)
                .Top(1)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$skip=1&$top=1");
        }

        [Fact(DisplayName = "Filter call ToString => Success")]
        public void ODataQueryBuilderList_Filter_Call_ToString_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.TypeCode == 44.ToString())
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=TypeCode eq '44'");
        }

        [Fact(DisplayName = "Filter and expand union => Success")]
        public void ODataQueryBuilderList_FilterAndExpand_Union_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
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
                .Filter(s => s.TypeCode == 44.ToString())
                .Expand(e =>
                {
                    e.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Filter(s => s.EndDate == DateTime.Today)
                        .Select(s => s.OpenDate)
                        .Filter(s => s.IdKind == 1)
                        .Count(false);
                })
                .Filter(s => s.IdType == 3)
                .Select(s => s.IdRule)
                .Filter(s => s.IdRule == 1)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?" +
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
                "$filter=TypeCode eq '44' and IdType eq 3 and IdRule eq 1" +
                "&" +
                "$select=IdRule");
        }

        [Fact(DisplayName = "Filter string with ReplaceCharacters => Success")]
        public void ODataQueryBuilderList_Filter_string_with_ReplaceCharacters_Success()
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
                .ByList()
                .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(constValue, dictionary))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq '3 %26 4 %2f 7 %3f 8 %25 9 %23 1'");
        }

        [Fact(DisplayName = "Filter enumerable string with ReplaceCharacters => Success")]
        public void ODataQueryBuilderList_Filter_enumerable_string_with_ReplaceCharacters_Success()
        {
            var strings = new string[] {
                @"test\\YUYYUT",
                @"test1\\YUYY123"
            };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, f.ReplaceCharacters(strings, new Dictionary<string, string>(0) { { @"\", "%5C" } })))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code in ('test%5C%5CYUYYUT','test1%5C%5CYUYY123')");
        }

        [Fact(DisplayName = "Filter call ReplaceCharacters in operator In => ArgumentException")]
        public void ODataQueryBuilderList_Filter_call_ReplaceCharacters_in_operator_In_ArgumentException()
        {
            var strings = new string[] {
                @"test\\YUYYUT",
                @"test1\\YUYY123"
            };

            _odataQueryBuilderDefault
                .Invoking((r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, f.ReplaceCharacters(strings, new Dictionary<string, string>(0))))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("KeyValuePairs is null");
        }

        private string[] GetStrings() => new[] { "123", "512" };

        [Fact(DisplayName = "Filter call func in func => NotSupportedException")]
        public void ODataQueryBuilderList_Filter_call_func_in_func_NotSupportedException()
        {
            _odataQueryBuilderDefault
                .Invoking((r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, GetStrings()))
                    .ToUri())
                .Should().Throw<NotSupportedException>().WithMessage($"Method {nameof(GetStrings)} not supported");
        }

        [Fact(DisplayName = "Filter string with ReplaceCharacters new Dictionary => Success")]
        public void ODataQueryBuilderList_Filter_With_ReplaceCharacters_new_dictionary_Success()
        {
            var constValue = "3 & 4 / 7 ? 8 % 9 # 1";

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(
                    constValue,
                    new Dictionary<string, string> { { "&", "%26" } }))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq '3 %26 4 / 7 ? 8 % 9 # 1'");
        }

        [Fact(DisplayName = "Filter string with ReplaceCharacters Value => Success")]
        public void ODataQueryBuilderList_Filter_With_ReplaceCharacters_Value_Success()
        {
            var constValue = default(string);

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(
                    constValue,
                    new Dictionary<string, string> { { "&", "%26" } }))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq null");
        }

        [Fact(DisplayName = "Filter string with ReplaceCharacters KeyValuePairs => ArgumentException")]
        public void ODataQueryBuilderList_Filter_With_ReplaceCharacters_KeyValuePairs_ArgumentException()
        {
            var constValue = "3 & 4 / 7 ? 8 % 9 # 1";

            _odataQueryBuilderDefault
                .Invoking((r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                        .ByList()
                            .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(
                                constValue,
                                null))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("KeyValuePairs is null");
        }

        [Fact(DisplayName = "Filter variable dynamic property int=> Success")]
        public void ODataQueryBuilderList_Filter_Simple_Variable_DynamicProperty_Success()
        {
            string propertyName = "ODataKind.ODataCode.IdCode";

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, _) => ODataProperty.FromPath<int>(propertyName) >= 3)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/IdCode ge 3");
        }

        [Fact(DisplayName = "Filter variable dynamic property wrong type => ArgumentException")]
        public void ODataQueryBuilderList_Filter_Simple_Variable_DynamicProperty_WrongType_ArgumentException()
        {
            string propertyName = "ODataKind.ODataCode.IdCode";

            var uri = _odataQueryBuilderDefault
                .Invoking(b => b
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, _) => ODataProperty.FromPath<string>(propertyName) == "test")
                    .ToUri()).Should().Throw<ArgumentException>();
        }

        [Fact(DisplayName = "Filter const dynamic property int=> Success")]
        public void ODataQueryBuilderList_Filter_Simple_Const_DynamicProperty_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, _) => ODataProperty.FromPath<int>("ODataKind.ODataCode.IdCode") >= 3)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/IdCode ge 3");
        }

        [Fact(DisplayName = "Filter simple const int=> Success")]
        public void ODataQueryBuilderList_Filter_Simple_Const_Int_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.ODataKind.ODataCode.IdCode >= 3 || s.IdType == 5)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/IdCode ge 3 or IdType eq 5");
        }

        [Fact(DisplayName = "Filter simple const string => Success")]
        public void ODataQueryBuilderList_Filter_Simple_Const_String_Success()
        {
            var constValue = "3";
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s =>
                     s.ODataKind.ODataCode.Code == constValue || s.ODataKind.ODataCode.Code == "5"
                     && s.ODataKind.ODataCode.Code == IdCodeStatic)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq '3' or ODataKind/ODataCode/Code eq '5' and ODataKind/ODataCode/Code eq 'testCode'");
        }

        [Fact(DisplayName = "Filter  operators All/Any => Success")]
        public void ODataQueryBuilderList_Filter_All_Any_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.ODataKind.ODataCodes, v => v.IdCode == 1)
                    && o.All(s.ODataKind.ODataCodes, v => v.IdActive))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCodes/any(v:v/IdCode eq 1) and ODataKind/ODataCodes/all(v:v/IdActive)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any => Success")]
        public void ODataQueryBuilderList_Filter_Any_Success1()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.Tags, t => t == "testTag"))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=Tags/any(t:t eq 'testTag')");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any Dynamic property => Success")]
        public void ODataQueryBuilderList_Filter_Any_DynamicProperty_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(ODataProperty.FromPath<string[]>("Tags"), t => t == "testTag"))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=Tags/any(t:t eq 'testTag')");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any with or => Success")]
        public void ODataQueryBuilderList_Filter_Any_with_or_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.Labels, label => label == "lb1" || label == "lb2"))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=Labels/any(label:label eq 'lb1' or label eq 'lb2')");
        }

        [Fact(DisplayName = "Filter  operators Any with func => Success")]
        public void ODataQueryBuilderList_Filter_Any_With_Func_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.ODataKind.ODataCodes, v => f.Date(v.Created) == new DateTime(2019, 2, 9)))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCodes/any(v:date(v/Created) eq 2019-02-09T00:00:00Z)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any without func => Success")]
        public void ODataQueryBuilderList_Filter_Any_Without_Func()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.Labels))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=Labels/any()");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any with func null supressed => Success")]
        public void ODataQueryBuilderList_Filter_Any_With_Func_null_Supressed()
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyOperatorArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var func = default(Func<string, bool>);

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, _, o) => o.Any(s.Labels, func))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any with func null => ArgumentException")]
        public void ODataQueryBuilderList_Filter_Any_With_Func_null()
        {
            var func = default(Func<string, bool>);

            _odataQueryBuilderDefault.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, _, o) => o.Any(s.Labels, func))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Func is null");
        }

        [Fact(DisplayName = "Expand,Filter,Select,OrderBy,OrderByDescending,Skip,Top,Count => Success")]
        public void ODataQueryBuilderList_Expand_Filter_Select_OrderBy_OrderByDescending_Skip_Top_Count_Success()
        {
            long constValue = 2;
            var constCurrentDate = DateTime.Today.ToString("yyyy-MM-dd");

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Expand(s => new { s.ODataKind })
                .Filter(s =>
                    (s.IdType < constValue && 3 <= s.ODataKind.ODataCode.IdCode)
                    || s.IdType == 5
                    && s.IdRule != default(int?)
                    && s.IdRule == null)
                .Select(s => new { s.ODataKind, s.Sum })
                .OrderBy(s => new { s.IdType })
                .Skip(1)
                .Top(1)
                .Count()
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$expand=ODataKind&$filter=IdType lt 2 and 3 le ODataKind/ODataCode/IdCode or IdType eq 5 and IdRule ne null and IdRule eq null&$select=ODataKind,Sum&$orderby=IdType asc&$skip=1&$top=1&$count=true");
        }

        [Fact(DisplayName = "Filter nullable bool eq null => Success")]
        public void ODataQueryBuilderList_filter_nullable_bool_eq_null_success()
        {
            var constValue = default(bool?);

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsOpen == constValue)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=IsOpen eq null");
        }

        [Fact(DisplayName = "Function Date => Success")]
        public void ODataQueryBuilderList_Function_Date_Success()
        {
            var currentDateToday = new DateTime(2019, 2, 9);
            var currentDateNow = new DateTime(2019, 2, 9, 1, 2, 4);
            var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { EndDate = currentDateToday } };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.Date(s.ODataKind.OpenDate) == currentDateNow
                    && s.ODataKind.OpenDate == currentDateToday
                    && s.ODataKind.OpenDate == DateTime.Today
                    && s.ODataKind.OpenDate == new DateTimeOffset()
                    && s.ODataKind.CustomNamedProperty == "test"
                    && s.Open == new DateTime()
                    && f.Date(s.Open) == DateTime.Today
                    && f.Date(s.Open) == DateTimeOffset.Now
                    && s.Open == DateTime.Today
                    && s.Open == currentDateToday
                    && f.Date(s.Open) == newObject.ODataKind.EndDate
                    && f.Date(s.ODataKind.OpenDate) == new DateTime(2019, 7, 9)
                    && f.Date(s.ODataKind.OpenDate) == new DateTimeOffset(currentDateToday)
                    && f.Date((DateTimeOffset)s.BeginDate) == DateTime.Today)
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=" +
                $"date(ODataKind/OpenDate) eq 2019-02-09T01:02:04Z " +
                $"and ODataKind/OpenDate eq 2019-02-09T00:00:00Z " +
                $"and ODataKind/OpenDate eq {DateTime.Today:s}Z " +
                $"and ODataKind/OpenDate eq {new DateTimeOffset():s}Z " +
                $"and ODataKind/customName eq 'test' " +
                $"and Open eq {new DateTime():s}Z " +
                $"and date(Open) eq {DateTime.Today:s}Z " +
                $"and date(Open) eq {DateTimeOffset.Now:s}Z " +
                $"and Open eq {DateTime.Today:s}Z " +
                $"and Open eq 2019-02-09T00:00:00Z " +
                $"and date(Open) eq 2019-02-09T00:00:00Z " +
                $"and date(ODataKind/OpenDate) eq 2019-07-09T00:00:00Z " +
                $"and date(ODataKind/OpenDate) eq 2019-02-09T00:00:00Z " +
                $"and date(BeginDate) eq {DateTime.Today:s}Z");
        }

        [Fact(DisplayName = "Function Datetime convert => Success")]
        public void ODataQueryBuilderList_Function_Datetime_convert_Success()
        {
            var currentDateToday = new DateTime?(new DateTime(2019, 2, 9));

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.Date(s.ODataKind.OpenDate) == f.ConvertDateTimeToString(currentDateToday.Value, "yyyy-MM-dd"))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=date(ODataKind/OpenDate) eq 2019-02-09");
        }

        [Fact(DisplayName = "Function Datetime convert => Exception")]
        public void ODataQueryBuilderList_Function_Datetime_convert_exception()
        {
            var currentDateToday = new DateTime?(new DateTime(2019, 2, 9));

            _odataQueryBuilderDefault.Invoking((q) => q
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.Date(s.ODataKind.OpenDate) == f.ConvertDateTimeToString(currentDateToday.Value, "3"))
                .ToUri())
                .Should().Throw<FormatException>();
        }

        [Fact(DisplayName = "Function Datetimeoffset convert => Success")]
        public void ODataQueryBuilderList_Function_Datetimeoffset_convert_Success()
        {
            var currentDateToday = new DateTimeOffset?(new DateTime(2019, 2, 9));

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.Date(s.ODataKind.OpenDate) == f.ConvertDateTimeOffsetToString(currentDateToday.Value, "yyyy-MM-dd"))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=date(ODataKind/OpenDate) eq 2019-02-09");
        }

        [Fact(DisplayName = "Function Datetimeoffset convert => Exception")]
        public void ODataQueryBuilderList_Function_Datetimeoffset_convert_exception()
        {
            var currentDateToday = new DateTimeOffset?(new DateTime(2019, 2, 9));

            _odataQueryBuilderDefault.Invoking((q) => q
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.Date(s.ODataKind.OpenDate) == f.ConvertDateTimeOffsetToString(currentDateToday.Value, "3"))
                .ToUri())
                .Should().Throw<FormatException>();
        }

        [Fact(DisplayName = "SubstringOf with ToUpper Simple Test => Success")]
        public void ODataQueryBuilderList_Test_Substringof_Simple()
        {
            var constValue = "p".ToUpper();
            var newObject = new ODataTypeEntity { TypeCode = "TypeCodeValue".ToUpper() };
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.SubstringOf("W", f.ToUpper(s.ODataKind.ODataCode.Code))
                    || f.SubstringOf(constValue, s.ODataKind.ODataCode.Code)
                    || f.SubstringOf(newObject.TypeCode, s.ODataKindNew.ODataCode.Code)
                    || f.SubstringOf("55", s.ODataKindNew.ODataCode.Code))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=substringof('W',toupper(ODataKind/ODataCode/Code)) or substringof('P',ODataKind/ODataCode/Code) or substringof('TYPECODEVALUE',ODataKindNew/ODataCode/Code) or substringof('55',ODataKindNew/ODataCode/Code)");
        }

        [Fact(DisplayName = "SubstringOf is null or empty value => Success")]
        public void ODataQueryBuilderList_Test_SubstringOf_is_null_or_empty_value_Success()
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyFunctionArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var constValue = "p".ToUpper();
            var newObject = new ODataTypeEntity { TypeCode = string.Empty };
            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.SubstringOf(string.Empty, f.ToUpper(s.ODataKind.ODataCode.Code))
                    || f.SubstringOf(constValue, s.ODataKind.ODataCode.Code)
                    || f.SubstringOf(newObject.TypeCode, s.ODataKindNew.ODataCode.Code)
                    || f.SubstringOf(null, s.ODataKindNew.ODataCode.Code))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=substringof('P',ODataKind/ODataCode/Code)");
        }


        [Fact(DisplayName = "SubstringOf is null or empty value => ArgumentException")]
        public void ODataQueryBuilderList_Test_Substringof_is_null_or_empty_value_ArgumentException()
        {
            var constValue = default(string);
            var newObject = new ODataTypeEntity { TypeCode = string.Empty };

            _odataQueryBuilderDefault.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f) =>
                        f.SubstringOf(null, f.ToUpper(s.ODataKind.ODataCode.Code))
                        || f.SubstringOf(constValue, s.ODataKind.ODataCode.Code)
                        || f.SubstringOf(newObject.TypeCode, s.ODataKindNew.ODataCode.Code)
                        || f.SubstringOf(string.Empty, s.ODataKindNew.ODataCode.Code))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Value is empty or null");
        }

        [Fact(DisplayName = "Contains with ToLower Simple Test => Success")]
        public void ODataQueryBuilderList_Test_Contains_Simple()
        {
            var constValue = "p".ToLower();
            var newObject = new ODataTypeEntity { TypeCode = "TypeCodeValue".ToLower() };
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.Contains(f.ToLower(s.ODataKind.ODataCode.Code), "W")
                    || f.Contains(s.ODataKind.ODataCode.Code, constValue)
                    || f.Contains(s.ODataKindNew.ODataCode.Code, newObject.TypeCode)
                    || f.Contains(s.ODataKindNew.ODataCode.Code, "55"))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=contains(tolower(ODataKind/ODataCode/Code),'W') or contains(ODataKind/ODataCode/Code,'p') or contains(ODataKindNew/ODataCode/Code,'typecodevalue') or contains(ODataKindNew/ODataCode/Code,'55')");
        }

        [Fact(DisplayName = "Contains is null or empty value => ArgumentException")]
        public void ODataQueryBuilderList_Test_Contains_is_null_or_empty_value_ArgumentException()
        {
            var constValue = default(string);
            var newObject = new ODataTypeEntity { TypeCode = string.Empty };

            _odataQueryBuilderDefault.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f) =>
                        f.Contains(f.ToLower(s.ODataKind.ODataCode.Code), null)
                        || f.Contains(s.ODataKind.ODataCode.Code, constValue)
                        || f.Contains(s.ODataKindNew.ODataCode.Code, newObject.TypeCode)
                        || f.Contains(s.ODataKindNew.ODataCode.Code, string.Empty))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Value is empty or null");
        }

        [Fact(DisplayName = "Contains is null or empty value => Success")]
        public void ODataQueryBuilderList_Test_Contains_is_null_or_empty_value_Success()
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyFunctionArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var constValue = "P";
            var newObject = new ODataTypeEntity { TypeCode = string.Empty };
            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) =>
                    f.Contains(f.ToLower(s.ODataKind.ODataCode.Code), null)
                    || f.Contains(s.ODataKind.ODataCode.Code, constValue)
                    || f.Contains(s.ODataKindNew.ODataCode.Code, newObject.TypeCode)
                    || f.Contains(s.ODataKindNew.ODataCode.Code, string.Empty))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=contains(ODataKind/ODataCode/Code,'P')");
        }

        [Fact(DisplayName = "Concat string simple => Success")]
        public void ODataQueryBuilderList_concat_string_simple_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(s.TypeCode, ";") == "typeCodeTest;")
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=concat(TypeCode,';') eq 'typeCodeTest;'");
        }

        [Fact(DisplayName = "Nested Concat string => Success")]
        public void ODataQueryBuilderList_nested_concat_string_simple_success1()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(f.Concat(s.TypeCode, ", "), s.ODataKind.ODataCode.Code) == "testTypeCode1, testTypeCode2")
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=concat(concat(TypeCode,', '),ODataKind/ODataCode/Code) eq 'testTypeCode1, testTypeCode2'");
        }

        [Fact(DisplayName = "Nested Concat string => Success")]
        public void ODataQueryBuilderList_nested_concat_string_simple_success2()
        {
            var constParam = ", ";

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(f.Concat(s.TypeCode, constParam), s.ODataKind.ODataCode.Code) == "testTypeCode1, testTypeCode2")
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=concat(concat(TypeCode,', '),ODataKind/ODataCode/Code) eq 'testTypeCode1, testTypeCode2'");
        }

        [Fact(DisplayName = "Nested Concat string => Success")]
        public void ODataQueryBuilderList_nested_concat_string_simple_success3()
        {
            var constParam = ", ";
            var constParamObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { ODataCode = new ODataCodeEntity { Code = constParam } } };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(f.Concat(s.TypeCode, constParamObject.ODataKind.ODataCode.Code), s.ODataKind.ODataCode.Code) == "testTypeCode1, testTypeCode2")
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=concat(concat(TypeCode,', '),ODataKind/ODataCode/Code) eq 'testTypeCode1, testTypeCode2'");
        }

        [Fact(DisplayName = "Concat string is null or empty value argument1 => Exception")]
        public void ODataQueryBuilderList_concat_string_simple_argument1_exception()
        {
            var constValue = default(string);
            var newObject = new ODataTypeEntity { TypeCode = string.Empty };

            _odataQueryBuilderDefault.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f) => f.Concat(s.TypeCode, constValue) == "typeCodeTest;")
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Value is empty or null");
        }

        [Fact(DisplayName = "Concat string is null or empty value argument2 => Exception")]
        public void ODataQueryBuilderList_concat_string_simple_argument2_exception()
        {
            var constValue = default(string);
            var newObject = new ODataTypeEntity { TypeCode = string.Empty };

            _odataQueryBuilderDefault.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f) => f.Concat(constValue, s.TypeCode) == "typeCodeTest;")
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Value is empty or null");
        }

        [Theory(DisplayName = "Concat is null empty value agr1 => Success")]
        [InlineData("")]
        [InlineData(null)]
        public void ODataQueryBuilderList_concat_is_null_or_empty_value_agr1_success(string value)
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyFunctionArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(value, s.TypeCode) == "typeCodeTest;")
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter='typeCodeTest;'");
        }

        [Theory(DisplayName = "Concat is null empty value agr2 => Success")]
        [InlineData("")]
        [InlineData(null)]
        public void ODataQueryBuilderList_concat_is_null_or_empty_value_agr2_success(string value)
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyFunctionArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(s.TypeCode, value) == "typeCodeTest;")
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter='typeCodeTest;'");
        }

        [Fact(DisplayName = "Operator IN => Success")]
        public void ODataQueryBuilderList_Operator_In_Success()
        {
            var constStrIds = new[] { "123", "512" };
            var constStrListIds = new[] { "123", "512" }.ToList();
            var constIntIds = new[] { 123, 512 };
            var constLongIds = new long[] { 333, 555 };
            var constIntListIds = new[] { 123, 512 }.ToList();
            var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { Sequence = constIntListIds } };
            var newObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = constIntIds, SequenceLongArray = constLongIds } };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, constStrIds)
                    && o.In(s.ODataKind.ODataCode.Code, constStrIds)
                    && o.In(s.ODataKind.ODataCode.Code, constStrListIds)
                    && o.In(s.IdType, constIntIds)
                    && o.In(s.IdType, constLongIds)
                    && o.In(s.IdType, constIntListIds)
                    && o.In((int)s.IdRule, constIntIds)
                    && o.In((int)s.IdRule, constIntListIds)
                    && o.In(s.ODataKind.IdKind, newObject.ODataKind.Sequence)
                    && o.In(s.ODataKind.ODataCode.IdCode, newObjectSequenceArray.ODataKind.SequenceArray))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code in ('123','512') and ODataKind/ODataCode/Code in ('123','512') and ODataKind/ODataCode/Code in ('123','512') and IdType in (123,512) and IdType in (333,555) and IdType in (123,512) and IdRule in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512) and ODataKind/ODataCode/IdCode in (123,512)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Operator IN empty => Success")]
        public void ODataQueryBuilderList_Operator_In_Empty_Success()
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyOperatorArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var constStrIds = default(IEnumerable<string>);
            var constEmprtyStrListIds = new string[] { }.ToList();
            var constIntIds = default(int[]);
            var constEmptyIntIds = new int[0];
            var constIntListIds = new[] { 123, 512 }.ToList();
            var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { Sequence = constIntListIds } };
            var newObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = constIntIds } };

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, constStrIds)
                    && o.In(s.ODataKind.ODataCode.Code, constEmprtyStrListIds)
                    && o.In(s.IdType, constIntIds)
                    && o.In(s.IdType, constEmptyIntIds)
                    && o.In(s.IdType, constIntListIds)
                    && o.In((int)s.IdRule, constIntIds)
                    && o.In((int)s.IdRule, constIntListIds)
                    && o.In(s.ODataKind.IdKind, newObject.ODataKind.Sequence)
                    && o.In(s.ODataKind.ODataCode.IdCode, newObjectSequenceArray.ODataKind.SequenceArray))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=IdType in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512)");
        }

        [Fact(DisplayName = "Operator IN is null => ArgumentException 1")]
        public void ODataQueryBuilderList_Operator_In_is_null_1()
        {
            var constEmprtyStrListIds = new string[] { }.ToList();

            _odataQueryBuilderDefault.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, constEmprtyStrListIds))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Enumeration is empty or null");
        }

        [Fact(DisplayName = "Operator IN is null => ArgumentException 2")]
        public void ODataQueryBuilderList_Operator_In_is_null_2()
        {
            var constStrIds = default(IEnumerable<string>);

            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyOperatorArgs = false };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            odataQueryBuilder.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, constStrIds))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Enumeration is empty or null");
        }

        [Fact(DisplayName = "Operator IN is null => ArgumentException 3")]
        public void ODataQueryBuilderList_Operator_In_is_null_3()
        {
            var constIntIds = default(int[]);

            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyOperatorArgs = false };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            odataQueryBuilder.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, o) => o.In(s.IdType, constIntIds))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Enumeration is empty or null");
        }

        [Fact(DisplayName = "Operator IN is null => ArgumentException 4")]
        public void ODataQueryBuilderList_Operator_In_is_null_4()
        {
            var constIntIds = default(int[]);
            var newObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = constIntIds } };

            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyOperatorArgs = false };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            odataQueryBuilder.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.IdCode, newObjectSequenceArray.ODataKind.SequenceArray))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Enumeration is empty or null");
        }

        [Fact(DisplayName = "Operator IN is empty => ArgumentException")]
        public void ODataQueryBuilderList_Operator_In_is_empty_1()
        {
            var constEmptyIntIds = new int[0];

            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyOperatorArgs = false };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            odataQueryBuilder.Invoking(
                (r) => r
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f, o) => o.In(s.IdType, constEmptyIntIds))
                    .ToUri())
                .Should().Throw<ArgumentException>().WithMessage("Enumeration is empty or null");
        }

        [Fact(DisplayName = "Filter In operator with new => Success")]
        public void ODataQueryBuilderList_Filter_In_with_new_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, new[] { "123", "512" }) && o.In(s.IdType, new[] { 123, 512 }))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code in ('123','512') and IdType in (123,512)");
        }

        [Fact(DisplayName = "IN operator with long => Success")]
        public void ODataQueryBuilderList_IN_with_long_Success()
        {
            var longs = new long[] { 123L, 512L };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, _, o) => o.In(s.Long, longs))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=Long in (123,512)");
        }

        [Fact(DisplayName = "IN operator with float => Success")]
        public void ODataQueryBuilderList_IN_with_float_Success()
        {
            var floats = new float[] { 123.54F, 512.45F };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, _, o) => o.In(s.Float, floats))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=Float in (123.54,512.45)");
        }

        [Fact(DisplayName = "IN operator with (long/double/float/DateTime) => Success")]
        public void ODataQueryBuilderList_IN_with_double_Success()
        {
            var doubles = new double[] { 123.23D, 512.12D };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, _, o) => o.In(s.Double, doubles))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=Double in (123.23,512.12)");
        }

        [Fact(DisplayName = "IN operator with DateTime => Success")]
        public void ODataQueryBuilderList_IN_with_DateTime_Success()
        {
            var dateTimes = new DateTime[] { new DateTime(2022, 1, 31), new DateTime(2022, 2, 1, 1, 3, 2) };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, _, o) => o.In(s.DateTime, dateTimes))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=DateTime in (2022-01-31T00:00:00Z,2022-02-01T01:03:02Z)");
        }

        [Fact(DisplayName = "IN operator with enumerable => Success")]
        public void ODataQueryBuilderList_IN_with_enumerable_Success()
        {
            var enumerableString = Enumerable.Range(1, 2).Select(s => s.ToString());

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, _, o) => o.In(s.TypeCode, enumerableString))
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=TypeCode in ('1','2')");
        }

        [Fact(DisplayName = "Filter boolean values => Success")]
        public void ODataQueryBuilderList_Filter_Boolean_Values_Success()
        {
            var constValue = false;
            var newObject = new ODataTypeEntity { IsOpen = false };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsActive
                    && s.IsOpen == constValue
                    && s.IsOpen == true
                    && s.ODataKind.ODataCode.IdActive == newObject.IsOpen)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=IsActive and IsOpen eq false and IsOpen eq true and ODataKind/ODataCode/IdActive eq false");
        }

        [Fact(DisplayName = "Filter support parentheses => Success")]
        public void ODataQueryBuilderList_Filter_support_parentheses_Success()
        {
            var constStrIds = new[] { "123", "512" };
            var constValue = 3;

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => s.IdRule == constValue
                    && s.IsActive
                    && (f.Date(s.EndDate.Value) == default(DateTimeOffset?) || s.EndDate > DateTime.Today)
                    && (f.Date((DateTimeOffset)s.BeginDate) != default(DateTime?) || f.Date((DateTime)s.BeginDate) <= DateTime.Now)
                    && o.In(s.ODataKind.ODataCode.Code, constStrIds), useParenthesis: true)
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=(((IdRule eq 3" +
                $" and IsActive)" +
                $" and (date(EndDate) eq null or EndDate gt {DateTime.Today:s}Z))" +
                $" and (date(BeginDate) ne null or date(BeginDate) le {DateTime.Now:s}Z))" +
                $" and ODataKind/ODataCode/Code in ('123','512')");
        }

        [Theory(DisplayName = "Count value => Success")]
        [InlineData(true)]
        [InlineData(false)]
        public void ODataQueryBuilderList_Count_Value_Success(bool value)
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Count(value)
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$count={value.ToString().ToLower()}");
        }

        [Fact(DisplayName = "Filter not bool => Success")]
        public void ODataQueryBuilderList_Filter_Not__Bool_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsActive && !(bool)s.IsOpen)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=IsActive and not IsOpen");
        }

        [Fact(DisplayName = "ToDicionary => Success")]
        public void ODataQueryBuilderList_ToDicionary()
        {
            var constValue = false;
            var newObject = new ODataTypeEntity { IsOpen = false };

            var dictionary = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsActive
                    && s.IsOpen == constValue
                    && s.IsOpen == true
                    && s.ODataKind.ODataCode.IdActive == newObject.IsOpen)
                .Skip(1)
                .Top(10)
                .ToDictionary();

            var resultEquivalent = new Dictionary<string, string>
            {
                { "$filter", "IsActive and IsOpen eq false and IsOpen eq true and ODataKind/ODataCode/IdActive eq false" },
                { "$skip", "1" },
                { "$top", "10" }
            };

            dictionary.Should().BeEquivalentTo(resultEquivalent);
        }

        [Fact(DisplayName = "ToDicionary empty resource => Success")]
        public void ODataQueryBuilderList_ToDicionary_Empty_Resource()
        {
            var dictionary = new ODataQueryBuilder()
                .For<ODataTypeEntity>(string.Empty)
                .ByList()
                .Expand(s => s.ODataKind)
                .Filter(s => s.IsActive == true)
                .Select(s => s.Open)
                .Skip(1)
                .Top(10)
                .ToDictionary();

            var resultEquivalent = new Dictionary<string, string>
            {
                { "$expand", "ODataKind" },
                { "$filter", "IsActive eq true" },
                { "$select", "Open" },
                { "$skip", "1" },
                { "$top", "10" }
            };

            dictionary.Should().BeEquivalentTo(resultEquivalent);
        }

        [Fact(DisplayName = "ToDicionary Complex => Success")]
        public void ODataQueryBuilderList_ToDicionary_Complex_Success()
        {
            var dictionary = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
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
                .Filter(s => s.IdRule == 3)
                .Select(s => new { s.IdType, s.Sum })
                .OrderBy(s => s.IdRule)
                .Skip(10)
                .Top(10)
                .ToDictionary();

            var resultEquivalent = new Dictionary<string, string>
            {
                ["$expand"] = "ODataKind($expand=ODataCode($select=IdCode)),ODataKindNew($expand=ODataCode;$select=IdKind),ODataKindNew($select=IdKind)",
                ["$filter"] = "IdRule eq 3",
                ["$select"] = "IdType,Sum",
                ["$orderby"] = "IdRule asc",
                ["$skip"] = "10",
                ["$top"] = "10"
            };

            dictionary.Should().BeEquivalentTo(resultEquivalent);
        }

        [Fact(DisplayName = "ToDicionary => Exception")]
        public void ODataQueryBuilderList_ToDicionary_Exception()
        {
            _odataQueryBuilderDefault
                .Invoking(s => s
                    .For<ODataTypeEntity>(null)
                        .ByList()
                            .Filter(s => s.IsActive)
                        .ToDictionary())
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Resource name is null (Parameter 'resource')");
        }

        [Fact(DisplayName = "ToDicionary => Exception 2")]
        public void ODataQueryBuilderList_ToDicionary_Exception_2()
        {
            new ODataQueryBuilder()
                .Invoking(s => s
                    .For<ODataTypeEntity>(null)
                        .ByList()
                            .Filter(s => s.IsActive)
                        .ToDictionary())
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Resource name is null (Parameter 'resource')");
        }

        [Fact(DisplayName = "ToDicionary filter and expand union => Success")]
        public void ODataQueryBuilderList_ToDicionary_FilterAndExpand_Union_Success()
        {
            var dictionary = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
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
                .Filter(s => s.TypeCode == 44.ToString())
                .Expand(e =>
                {
                    e.For<ODataKindEntity>(s => s.ODataKindNew)
                        .Filter(s => s.EndDate == DateTime.Today)
                        .Select(s => s.OpenDate)
                        .Filter(s => s.IdKind == 1)
                        .Count(false);
                })
                .Filter(s => s.IdType == 3)
                .Select(s => s.IdRule)
                .Filter(s => s.IdRule == 1)
                .ToDictionary();

            var resultEquivalent = new Dictionary<string, string>
            {
                ["$expand"] = $"ODataKind($expand=ODataCode($filter=Code eq 'test' and IdActive;$select=Created);$filter=EndDate eq {DateTime.Today:s}Z and IdKind eq 1;$select=OpenDate;$count=false),ODataKindNew($filter=EndDate eq {DateTime.Today:s}Z and IdKind eq 1;$select=OpenDate;$count=false)",
                ["$filter"] = "TypeCode eq '44' and IdType eq 3 and IdRule eq 1",
                ["$select"] = "IdRule"
            };

            dictionary.Should().BeEquivalentTo(resultEquivalent);
        }

        [Fact(DisplayName = "Filter Enum => Success")]
        public void ODataQueryBuilderList_Filter_Enum_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => s.ODataKind.Color == f.ConvertEnumToString(ColorEnum.Blue)
                    && s.ODataKind.Color == ColorEnum.Blue)
                .Skip(1)
                .Top(10)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/Color eq 'Blue' and ODataKind/Color eq 2&$skip=1&$top=10");
        }

        [Fact(DisplayName = "Filter method not supported => NotSupportedException")]
        public void ODataQueryBuilderList_Filter_Method_Not_Supported_NotSupportedException()
        {
            var @string = "test";

            _odataQueryBuilderDefault
                .Invoking(c => c
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f) => @string.Trim() == "s")
                    .ToUri())
                .Should().Throw<NotSupportedException>().WithMessage($"Method {nameof(string.Trim)} not supported");
        }

        [Fact(DisplayName = "IndexOf Test => Success")]
        public void ODataQueryBuilderList_Test_IndexOf()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.IndexOf(s.ODataKind.ODataCode.Code, "testCode") == 1)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=indexof(ODataKind/ODataCode/Code,'testCode') eq 1");
        }

        [Fact(DisplayName = "Filter Guid Test => Success")]
        public void ODataQueryBuilderList_Test_Filter_Guid()
        {
            var newGuid = Guid.NewGuid();

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.Id == new Guid() || s.Id == newGuid)
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=Id eq 00000000-0000-0000-0000-000000000000 or Id eq {newGuid}");
        }

        [Fact(DisplayName = "Without base url => Success")]
        public void ODataQueryBuilder_Test_without_base_url()
        {
            var uri = new ODataQueryBuilder<ODataInfoContainer>()
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IdRule == 1)
                .ToUri();

            uri.Should().Be("ODataType?$filter=IdRule eq 1");
        }

        [Fact(DisplayName = "Without base url and root model => Success")]
        public void ODataQueryBuilder_Test_without_base_url_and_root_model()
        {
            var uri = new ODataQueryBuilder()
                .For<ODataTypeEntity>("ODataType")
                .ByList()
                .Filter(s => s.IdRule == 1)
                .ToUri();

            uri.Should().Be("ODataType?$filter=IdRule eq 1");
        }

        [Fact(DisplayName = "StartsWith Test => Success")]
        public void ODataQueryBuilder_Test_StartsWith_Success()
        {
            var uri = new ODataQueryBuilder<ODataInfoContainer>()
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.StartsWith(s.TypeCode, "some-name"))
                .ToUri();

            uri.Should().Be("ODataType?$filter=startswith(TypeCode,'some-name')");
        }

        [Fact(DisplayName = "StartsWith Test null value => ArgumentException")]
        public void ODataQueryBuilder_Test_StartsWith_null_value_ArgumentException()
        {
            var byList = new ODataQueryBuilder<ODataInfoContainer>()
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList();

            byList.Invoking(s => s.Filter((s, f) => f.StartsWith(s.TypeCode, null)).ToUri())
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Value is empty or null");
        }

        [Fact(DisplayName = "StartsWith Test null value => Success")]
        public void ODataQueryBuilder_Test_StartsWith_null_value_Success()
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyFunctionArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.StartsWith(s.TypeCode, null))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=");
        }

        [Fact(DisplayName = "Should use filter long, double => Success")]
        public void ODataQueryBuilder_Filter_use_long_double()
        {
            var uri = new ODataQueryBuilder<ODataInfoContainer>(_commonFixture.BaseUri)
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.TotalCount == 1 && s.Money == 0.11)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=TotalCount eq 1 and Money eq 0.11");
        }

        [Fact(DisplayName = "Function Length => Success")]
        public void ODataQueryBuilder_Function_Length_Success()
        {
            var uri = new ODataQueryBuilder<ODataInfoContainer>()
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Length(s.TypeCode) > 0)
                .ToUri();

            uri.Should().Be("ODataType?$filter=length(TypeCode) gt 0");
        }

        [Fact(DisplayName = "Try convert decimal => Success")]
        public void ODataQueryBuilderList_Decimal_Success()
        {
            decimal decimalValue = 10.3M;
            double doubleValue = 20;

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.Sum == decimalValue && s.Money == doubleValue)
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=Sum eq 10.3 and Money eq 20");
        }

        [Fact(DisplayName = "Function Cast => Success")]
        public void ODataQueryBuilder_Function_Cast_Success()
        {
            var uri = new ODataQueryBuilder<ODataInfoContainer>()
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Contains(f.Cast(s.ODataKindNew.ODataCode.Code, "Edm.String"), "55"))
                .ToUri();

            uri.Should().Be("ODataType?$filter=contains(cast(ODataKindNew/ODataCode/Code,Edm.String),'55')");
        }

        [Theory(DisplayName = "Function Cast => Exception")]
        [InlineData(null)]
        [InlineData("")]
        public void ODataQueryBuilder_Function_Cast_Exception(string value)
        {
            _odataQueryBuilderDefault
               .Invoking((r) => r
                   .For<ODataTypeEntity>(s => s.ODataType)
                   .ByList()
                   .Filter((s, f) => f.Contains(f.Cast(s.ODataKindNew.ODataCode.Code, value), "55"))
                   .ToUri())
               .Should().Throw<ArgumentException>().WithMessage("Type is empty or null");
        }

        [Theory(DisplayName = "Function Cast => Skip Exception")]
        [InlineData(null)]
        [InlineData("")]
        public void ODataQueryBuilder_Function_Cast_Skip_Exception(string value)
        {
            var odataQueryBuilderOptions = new ODataQueryBuilderOptions { SuppressExceptionOfNullOrEmptyFunctionArgs = true };
            var odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>(
                _commonFixture.BaseUri, odataQueryBuilderOptions);

            var uri = odataQueryBuilder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Contains(f.Cast(s.ODataKindNew.ODataCode.Code, value), "55"))
                .ToUri();

            uri.Should().Be("http://mock/odata/ODataType?$filter=contains(,'55')");
        }

        [Fact(DisplayName = "UseCorrectDateTimeFormat Convert => Success")]
        public void ODataQueryBuilderList_UseCorrectDatetimeFormat_Convert_Success()
        {
            var builder = new ODataQueryBuilder<ODataInfoContainer>(
                 _commonFixture.BaseUri,
                 new ODataQueryBuilderOptions { UseCorrectDateTimeFormat = true });

            var dateTimeLocal = new DateTime(
                year: 2023, month: 04, day: 07, hour: 12, minute: 30, second: 20, kind: DateTimeKind.Local);
            var dateTimeUtc = new DateTime(
                year: 2023, month: 04, day: 07, hour: 12, minute: 30, second: 20, kind: DateTimeKind.Utc);
            var dateTimeOffset = new DateTimeOffset(
                year: 2023, month: 04, day: 07, hour: 12, minute: 30, second: 20, offset: TimeSpan.FromHours(+7));
            var dateTimeOffset2 = new DateTimeOffset(
                year: 2023, month: 04, day: 07, hour: 12, minute: 30, second: 20, offset: TimeSpan.FromHours(-7));
            var nowOffset = $"{DateTimeOffset.Now:zzz}".Replace("+", "%2B");

            var uri = builder
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((o) =>
                    o.DateTime == dateTimeLocal
                    && o.DateTime == dateTimeUtc
                    && o.DateTime == dateTimeOffset
                    && o.DateTime == dateTimeOffset2)
                .ToUri();

            uri.Should().Be($"http://mock/odata/ODataType?$filter=" +
                $"DateTime eq 2023-04-07T12:30:20{nowOffset} and " +
                $"DateTime eq 2023-04-07T12:30:20%2B00:00 and " +
                $"DateTime eq 2023-04-07T12:30:20%2B07:00 and " +
                $"DateTime eq 2023-04-07T12:30:20-07:00");
        }
    }
}