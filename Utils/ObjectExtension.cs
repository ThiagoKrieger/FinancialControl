using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Utils;

public static class ObjectExtension
{
    public static TDestination ProjectToNew<TDestination>(this object source)
        where TDestination : class, new()
    {
        var target = new TDestination();
        source.ProjectTo(target);
        return target;
    }

    public static void ProjectTo(this object? source, object target)
    {
        if (source == null)
            return;

        if (target == null)
            throw new ArgumentNullException(nameof(target));

        var sourceProperties = ReflectionUtils.GetPocoProperties(source.GetType());
        var targetProperties = ReflectionUtils.GetPocoProperties(target.GetType());

        foreach (var (name, sourceProperty) in sourceProperties)
        {
            if (!targetProperties.TryGetValue(name, out var targetProperty))
                continue;

            var sourceValue = sourceProperty.GetValue(source, null);
            var targetValue = targetProperty.GetValue(target, null);

            if (sourceValue != targetValue)
                targetProperty.SetValue(target, sourceValue, null);
        }
    }
}