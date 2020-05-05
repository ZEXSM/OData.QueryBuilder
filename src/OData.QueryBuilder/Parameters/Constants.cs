namespace OData.QueryBuilder.Parameters
{
    internal struct Constants
    {
        public const string QueryStringSeparator = "&";
        public const char QueryCharSeparator = '&';
        public const string QueryStringNestedSeparator = ";";
        public const char QueryCharNestedSeparator = ';';
        public const string QueryStringBegin = "?";
        public const char QueryCharBegin = '?';
        public const string QueryStringEqualSign = "=";
        public const char QueryCharEqualSign = '=';
        public const string SlashStringSeparator = "/";
        public const string CommaStringSeparator = ",";
        public const char DotCharSeparator = '.';

        public const string QueryParameterSelect = "$select";
        public const string QueryParameterExpand = "$expand";
        public const string QueryParameterFilter = "$filter";
        public const string QueryParameterOrderBy = "$orderby";
        public const string QueryParameterTop = "$top";
        public const string QueryParameterSkip = "$skip";
        public const string QueryParameterCount = "$count";

        public const string QuerySortAsc = "asc";
        public const string QuerySortDesc = "desc";
    }
}
