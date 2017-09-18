using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHelper.Object
{
    public static class SharpCacheDict {
        public const string DEFAULT_CACHE = "default";

        public static readonly Dictionary<string, SharpCache> Dict =
            new Dictionary<string, SharpCache> {{DEFAULT_CACHE, SharpCache.Default}};

        public static SharpCache GetCache(string cacheId) {
            try {
                return Dict[cacheId];
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }

        public static SharpObject GetMember(string name, string cacheId = DEFAULT_CACHE) =>
            GetCache(cacheId)?.GetMember(name);

    }
}
