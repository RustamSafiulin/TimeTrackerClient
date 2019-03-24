
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace TimeTracker.Helpers
{
    public class Memento<T> : IDisposable
    {
        private Dictionary<PropertyInfo, object> _dumpedProperties;
        private static readonly List<String> _propertiesToIgnore = new List<String> { "Memento", "IsDirty", "IsTrackable" };

        public void SaveOrUpdate(Object originator)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(p => _propertiesToIgnore.IndexOf(p.Name) == -1)
                                                    .Where(p => p.CanRead && p.CanWrite);
            foreach (var prop in properties)
            {
                _dumpedProperties[prop] = prop.GetValue((T)originator, null);
            }
        }

        public void Restore(Object originator)
        {
            foreach (var prop in _dumpedProperties.Keys.ToList())
            {
                prop.SetValue((T)originator, _dumpedProperties[prop], null);
            }
        }

        public void Dispose()
        {
            _dumpedProperties.Clear();
        }

        public Memento()
        {
            _dumpedProperties = new Dictionary<PropertyInfo, object>();
        }
    }
}