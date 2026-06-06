namespace Gtk4DotNet;

public abstract class FloatingObject : GObject
{
    public FloatingObject() : base() => IsFloating = true;

    internal void RefSink() => IsFloating = false;
}