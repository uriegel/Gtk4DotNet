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
}



