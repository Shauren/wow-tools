namespace UpdateFieldCodeGenerator
{
    public static class DictionaryExtensions
    {
        public static TValue ComputeIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> func)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            value = func(key);
            dictionary[key] = value;
            return value;
        }
    }
}
