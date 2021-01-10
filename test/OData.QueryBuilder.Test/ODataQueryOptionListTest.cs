﻿using FluentAssertions;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Fakes;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OData.QueryBuilder.Test
{
    public class ODataQueryOptionListTest : IClassFixture<CommonFixture>
    {
        private readonly CommonFixture _commonFixture;
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilderDefault;

        public static string IdCodeStatic => "testCode";

        public ODataQueryOptionListTest(CommonFixture commonFixture)
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$expand=ODataKind");
        }

        [Fact(DisplayName = "Expand nested => Success")]
        public void ODataQueryBuilderList_ExpandNested_Success()
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$expand=ODataKind($expand=ODataCode($select=IdCode);$select=IdKind),ODataKindNew($select=IdKind),ODataKindNew($select=IdKind)");
        }

        [Fact(DisplayName = "Expand nested orderby => Success")]
        public void ODataQueryBuilderList_ExpandNested_OrderBy_Success()
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$orderby=EndDate asc)");
        }

        [Fact(DisplayName = "Expand nested top  => Success")]
        public void ODataQueryBuilderList_ExpandNested_Top_Success()
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$top=1;$orderby=EndDate asc)");
        }

        [Fact(DisplayName = "Expand nested orderby desc => Success")]
        public void ODataQueryBuilderList_ExpandNested_OrderByDescending_Success()
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$expand=ODataKindNew($select=IdKind;$orderby=EndDate desc)");
        }

        [Fact(DisplayName = "Select simple => Success")]
        public void ODataQueryBuilderList_Select_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Select(s => s.IdType)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$select=IdType");
        }

        [Fact(DisplayName = "OrderBy simple => Success")]
        public void ODataQueryBuilderList_OrderBy_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderBy(s => s.IdType)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$orderby=IdType asc");
        }

        [Fact(DisplayName = "OrderByDescending simple => Success")]
        public void ODataQueryBuilderList_OrderByDescending_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .OrderByDescending(s => s.IdType)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$orderby=IdType desc");
        }

        [Fact(DisplayName = "Count simple => Success")]
        public void ODataQueryBuilderList_Count_Simple_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Count()
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$count=true");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$skip=1&$top=1");
        }


        [Fact(DisplayName = "Filter call ToString => Success")]
        public void ODataQueryBuilderList_Filter_Call_ToString_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.TypeCode == 44.ToString())
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=TypeCode eq '44'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq '3 %26 4 %2f 7 %3f 8 %25 9 %23 1'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code in ('test%5C%5CYUYYUT','test1%5C%5CYUYY123')");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq '3 %26 4 / 7 ? 8 % 9 # 1'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq null");
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

        [Fact(DisplayName = "Filter simple const int=> Success")]
        public void ODataQueryBuilderList_Filter_Simple_Const_Int_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.ODataKind.ODataCode.IdCode >= 3 || s.IdType == 5)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/IdCode ge 3 or IdType eq 5");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code eq '3' or ODataKind/ODataCode/Code eq '5' and ODataKind/ODataCode/Code eq 'testCode'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCodes/any(v:v/IdCode eq 1) and ODataKind/ODataCodes/all(v:v/IdActive)");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any => Success")]
        public void ODataQueryBuilderList_Filter_Any_Success1()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.Tags, t => t == "testTag"))
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=Tags/any(t:t eq 'testTag')");
        }

        [Fact(DisplayName = "(ODataQueryBuilderList) Filter Any with or => Success")]
        public void ODataQueryBuilderList_Filter_Any_with_or_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.Labels, label => label == "lb1" || label == "lb2"))
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=Labels/any(label:label eq 'lb1' or label eq 'lb2')");
        }

        [Fact(DisplayName = "Filter  operators Any with func => Success")]
        public void ODataQueryBuilderList_Filter_Any_With_Func_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.Any(s.ODataKind.ODataCodes, v => f.Date(v.Created) == new DateTime(2019, 2, 9)))
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCodes/any(v:date(v/Created) eq 2019-02-09T00:00:00Z)");
        }

        [Fact(DisplayName = "Expand,Filter,Select,OrderBy,OrderByDescending,Skip,Top,Count => Success")]
        public void ODataQueryBuilderList_Expand_Filter_Select_OrderBy_OrderByDescending_Skip_Top_Count_Success()
        {
            var constValue = 2;
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

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$expand=ODataKind&$filter=IdType lt 2 and 3 le ODataKind/ODataCode/IdCode or IdType eq 5 and IdRule ne null and IdRule eq null&$select=ODataKind,Sum&$orderby=IdType asc&$skip=1&$top=1&$count=true");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IsOpen eq null");
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
                    && f.Date(s.Open) == DateTime.Today
                    && f.Date(s.Open) == DateTimeOffset.Now
                    && s.Open == DateTime.Today
                    && s.Open == currentDateToday
                    && f.Date(s.Open) == newObject.ODataKind.EndDate
                    && f.Date(s.ODataKind.OpenDate) == new DateTime(2019, 7, 9)
                    && f.Date(s.ODataKind.OpenDate) == new DateTimeOffset(currentDateToday)
                    && f.Date((DateTimeOffset)s.BeginDate) == DateTime.Today)
                .ToUri();

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$filter=" +
                $"date(ODataKind/OpenDate) eq 2019-02-09T01:02:04Z " +
                $"and ODataKind/OpenDate eq 2019-02-09T00:00:00Z " +
                $"and ODataKind/OpenDate eq {DateTime.Today:s}Z " +
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

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$filter=date(ODataKind/OpenDate) eq 2019-02-09");
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

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$filter=date(ODataKind/OpenDate) eq 2019-02-09");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=substringof('W',toupper(ODataKind/ODataCode/Code)) or substringof('P',ODataKind/ODataCode/Code) or substringof('TYPECODEVALUE',ODataKindNew/ODataCode/Code) or substringof('55',ODataKindNew/ODataCode/Code)");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=substringof('P',ODataKind/ODataCode/Code)");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=contains(tolower(ODataKind/ODataCode/Code),'W') or contains(ODataKind/ODataCode/Code,'p') or contains(ODataKindNew/ODataCode/Code,'typecodevalue') or contains(ODataKindNew/ODataCode/Code,'55')");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=contains(ODataKind/ODataCode/Code,'P')");
        }

        [Fact(DisplayName = "Concat string simple => Success")]
        public void ODataQueryBuilderList_concat_string_simple_success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(s.TypeCode, ";") == "typeCodeTest;")
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=concat(TypeCode,';') eq 'typeCodeTest;'");
        }

        [Fact(DisplayName = "Nested Concat string => Success")]
        public void ODataQueryBuilderList_nested_concat_string_simple_success1()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f) => f.Concat(f.Concat(s.TypeCode, ", "), s.ODataKind.ODataCode.Code) == "testTypeCode1, testTypeCode2")
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=concat(concat(TypeCode,', '),ODataKind/ODataCode/Code) eq 'testTypeCode1, testTypeCode2'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=concat(concat(TypeCode,', '),ODataKind/ODataCode/Code) eq 'testTypeCode1, testTypeCode2'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=concat(concat(TypeCode,', '),ODataKind/ODataCode/Code) eq 'testTypeCode1, testTypeCode2'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter='typeCodeTest;'");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter='typeCodeTest;'");
        }

        [Fact(DisplayName = "Operator IN => Success")]
        public void ODataQueryBuilderList_Operator_In_Success()
        {
            var constStrIds = new[] { "123", "512" };
            var constStrListIds = new[] { "123", "512" }.ToList();
            var constIntIds = new[] { 123, 512 };
            var constIntListIds = new[] { 123, 512 }.ToList();
            var newObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { Sequence = constIntListIds } };
            var newObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = constIntIds } };

            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, constStrIds)
                    && o.In(s.ODataKind.ODataCode.Code, constStrIds)
                    && o.In(s.ODataKind.ODataCode.Code, constStrListIds)
                    && o.In(s.IdType, constIntIds)
                    && o.In(s.IdType, constIntListIds)
                    && o.In((int)s.IdRule, constIntIds)
                    && o.In((int)s.IdRule, constIntListIds)
                    && o.In(s.ODataKind.IdKind, newObject.ODataKind.Sequence)
                    && o.In(s.ODataKind.ODataCode.IdCode, newObjectSequenceArray.ODataKind.SequenceArray))
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code in ('123','512') and ODataKind/ODataCode/Code in ('123','512') and ODataKind/ODataCode/Code in ('123','512') and IdType in (123,512) and IdType in (123,512) and IdRule in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512) and ODataKind/ODataCode/IdCode in (123,512)");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IdType in (123,512) and IdRule in (123,512) and ODataKind/IdKind in (123,512)");
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

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$filter=ODataKind/ODataCode/Code in ('123','512') and IdType in (123,512)");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IsActive and IsOpen eq false and IsOpen eq true and ODataKind/ODataCode/IdActive eq false");
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

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$filter=(((IdRule eq 3" +
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

            uri.OriginalString.Should().Be($"http://mock/odata/ODataType?$count={value.ToString().ToLower()}");
        }

        [Fact(DisplayName = "Filter not bool => Success")]
        public void ODataQueryBuilderList_Filter_Not__Bool_Success()
        {
            var uri = _odataQueryBuilderDefault
                .For<ODataTypeEntity>(s => s.ODataType)
                .ByList()
                .Filter(s => s.IsActive && !(bool)s.IsOpen)
                .ToUri();

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=IsActive and not IsOpen");
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

            uri.OriginalString.Should().Be("http://mock/odata/ODataType?$filter=ODataKind/Color eq 'Blue' and ODataKind/Color eq 2&$skip=1&$top=10");
        }

        [Fact(DisplayName = "Filter method not supported => NotSupportedException")]
        public void ODataQueryBuilderList_Filter_Method_Not_Supported_NotSupportedException()
        {
            var @string = "test";

            _odataQueryBuilderDefault
                .Invoking(c => c
                    .For<ODataTypeEntity>(s => s.ODataType)
                    .ByList()
                    .Filter((s, f) => @string.IndexOf("t") == 1)
                    .ToUri())
                .Should().Throw<NotSupportedException>().WithMessage($"Method {nameof(string.IndexOf)} not supported");
        }
    }
}
