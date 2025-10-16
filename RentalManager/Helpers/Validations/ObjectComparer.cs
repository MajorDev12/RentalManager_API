public static class ObjectComparer
{
    public static bool HasChanges<TSource, TTarget>(
        TSource source,
        TTarget target,
        params string[] ignoreProps)
    {
        if (source == null || target == null)
            throw new ArgumentNullException("Objects to compare cannot be null.");

        var ignored = new HashSet<string>(ignoreProps, StringComparer.OrdinalIgnoreCase);

        var targetProps = typeof(TTarget).GetProperties();

        foreach (var targetProp in targetProps)
        {
            if (ignored.Contains(targetProp.Name))
                continue;

            var sourceProp = typeof(TSource).GetProperty(targetProp.Name);
            if (sourceProp == null)
                continue;

            var sourceValue = sourceProp.GetValue(source);
            var targetValue = targetProp.GetValue(target);

            if (!Equals(sourceValue, targetValue))
                return true; // Found a change
        }

        return false; // No changes detected
    }
}
