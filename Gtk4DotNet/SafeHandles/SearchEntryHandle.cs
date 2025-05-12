using GtkDotNet.Interfaces;

namespace GtkDotNet.SafeHandles;

public class SearchEntryHandle : WidgetHandle
{
    public SearchEntryHandle() : base() {}
    public SearchEntryHandle(nint obj) : base() => SetInternalHandle(obj);

    public string GetText()
        => Editable.GetText(this) ?? "";

    public void SetText(string text)
        => Editable.SetText(this, text);

    internal SearchEntryHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class SearchEntryHandleExtensions
{
    public static SearchEntryHandle DownCastSearchEntry(this WidgetHandle widget) => new(widget);
}




