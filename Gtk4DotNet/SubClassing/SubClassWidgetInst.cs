using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassWidgetInst<THandle>(nint obj) : SubClassInst<THandle>(obj)
    where THandle : WidgetHandle, new()
{
    protected internal override void OnCreate()
        => Handle.OnRealize(_ => OnInitialize());

    protected virtual void OnInitialize() { }
}

