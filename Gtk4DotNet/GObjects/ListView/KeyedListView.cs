namespace Gtk4DotNet;

public class KeyedListStore<T, TKey> : ListStore<T>
    where T : class
    where TKey : notnull
{
    public KeyedListStore(Func<T, TKey> selector) => this.selector = selector;

    public override ListStore<T> Append(T t)
    {
        var key = selector(t);
        dictionary.TryAdd(key, t);
        base.Append(t);
        return this;
    }

    public void Delete(TKey key)
    {
        if (dictionary.TryGetValue(key, out var t))
        {
            dictionary.Remove(key);

            var idx = 0;
            foreach (var item in GetItems<T>())
            {               
                if (ReferenceEquals(item, t))
                    break;
                idx++;
            }
            base.Remove(idx);
        }
    }

    public void ReplaceAll(IEnumerable<T> objs)
    {
        dictionary = objs.ToDictionary(n => selector(n));
        RemoveAll();
        base.Splice(0, 0, objs);
    }

    public T? GetValue(TKey key) => dictionary.TryGetValue(key, out var ret) ? ret : null;

    public new void Splice(int pos, int removals, IEnumerable<T> objs) => throw new NotImplementedException();

    public new void Remove(int position) => throw new NotImplementedException();

    public new void RemoveItems(int pos, int removals) => throw new NotImplementedException();

    readonly Func<T, TKey> selector;
    Dictionary<TKey, T> dictionary = [];
}