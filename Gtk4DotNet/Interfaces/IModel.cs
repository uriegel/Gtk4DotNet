namespace GtkDotNet;

public interface IModel<T>
{
    public void Insert(IEnumerable<T> items);
}
