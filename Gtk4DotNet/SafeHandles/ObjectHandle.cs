using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class ObjectHandle : BaseHandle
{
    public ObjectHandle() : base() { }

    public bool IsFloating { get; set; }

    protected override bool ReleaseHandle()
        => IsFloating
            || true.SideEffectIf(!IsFloating, _ => GObject.Unref(handle));
}