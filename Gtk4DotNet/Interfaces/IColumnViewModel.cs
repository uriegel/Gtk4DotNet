namespace GtkDotNet;

public interface IColumnViewModel<T>
{
    void Insert(IEnumerable<T> items);
    void Insert(uint pos, IEnumerable<T> items);
    void RemoveAll();
    IEnumerable<T> Items();
    IEnumerable<nint> RawItems();
    T? GetItem(uint pos);
}
