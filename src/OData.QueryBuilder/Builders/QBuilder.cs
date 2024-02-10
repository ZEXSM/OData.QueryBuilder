using System.Text;

namespace OData.QueryBuilder.Builders
{
    internal class QBuilder
    {
        private readonly StringBuilder _builder;
        private readonly StringBuilder _templateBuilder;

        private readonly bool _includeTemplate;

        public QBuilder(string baseUrl, bool includeTemplate = false)
        {
            _builder = new StringBuilder(baseUrl);
            _templateBuilder = new StringBuilder(baseUrl);
            _includeTemplate = includeTemplate;
        }

        public QBuilder(bool includeTemplate = false)
        {
            _builder = new StringBuilder();
            _templateBuilder = new StringBuilder();
            _includeTemplate = includeTemplate;
        }

        public bool IsEmpty() => _builder.Length == 0;

        public QBuilder Append(string value)
        {
            _builder.Append(value);

            if (_includeTemplate)
            {
                _templateBuilder.Append(value);
            }

            return this;
        }

        public QBuilder Append(char value)
        {
            _builder.Append(value);

            if (_includeTemplate)
            {
                _templateBuilder.Append(value);
            }

            return this;
        }

        public QBuilder LastRemove(char @char)
        {
            if (_builder.Length == 0)
            {
                return this;
            }

            var lastIndex = _builder.Length - 1;

            if (_builder[lastIndex] == @char)
            {
                _builder.Remove(lastIndex, 1);
            }

            return this;
        }

        public QBuilder LastReplace(char oldChar, char newChar)
        {
            var lastIndex = _builder.Length - 1;

            if (_builder[lastIndex] == oldChar)
            {
                _builder[lastIndex] = newChar;
            }

            return this;
        }

        public QBuilder Merge(string startValue, char endChar, string value)
        {
            var positionEndFilter = -1;

            for (var position = _builder.Length - 1; position >= 0; position--)
            {
                if (_builder[position] == endChar)
                {
                    positionEndFilter = position;

                    continue;
                }

                if (_builder[position] == startValue[0]
                    && _builder[position + 1] == startValue[1])
                {

                    _builder.Insert(positionEndFilter, value);

                    break;
                }
            }

            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
