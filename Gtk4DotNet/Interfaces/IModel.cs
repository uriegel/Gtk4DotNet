namespace GtkDotNet;

public interface IColumnViewModel<T>
{
    public void Insert(IEnumerable<T> items);
}
