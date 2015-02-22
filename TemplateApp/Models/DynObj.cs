using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateApp.Models
{
  

    public class DynObj : Dynamitey.DynamicObjects.Dictionary
    {
        public struct TryDynObj<T>
        {
            public bool Existent { get; set; }
            public bool Valid { get; set; }
            public DynObj Source { get; set; }
            public T ValueOrDefault { get; set; }
        }

        public TryDynObj<T> Maybe<T>(string propName)
        {
            if (propName == null)
                throw new NullReferenceException("propName");

            object value;
            if (!this.TryGetValue(propName, out value))
            {
                return new TryDynObj<T> { Source = this };
            }

            if (value is T)
            {
                return new TryDynObj<T> { Source = this, Existent = true, Valid = true, ValueOrDefault = (T)value };
            }

            if (value == null || Convert.IsDBNull(value))
            {
                return new TryDynObj<T> { Source = this, Existent = true };
            }

            try
            {
                var res = Convert.ChangeType(value, typeof(T));

                return new TryDynObj<T> { Source = this, Existent = true, ValueOrDefault = (T)res };
            }
            catch
            {
                return new TryDynObj<T> { Source = this, Existent = true, Valid = false };
            }
        }

        public T Get<T>(string propName)
        {
            if (propName == null)
                throw new NullReferenceException("propName");

            return (T)this[propName];
        }
        public T GetValueOrDefault<T>(string propName)
        {
            if (propName == null)
                throw new NullReferenceException("propName");

            object value;
            if (!this.TryGetValue(propName, out value))
                return default(T);

            if (value is T)
                return (T)value;

            if (value == null || Convert.IsDBNull(value))
                return default(T);

            try
            {
                var res = Convert.ChangeType(value, typeof(T));
                return (T)res;
            }
            catch
            {
                return default(T);
            }
        }
        public bool TryGetValue<T>(string propName, T realValue)
        {
            if (propName == null)
                throw new NullReferenceException("propName");

            object objValue;
            if (!this.TryGetValue(propName, out objValue))
            {
                realValue = default(T);
                return false;
            }

            if (objValue is T)
            {
                realValue = (T)objValue;
                return true;
            }

            if (objValue == null)
            {
                realValue = default(T);
                return false;
            }

            try
            {
                var convertedValue = Convert.ChangeType(objValue, typeof(T));
                realValue = (T)convertedValue;

                return false;
            }
            catch
            {
                realValue = default(T);
                return false;
            }
        }
        public DynObj Fetch(string propName, object value)
        {
            this[propName] = value;
            return this;
        }
    }

}