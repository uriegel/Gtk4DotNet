namespace GtkDotNet.Controls;

public class ObservableModel<T>
{
    public T[] Items { get; private set; } = [];
    public ObservableModel() { }
    public ObservableModel(IEnumerable<T> items)
        => Items = [.. items];
}
