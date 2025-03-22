using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public interface IColumnViewModel<T>
{
    void Insert(IEnumerable<T> items);
    void Insert(int pos, IEnumerable<T> items);
    void RemoveAll();
    IEnumerable<T> Items();
    IEnumerable<nint> RawItems();
    T? GetItem(int pos);
}
