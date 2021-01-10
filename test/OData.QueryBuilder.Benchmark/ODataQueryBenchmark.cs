using BenchmarkDotNet.Attributes;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Fakes;
using OData.QueryBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OData.QueryBuilder.Benchmark
{
    [MemoryDiagnoser]
    public class ODataQueryBenchmark
    {
        private readonly ODataQueryBuilder<ODataInfoContainer> _odataQueryBuilder;

        public static string IdCodeStatic => "testCode";

        public ODataQueryBenchmark() =>
            _odataQueryBuilder = new ODataQueryBuilder<ODataInfoContainer>("http://odata/", new ODataQueryBuilderOptions());

        [Benchmark]
        public ODataQueryBuilder<ODataInfoContainer> ODataQueryBuilder_with_options() =>
            new ODataQueryBuilder<ODataInfoContainer>("http://odata/", new ODataQueryBuilderOptions());

        [Benchmark]
        public ODataQueryBuilder<ODataInfoContainer> ODataQueryBuilder_without_options() =>
            new ODataQueryBuilder<ODataInfoContainer>("http://odata/");

        private Uri ODataQueryBuilder_without_options_and_uri_value = new Uri("http://odata/");

        [Benchmark]
        public ODataQueryBuilder<ODataInfoContainer> ODataQueryBuilder_without_options_and_uri() =>
            new ODataQueryBuilder<ODataInfoContainer>(ODataQueryBuilder_without_options_and_uri_value);

        [Benchmark]
        public Uri ODataQueryBuilderKey_Expand_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByKey(223123123)
            .Expand(s => s.ODataKind)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderKey_Expand_Simple_With_Key_String() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByKey("223123123")
            .Expand(s => s.ODataKind)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderKey_Select_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByKey(223123123)
            .Select(s => s.IdType)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderKey_Expand_Select() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByKey(223123123)
            .Expand(f => f.ODataKind)
            .Select(s => new { s.IdType, s.Sum })
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderKey_ExpandNested_Select() => _odataQueryBuilder
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


        [Benchmark]
        public Uri ODataQueryBuilderKey_ExpandNested_OrderBy() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByKey(223123123)
            .Expand(f =>
            {
                f.For<ODataKindEntity>(s => s.ODataKindNew)
                    .Select(s => s.IdKind)
                    .OrderBy(s => s.EndDate);
            })
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderKey_ExpandNested_OrderByDescending() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByKey(223123123)
            .Expand(f =>
            {
                f.For<ODataKindEntity>(s => s.ODataKindNew)
                    .Select(s => s.IdKind)
                    .OrderByDescending(s => s.EndDate);
            })
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderKey_ExpandNested_Top() => _odataQueryBuilder
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

        [Benchmark]
        public Uri ODataQueryBuilderKey_Expand_Nested_Filter() => _odataQueryBuilder
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

        [Benchmark]
        public Dictionary<string, string> ODataQueryBuilderKey_ToDicionary() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByKey("223123123")
            .Expand(s => s.ODataKind)
            .ToDictionary();

        [Benchmark]
        public Uri ODataQueryBuilderList_Expand_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Expand(s => new { s.ODataKind })
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_ExpandNested()
        {
            var uri = _odataQueryBuilder
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

            uri = _odataQueryBuilder
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

            return uri;
        }

        [Benchmark]
        public Uri ODataQueryBuilderList_ExpandNested_OrderBy() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Expand(f =>
            {
                f.For<ODataKindEntity>(s => s.ODataKindNew)
                    .Select(s => s.IdKind)
                    .OrderBy(s => s.EndDate);
            })
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_ExpandNested_Top() => _odataQueryBuilder
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

        [Benchmark]
        public Uri ODataQueryBuilderList_ExpandNested_OrderByDescending() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Expand(f =>
            {
                f.For<ODataKindEntity>(s => s.ODataKindNew)
                    .Select(s => s.IdKind)
                    .OrderByDescending(s => s.EndDate);
            })
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Select_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Select(s => s.IdType)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_OrderBy_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .OrderBy(s => s.IdType)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_OrderByDescending_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .OrderByDescending(s => s.IdType)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Count_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Count()
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Skip_Top_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Skip(1)
            .Top(1)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Call_ToString() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter(s => s.TypeCode == 44.ToString())
            .ToUri();

        private Dictionary<string, string> ODataQueryBuilderList_Filter_string_with_ReplaceCharacters_Dictionary = new Dictionary<string, string>()
        {
            { "%", "%25" },
            { "/", "%2f" },
            { "?", "%3f" },
            { "#", "%23" },
            { "&", "%26" }
        };

        private string ODataQueryBuilderList_Filter_string_with_ReplaceCharacters_ConstValue = "3 & 4 / 7 ? 8 % 9 # 1";

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_string_with_ReplaceCharacters() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(
                ODataQueryBuilderList_Filter_string_with_ReplaceCharacters_ConstValue,
                ODataQueryBuilderList_Filter_string_with_ReplaceCharacters_Dictionary))
            .ToUri();

        private string[] ODataQueryBuilderList_Filter_enumerable_string_with_ReplaceCharacters_Strings = new string[] {
            @"test\\YUYYUT",
            @"test1\\YUYY123"
        };

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_enumerable_string_with_ReplaceCharacters() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, f.ReplaceCharacters(
                ODataQueryBuilderList_Filter_enumerable_string_with_ReplaceCharacters_Strings,
                new Dictionary<string, string> { { @"\", "%5C" } })))
            .ToUri();

        private string ODataQueryBuilderList_Filter_With_ReplaceCharacters_new_dictionary_ConstValue = "3 & 4 / 7 ? 8 % 9 # 1";

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_With_ReplaceCharacters_new_dictionary() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(
                ODataQueryBuilderList_Filter_With_ReplaceCharacters_new_dictionary_ConstValue,
                new Dictionary<string, string> { { "&", "%26" } }))
            .ToUri();

        private string ODataQueryBuilderList_Filter_With_ReplaceCharacters_Value_Null_ConstValue = default(string);

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_With_ReplaceCharacters_Value_Null() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => s.ODataKind.ODataCode.Code == f.ReplaceCharacters(
                ODataQueryBuilderList_Filter_With_ReplaceCharacters_Value_Null_ConstValue,
                new Dictionary<string, string> { { "&", "%26" } }))
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Simple_Const_Int() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter(s => s.ODataKind.ODataCode.IdCode >= 3 || s.IdType == 5)
            .ToUri();

        private string ODataQueryBuilderList_Filter_Simple_Const_String_ConstValue = "3";

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Simple_Const_String() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter(s =>
                    s.ODataKind.ODataCode.Code == ODataQueryBuilderList_Filter_Simple_Const_String_ConstValue || s.ODataKind.ODataCode.Code == "5"
                    && s.ODataKind.ODataCode.Code == IdCodeStatic)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_All_Any() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => o.Any(s.ODataKind.ODataCodes, v => v.IdCode == 1)
                && o.All(s.ODataKind.ODataCodes, v => v.IdActive))
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Any1() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => o.Any(s.Tags, t => t == "testTag"))
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Any_with_or() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => o.Any(s.Labels, label => label == "lb1" || label == "lb2"))
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Any_With_Func() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => o.Any(s.ODataKind.ODataCodes, v => f.Date(v.Created) == new DateTime(2019, 2, 9)))
            .ToUri();

        private int ODataQueryBuilderList_Expand_Filter_Select_OrderBy_OrderByDescending_Skip_Top_Count_ConstValue = 2;

        [Benchmark]
        public Uri ODataQueryBuilderList_Expand_Filter_Select_OrderBy_OrderByDescending_Skip_Top_Count() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Expand(s => new { s.ODataKind })
            .Filter(s =>
                (s.IdType < ODataQueryBuilderList_Expand_Filter_Select_OrderBy_OrderByDescending_Skip_Top_Count_ConstValue && 3 <= s.ODataKind.ODataCode.IdCode)
                || s.IdType == 5
                && s.IdRule != default(int?)
                && s.IdRule == null)
            .Select(s => new { s.ODataKind, s.Sum })
            .OrderBy(s => new { s.IdType })
            .Skip(1)
            .Top(1)
            .Count()
            .ToUri();

        private bool? ODataQueryBuilderList_filter_nullable_bool_eq_null_ConstValue = default(bool?);

        [Benchmark]
        public Uri ODataQueryBuilderList_filter_nullable_bool_eq_null() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter(s => s.IsOpen == ODataQueryBuilderList_filter_nullable_bool_eq_null_ConstValue)
            .ToUri();

        private DateTime ODataQueryBuilderList_Function_Date_CurrentDateToday = new DateTime(2019, 2, 9);
        private DateTime ODataQueryBuilderList_Function_Date_CurrentDateNow = new DateTime(2019, 2, 9, 1, 2, 4);
        private ODataTypeEntity ODataQueryBuilderList_Function_Date_NewObject = new ODataTypeEntity
        {
            ODataKind = new ODataKindEntity { EndDate = new DateTime(2019, 2, 9) }
        };

        [Benchmark]
        public Uri ODataQueryBuilderList_Function_Date() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) =>
                f.Date(s.ODataKind.OpenDate) == ODataQueryBuilderList_Function_Date_CurrentDateNow
                && s.ODataKind.OpenDate == ODataQueryBuilderList_Function_Date_CurrentDateToday
                && s.ODataKind.OpenDate == DateTime.Today
                && f.Date(s.Open) == DateTime.Today
                && f.Date(s.Open) == DateTimeOffset.Now
                && s.Open == DateTime.Today
                && s.Open == ODataQueryBuilderList_Function_Date_CurrentDateToday
                && f.Date(s.Open) == ODataQueryBuilderList_Function_Date_NewObject.ODataKind.EndDate
                && f.Date(s.ODataKind.OpenDate) == new DateTime(2019, 7, 9)
                && f.Date(s.ODataKind.OpenDate) == new DateTimeOffset(ODataQueryBuilderList_Function_Date_CurrentDateToday)
                && f.Date((DateTimeOffset)s.BeginDate) == DateTime.Today)
            .ToUri();

        private DateTime? ODataQueryBuilderList_Function_Datetime_convert_CurrentDateToday = new DateTime?(new DateTime(2019, 2, 9));

        [Benchmark]
        public Uri ODataQueryBuilderList_Function_Datetime_convert() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) =>
                f.Date(s.ODataKind.OpenDate) == f.ConvertDateTimeToString(ODataQueryBuilderList_Function_Datetime_convert_CurrentDateToday.Value, "yyyy-MM-dd"))
            .ToUri();

        private DateTimeOffset? ODataQueryBuilderList_Function_Datetimeoffset_convert_CurrentDateToday = new DateTimeOffset?(new DateTime(2019, 2, 9));

        [Benchmark]
        public Uri ODataQueryBuilderList_Function_Datetimeoffset_convert() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) =>
                f.Date(s.ODataKind.OpenDate) == f.ConvertDateTimeOffsetToString(ODataQueryBuilderList_Function_Datetimeoffset_convert_CurrentDateToday.Value, "yyyy-MM-dd"))
            .ToUri();

        private string ODataQueryBuilderList_Test_Substringof_Simple_ConstValue = "p".ToUpper();
        private ODataTypeEntity ODataQueryBuilderList_Test_Substringof_Simple_NewObject = new ODataTypeEntity { TypeCode = "TypeCodeValue".ToUpper() };

        [Benchmark]
        public Uri ODataQueryBuilderList_Test_Substringof_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) =>
                f.SubstringOf("W", f.ToUpper(s.ODataKind.ODataCode.Code))
                || f.SubstringOf(ODataQueryBuilderList_Test_Substringof_Simple_ConstValue, s.ODataKind.ODataCode.Code)
                || f.SubstringOf(ODataQueryBuilderList_Test_Substringof_Simple_NewObject.TypeCode, s.ODataKindNew.ODataCode.Code)
                || f.SubstringOf("55", s.ODataKindNew.ODataCode.Code))
            .ToUri();

        private string ODataQueryBuilderList_Test_Contains_Simple_ConstValue = "p".ToUpper();
        private ODataTypeEntity ODataQueryBuilderList_Test_Contains_Simple_Simple_NewObject = new ODataTypeEntity { TypeCode = "TypeCodeValue".ToUpper() };

        [Benchmark]
        public Uri ODataQueryBuilderList_Test_Contains_Simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) =>
                f.Contains(f.ToLower(s.ODataKind.ODataCode.Code), "W")
                || f.Contains(s.ODataKind.ODataCode.Code, ODataQueryBuilderList_Test_Contains_Simple_ConstValue)
                || f.Contains(s.ODataKindNew.ODataCode.Code, ODataQueryBuilderList_Test_Contains_Simple_Simple_NewObject.TypeCode)
                || f.Contains(s.ODataKindNew.ODataCode.Code, "55"))
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_concat_string_simple() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => f.Concat(s.TypeCode, ";") == "typeCodeTest;")
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_nested_concat_string_simple1() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => f.Concat(f.Concat(s.TypeCode, ", "), s.ODataKind.ODataCode.Code) == "testTypeCode1, testTypeCode2")
            .ToUri();

        private string ODataQueryBuilderList_nested_concat_string_simple2_ConstParam = ", ";

        [Benchmark]
        public Uri ODataQueryBuilderList_nested_concat_string_simple2() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => f.Concat(f.Concat(s.TypeCode, ODataQueryBuilderList_nested_concat_string_simple2_ConstParam), s.ODataKind.ODataCode.Code) == "testTypeCode1, testTypeCode2")
            .ToUri();

        private ODataTypeEntity ODataQueryBuilderList_nested_concat_string_simple3_ConstParamObject = new ODataTypeEntity
        {
            ODataKind = new ODataKindEntity { ODataCode = new ODataCodeEntity { Code = ", " } }
        };

        [Benchmark]
        public Uri ODataQueryBuilderList_nested_concat_string_simple3() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => f.Concat(f.Concat(s.TypeCode, ODataQueryBuilderList_nested_concat_string_simple3_ConstParamObject.ODataKind.ODataCode.Code), s.ODataKind.ODataCode.Code) == "testTypeCode1, testTypeCode2")
            .ToUri();

        private string[] ODataQueryBuilderList_Operator_In_ConstStrIds = new[] { "123", "512" };
        private List<string> ODataQueryBuilderList_Operator_In_ConstStrListIds = new[] { "123", "512" }.ToList();
        private int[] ODataQueryBuilderList_Operator_In_ConstIntIds = new[] { 123, 512 };
        private List<int> ODataQueryBuilderList_Operator_In_ConstIntListIds = new[] { 123, 512 }.ToList();
        private ODataTypeEntity ODataQueryBuilderList_Operator_In_NewObject = new ODataTypeEntity { ODataKind = new ODataKindEntity { Sequence = new[] { 123, 512 }.ToList() } };
        private ODataTypeEntity ODataQueryBuilderList_Operator_In_NewObjectSequenceArray = new ODataTypeEntity { ODataKind = new ODataKindEntity { SequenceArray = new[] { 123, 512 } } };

        [Benchmark]
        public Uri ODataQueryBuilderList_Operator_In() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, ODataQueryBuilderList_Operator_In_ConstStrIds)
                && o.In(s.ODataKind.ODataCode.Code, ODataQueryBuilderList_Operator_In_ConstStrIds)
                && o.In(s.ODataKind.ODataCode.Code, ODataQueryBuilderList_Operator_In_ConstStrListIds)
                && o.In(s.IdType, ODataQueryBuilderList_Operator_In_ConstIntIds)
                && o.In(s.IdType, ODataQueryBuilderList_Operator_In_ConstIntListIds)
                && o.In((int)s.IdRule, ODataQueryBuilderList_Operator_In_ConstIntIds)
                && o.In((int)s.IdRule, ODataQueryBuilderList_Operator_In_ConstIntListIds)
                && o.In(s.ODataKind.IdKind, ODataQueryBuilderList_Operator_In_NewObject.ODataKind.Sequence)
                && o.In(s.ODataKind.ODataCode.IdCode, ODataQueryBuilderList_Operator_In_NewObjectSequenceArray.ODataKind.SequenceArray))
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_In_with_new() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => o.In(s.ODataKind.ODataCode.Code, new[] { "123", "512" }) && o.In(s.IdType, new[] { 123, 512 }))
            .ToUri();

        private bool ODataQueryBuilderList_Filter_Boolean_Values_ConstValue = false;
        private ODataTypeEntity ODataQueryBuilderList_Filter_Boolean_Values_NewObject = new ODataTypeEntity { IsOpen = false };

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Boolean_Values() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter(s => s.IsActive
                && s.IsOpen == ODataQueryBuilderList_Filter_Boolean_Values_ConstValue
                && s.IsOpen == true
                && s.ODataKind.ODataCode.IdActive == ODataQueryBuilderList_Filter_Boolean_Values_NewObject.IsOpen)
            .ToUri();

        private string[] ODataQueryBuilderList_Filter_support_parentheses_ConstStrIds = new[] { "123", "512" };
        private int ODataQueryBuilderList_Filter_support_parentheses_ConstValue = 3;

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_support_parentheses() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f, o) => s.IdRule == ODataQueryBuilderList_Filter_support_parentheses_ConstValue
                && s.IsActive
                && (f.Date(s.EndDate.Value) == default(DateTimeOffset?) || s.EndDate > DateTime.Today)
                && (f.Date((DateTimeOffset)s.BeginDate) != default(DateTime?) || f.Date((DateTime)s.BeginDate) <= DateTime.Now)
                && o.In(s.ODataKind.ODataCode.Code, ODataQueryBuilderList_Filter_support_parentheses_ConstStrIds), useParenthesis: true)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Count_Value() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Count(false)
            .ToUri();

        [Benchmark]
        public Uri ODataQueryBuilderList_Filter_Not__Bool() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter(s => s.IsActive && !(bool)s.IsOpen)
            .ToUri();

        [Benchmark]
        public Uri FilterEnumTest() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter((s, f) => s.ODataKind.Color == f.ConvertEnumToString(ColorEnum.Blue)
                && s.ODataKind.Color == ColorEnum.Blue)
            .Skip(1)
            .Top(10)
            .ToUri();

        private bool ODataQueryBuilderList_ToDicionary_ConstValue = false;
        private ODataTypeEntity ODataQueryBuilderList_ToDicionary_NewObject = new ODataTypeEntity { IsOpen = false };

        [Benchmark]
        public Dictionary<string, string> ToDicionary() => _odataQueryBuilder
            .For<ODataTypeEntity>(s => s.ODataType)
            .ByList()
            .Filter(s => s.IsActive
                && s.IsOpen == ODataQueryBuilderList_ToDicionary_ConstValue
                && s.IsOpen == true
                && s.ODataKind.ODataCode.IdActive == ODataQueryBuilderList_ToDicionary_NewObject.IsOpen)
            .Skip(1)
            .Top(10)
            .ToDictionary();
    }
}
