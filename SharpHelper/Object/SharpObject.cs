using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using SharpHelper.Util;

namespace SharpHelper.Object {
    [Serializable]
    public abstract class SharpObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlIgnore] public bool IsListen = false;
        [XmlIgnore] public readonly Dictionary<string, PropertyInfo> Properties;
        [XmlIgnore] public readonly Dictionary<string, List<MethodInfo>> Methods;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (IsListen)
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected SharpObject() {
            var type = GetType();
            Properties = type.GetProperties().ToDictionary(p => p.Name.ToUpper(), p => p);
            Methods = type.GetMethods().GroupBy(f => f.Name.ToUpper()).ToDictionary(g => g.Key, g => g.ToList());
        }
        
        public object Get(string property) {
            try
            {
                var prop = Properties[property.ToUpper()];
                return prop.GetValue(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool Set(string property, object value) {
            try
            {
                var prop = Properties[property.ToUpper()];
                prop.SetValue(this, value);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public Func<object> Getter(string property) {
            try {
                var prop = Properties[property.ToUpper()];
                return () => prop.GetValue(this);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }

        public Action<object> Setter(string property) {
            try {
                var prop = Properties[property.ToUpper()];
                return value => prop.SetValue(this, value);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }

        public object Invoke(string funcName, params object[] paras) {
            try {
                var func = Methods[funcName.ToUpper()].First();
                return func?.Invoke(this, paras);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }

        public object InvokeByDict(string funcName, Dictionary<string, object> paraDict) {
            try {
                var funcs = Methods[funcName.ToUpper()];
                if (paraDict is null) paraDict = new Dictionary<string, object>();
                var inputNames = paraDict.Keys;
                var func = funcs.MaxItem(f => inputNames.Intersect(
                                                            f.GetParameters().Where(p => !p.HasDefaultValue)
                                                             .Select(p => p.Name.ToUpper()))
                                                        .Count());
                return func.InvokeByDict(paraDict, this);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }

        public static object ConstructByDict(Type type, Dictionary<string, object> paraDict) {
            try {
                var cons = type.GetConstructors();
                var inputNames = paraDict.Keys;
                var con = cons.MaxItem(f => inputNames.Intersect(
                                                          f.GetParameters().Where(p => !p.HasDefaultValue)
                                                           .Select(p => p.Name.ToUpper()))
                                                      .Count());
                var ps = con.GetParameters();
                var paras = new object[ps.Length];
                for (var i = 0; i < ps.Length; i++)
                    paras[i] = paraDict.GetValue(ps[i].Name.ToUpper(), Type.Missing);
                return con.Invoke(paras);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }


        public override string ToString() => 
           $@"{GetType().Name}[{Properties.Values.Select(p => $@"{p.Name}: {p.GetValue(this)}").JoinStr(" ")}]";
    }
}
