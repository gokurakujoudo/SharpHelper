using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharpHelper.Util
{
    public static class ReflectionHelper
    {
        public static PropertyInfo GetPropertyInfo(this object o, string property, bool caseSensitive = false) {
            var prop = caseSensitive
                ? o.GetType().GetProperty(property)
                : o.GetType().GetProperties()
                   .First(p => string.Equals(p.Name, property, StringComparison.CurrentCultureIgnoreCase));
            return prop;
        }

        public static bool HasAttribute<T>(this MemberInfo member) where T : Attribute {
            var attribute = member.GetCustomAttribute<T>();
            return !(attribute is null);
        }


        public static IEnumerable<Type> FindChildClass(this Type parent) {
            var cs = FindClasses().Where(t => t.IsSubclassOf(parent));
            return cs;
        }

        public static IEnumerable<Type> FindClasses() {
            return AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(ass => ass.GetTypes());
        }

        public static object InvokeByDict(this MethodInfo func, Dictionary<string, object> paraDict, object by = null) {
            if (paraDict is null) paraDict = new Dictionary<string, object>();
            var ps = func.GetParameters();
            var paras = new object[ps.Length];
            for (var i = 0; i < ps.Length; i++)
                paras[i] = paraDict.GetValue(ps[i].Name.ToUpper(), Type.Missing);
            return func.Invoke(by, paras);
        }

        public static List<string> GetParaNames(this MethodInfo func) {
            var ps = func.GetParameters();
            return ps.Select(p => p.Name.ToUpper()).ToList();
        }

        public static List<string> GetParaNames(this ConstructorInfo con)
        {
            var ps = con.GetParameters();
            return ps.Select(p => p.Name.ToUpper()).ToList();
        }
    }
}
