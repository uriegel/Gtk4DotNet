using GtkDotNet.Interfaces;

namespace GtkDotNet.SafeHandles;

public class SearchEntryHandle : WidgetHandle
{
    public SearchEntryHandle() : base() {}

    public string GetText()
        => Editable.GetText(this) ?? "";

    public void SetText(string text)
        => Editable.SetText(this, text);
}



