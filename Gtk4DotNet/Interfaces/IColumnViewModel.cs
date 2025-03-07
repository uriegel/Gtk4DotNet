namespace GtkDotNet;

public interface IColumnViewModel<T>
{
    void Insert(IEnumerable<T> items);
    void Insert(uint pos, IEnumerable<T> items);
    IEnumerable<T> Items();
}
