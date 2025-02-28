namespace GtkDotNet.SafeHandles;

public abstract class ObjectFloatingHandle : ObjectHandle
{
    public ObjectFloatingHandle() : base() => IsFloating = true;

    internal void RefSink() => IsFloating = false;
}