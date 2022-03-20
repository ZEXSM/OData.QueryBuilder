using System;

namespace OData.QueryBuilder.Conventions.Constants
{
    internal struct QuerySeparators
    {
        public const char Main = '&';

        public const char Nested = ';';

        public const char Begin = '?';

        public const char EqualSign = '=';

        public const char Slash = '/';

        public const char Comma = ',';

        public const char DollarSign = '$';

        [Obsolete("Remove after upgrade to netstandard 2.1")]
        public const string StringComma = ",";

        public const char Dot = '.';
    }
}
