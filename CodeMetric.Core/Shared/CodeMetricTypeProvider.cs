using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMetric.Core.Shared
{
    public static class CodeMetricTypeProvider
    {
        private static readonly Dictionary<Type, object> CachedObjectInstance = new Dictionary<Type, object>();

        public static T Get<T>()
        {
            object o;
            if(CachedObjectInstance.TryGetValue(typeof(T), out o))
            {
                return (T)o;
            }

            var type = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .Where(i => i.GetName().Name == "CodeMetric.Core")
                                .ToList()
                                .Single()
                                .ExportedTypes.First(t => typeof(T).IsAssignableFrom(t) && t.IsClass);

            o = Activator.CreateInstance(type);
            CachedObjectInstance.Add(typeof(T), 0);
            return (T)o;
        }

        public static T Add<T>(Type key, object oNew)
        {
            object oExist;
            if(CachedObjectInstance.TryGetValue(key, out oExist))
            {
                return (T)oExist;
            }

            CachedObjectInstance.Add(key, oNew);
            return (T)oNew;
        }
    }
}
