using GtkDotNet.SafeHandles;

namespace GtkDotNet.Controls;

public class ColumnViewControlColumn<T>
{
    public string Title { get; set; } = string.Empty;
    public bool Expanded { get; set; }
    public bool Resizeable { get; set; }
    public Func<WidgetHandle> OnItemSetup { get; set; } = () => Label.New("").HAlign(Align.Start);
    public Action<ListItemHandle, T>? OnItemBind { get; set; }
    public Func<T, string>? OnLabelBind { get; set; } 
    public Func<T, T, int>? OnSort { get; set; } 
}


