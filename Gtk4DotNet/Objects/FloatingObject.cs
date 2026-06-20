namespace Gtk4DotNet;

public abstract class FloatingObject : GObject
{
    public FloatingObject() : base() => AutoDestroyed = true;

    internal void RefSink() => AutoDestroyed = false;
}