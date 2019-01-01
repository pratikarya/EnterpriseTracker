using System.Collections.Generic;

namespace EnterpriseTracker.Core.Utility
{
    public static class Session
    {
        public static Dictionary<string, object> Data { get; set; }
        public static void Add(string key, object o)
        {
            Data.Add(key, o);
        }
        public static object Get(string key)
        {
            object o = null;
            Data.TryGetValue(key, out o);
            return o;
        }
    }
}
