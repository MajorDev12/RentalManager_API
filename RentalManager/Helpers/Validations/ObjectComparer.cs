using RentalManager.Helpers.Validations;

public static class ObjectComparer
{
    // 1. default ignored fields (shared)
    private static readonly HashSet<string> DefaultIgnoredProperties =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "Id",
            "AccountId",
            "CreatedOn",
            "CreatedBy",
            "UpdatedOn",
            "UpdatedBy",
            "DeletedOn",
            "DeletedBy",
            "IsDeleted"
        };

    // 2. APPLY changes (your existing usage)
    public static bool ApplyChanges<TSource, TTarget>(
        TSource source,
        TTarget target,
        bool ignoreNulls = true,
        params string[] ignoreProps)
    {
        var changes = GetChanges(source, target, ignoreNulls, ignoreProps);

        foreach (var change in changes.Changes)
        {
            var prop = typeof(TTarget).GetProperty(change.Key);
            if (prop != null && prop.CanWrite)
                prop.SetValue(target, change.Value.NewValue);
        }

        return changes.HasChanges;
    }

    // 3. GET changes (Step 2 feature)
    public static ChangeResult GetChanges<TSource, TTarget>(
        TSource source,
        TTarget target,
        bool ignoreNulls = true,
        params string[] ignoreProps)
    {
        if (source == null || target == null)
            throw new ArgumentNullException("Source or target cannot be null.");

        var ignored = new HashSet<string>(DefaultIgnoredProperties, StringComparer.OrdinalIgnoreCase);

        if (ignoreProps != null)
        {
            foreach (var prop in ignoreProps)
                ignored.Add(prop);
        }

        var result = new ChangeResult();

        var targetProps = typeof(TTarget).GetProperties();

        foreach (var targetProp in targetProps)
        {
            if (ignored.Contains(targetProp.Name))
                continue;

            if (!targetProp.CanWrite)
                continue;

            var sourceProp = typeof(TSource).GetProperty(targetProp.Name);
            if (sourceProp == null || !sourceProp.CanRead)
                continue;

            var newValue = sourceProp.GetValue(source);
            var oldValue = targetProp.GetValue(target);

            if (ignoreNulls && newValue == null)
                continue;

            if (newValue is string ns && oldValue is string os)
            {
                var n1 = ns?.Trim();
                var n2 = os?.Trim();

                if (!string.Equals(n1, n2, StringComparison.OrdinalIgnoreCase))
                {
                    result.Changes[targetProp.Name] = (oldValue, n1);
                }

                continue;
            }

            if (!Equals(newValue, oldValue))
            {
                result.Changes[targetProp.Name] = (oldValue, newValue);
            }
        }

        return result;
    }


    public static (object oldValues, object newValues) SplitChanges(ChangeResult changes)
    {
        var oldVals = changes.Changes.ToDictionary(x => x.Key, x => x.Value.OldValue);
        var newVals = changes.Changes.ToDictionary(x => x.Key, x => x.Value.NewValue);

        return (oldVals, newVals);
    }
}