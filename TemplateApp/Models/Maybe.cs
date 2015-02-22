using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Dynamitey.DynamicObjects;

namespace TemplateApp.Models
{
    public interface IMaybe<T> { }

    public class Nothing<T> : IMaybe<T>
    {
        private Nothing()
        {

        }
        public static readonly IMaybe<T> Default = new Nothing<T>();
        public override string ToString()
        {
            return "GetNothing";
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Nothing<T>))
                return false;

            return true;
        }
    }

    public class FallBack<T> : IMaybe<T>
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is FallBack<T>))
                return false;

            var other = ((FallBack<T>) obj);
            return other.Description == this.Description && other.ErrorCode == this.ErrorCode;
        }


    }
    public class Just<T> : IMaybe<T>
    {
        public T Value { get; private set; }
        public Just(T value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public static class Maybe
    {
        public static IMaybe<T> GetNothing<T>()
        {
            return Nothing<T>.Default;
        }

        public static IMaybe<int> Div(this int numerator, int denominator)
        {
            return denominator == 0
                       ? GetNothing<int>()
                       : new Just<int>(numerator / denominator);
        }

        public static IMaybe<T> ToMaybe<T>(this T value)
        {
            return new Just<T>(value);
        }

        public static IMaybe<B> Bind<A, B>(this IMaybe<A> a, Func<A, IMaybe<B>> func)
        {
            var justa = a as Just<A>;
            return justa == null ?
                 Nothing<B>.Default :
                func(justa.Value);
        }


        public static IMaybe<C> SelectMany<A, B, C>(this IMaybe<A> a, Func<A, IMaybe<B>> func, Func<A, B, C> select)
        {
            return a.Bind(aval =>
                    func(aval).Bind(bval =>
                    select(aval, bval).ToMaybe()));
        }

        public static IMaybe<B> Select<A, B>(this IMaybe<A> a, Func<A, IMaybe<B>> func)
        {
            return a.Bind(func);
        }

        public static IMaybe<IEnumerable<string>> Just( IEnumerable<string> res)
        {
           return new Just<IEnumerable<string>>(res);
        }

        internal static bool IsNothing<T>(this IMaybe<T> res)
        {
            return Nothing<T>.Default.Equals(res);
        }

        internal static bool RetOrDefault<T>(this IMaybe<T> res, out T value)
        {
            var just = res as Just<T>;
            if (just == null)
            {
                value = default(T);
                return false;
            }
            value = just.Value;
            return true;
        }

        internal static T RetOrDefault<T>(this IMaybe<T> res)
        {
            var just = res as Just<T>;
            if (just == null)
            {
                 return default(T);
            }
            return just.Value;
        }

        internal static T Ret<T>(this IMaybe<T> res)
        {
            var just = res as Just<T>;
            if (just == null)
            {
                if (Nothing<T>.Default.Equals(res))
                    throw new InvalidOperationException("There is Nothing to return.");

                var exp = res as FallBack<T>;
                if (exp == null)
                    throw new NullReferenceException("unexpected or invalid argument");

                throw new Exception(string.Format("({0})", exp.ErrorCode) + exp.Description);
            }
            return just.Value;
        }

    }
}