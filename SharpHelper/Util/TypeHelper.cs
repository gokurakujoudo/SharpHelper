using System;
using System.Linq;

namespace SharpHelper.Util {
    public static class TypeHelper {
        public static bool IsImplement<T>(this Type t) => t.GetInterfaces().Any(i => i == typeof(T));

        public static object Get(this object o, string property, bool caseSensitive = false) {
            var prop = o.GetPropertyInfo(property, caseSensitive);
            if (prop is null) throw new NullReferenceException($@"{o.GetType()} doesn't have property {property}");
            return prop.GetValue(o);
        }

        public static void Set(this object o, string property, object value, bool caseSensitive = false) {
            var prop = o.GetPropertyInfo(property, caseSensitive);
            if (prop is null) throw new NullReferenceException($@"{o.GetType()} doesn't have property {property}");
            prop.SetValue(o, value);
        }

        public static Func<object> Getter(this object o, string property, bool caseSensitive = false) {
            var prop = o.GetPropertyInfo(property, caseSensitive);
            if (prop is null) throw new NullReferenceException($@"{o.GetType()} doesn't have property {property}");
            return () => prop.GetValue(o);
        }

        public static Action<object> Setter(this object o, string property, bool caseSensitive = false)
        {
            var prop = o.GetPropertyInfo(property, caseSensitive);
            if (prop is null) throw new NullReferenceException($@"{o.GetType()} doesn't have property {property}");
            return value => prop.SetValue(o, value);
        }

        public static Func<object, object> AsFunc(this object o, string propIn, string propOut,
                                                  bool caseSensitive = false) {
            var pIn = o.GetPropertyInfo(propIn, caseSensitive);
            if (pIn is null) throw new NullReferenceException($@"{o.GetType()} doesn't have property {propIn}");
            var pOut = o.GetPropertyInfo(propOut, caseSensitive);
            if (pOut is null) throw new NullReferenceException($@"{o.GetType()} doesn't have property {propOut}");
            return value => {
                pIn.SetValue(o, value);
                return pOut.GetValue(o);
            };
        }

    }
}
