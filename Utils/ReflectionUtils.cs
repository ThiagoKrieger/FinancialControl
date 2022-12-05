using System.Collections.Concurrent;
using System.Reflection;

namespace Utils;

public static class ReflectionUtils
{
    private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>
        PocoPropertiesCache = new ();

    private static Dictionary<string, PropertyInfo> LoadProperties(Type entityType) => entityType
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.CanRead && p.CanWrite)
        .ToDictionary(p => p.Name);

    public static Dictionary<string, PropertyInfo> GetPocoProperties(Type entityType)
    {
        return PocoPropertiesCache.GetOrAdd(entityType, LoadProperties);
    }
}