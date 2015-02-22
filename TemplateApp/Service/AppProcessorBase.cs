using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateApp.Service
{
    public abstract class AppProcessorBase
    {
        private readonly string _value;
        private readonly int _resultLimit;
        protected  Lazy<Dictionary<string, Func<object, IEnumerable<string>>>> _dictionaryAnswer;


        protected AppProcessorBase(string value, int resultLimit)
        {
            this._value = value;
            this._resultLimit = resultLimit;
            _dictionaryAnswer = new Lazy<Dictionary<string, Func<object, IEnumerable<string>>>>(ValueFactory);
        }

        public string Value
        {
            get { return _value; }
        }

        protected int ResultLimit
        {
            get { return _resultLimit; }
        }

        protected abstract Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory();

        public  virtual IEnumerable<string> GetResult(object customArg = null)
        {
            Func<object, IEnumerable<string>> method;
            if (!_dictionaryAnswer.Value.TryGetValue(_value, out method))
            {
                return Enumerable.Empty<string>();
            }

            var res = EvaluateResult(method, customArg);
            return res;
        }

        internal IEnumerable<string> EvaluateResult(Func<object, IEnumerable<string>> method, object customArg)
        {
            return method(customArg);
        }

        public abstract IEnumerable<string> GetNonParticipatingCities();
    }
}