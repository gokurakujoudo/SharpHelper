using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SharpHelper.Object {
    public class SharpCache : SharpObject {
        public static readonly SharpCache Default = new SharpCache();

        private readonly Dictionary<string, SharpObject> _dict;

        public SharpCache() {
            _dict = new Dictionary<string, SharpObject>();
            IsListen = true;
        }

        public ObservableCollection<KeyValuePair<string, SharpObject>> MemberList =>
            new ObservableCollection<KeyValuePair<string, SharpObject>>(_dict);

        public SharpObject GetMember(string name) => _dict.TryGetValue(name, out var mem) ? mem : null;

        public bool AddMember(string name, SharpObject obj) {
            if (_dict.ContainsKey(name)) return false;
            AddOrOverlap(name, obj);
            return true;
        }

        public void AddOrOverlap(string name, SharpObject obj) {
            obj.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(this.MemberList));
            _dict[name] = obj;
        }

        public bool RemoveMember(string name) {
            if (!_dict.ContainsKey(name)) return false;
            _dict.Remove(name);
            return true;
        }

        public object GetMemberProperty(string objName, string propName) {
            try {
                var so = GetMember(objName);
                return so?.Get(propName);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool SetMemberProperty(string objName, string propName, object value) {
            try {
                var so = GetMember(objName);
                return so?.Set(propName, value) ?? false;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        public object InvokeMember(string objName, string funcName, params object[] paras) {
            try {
                var so = GetMember(objName);
                return so?.Invoke(funcName, paras);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
