using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// A single-line text entry widget
/// </summary>
public class Entry : Widget
{
    public Editable AsEditable() => new Editable(GetInternalHandle());

    public DelegateId OnActivate(Action onActivate) => SignalConnect<TwoPointerDelegate>("activate", (_, __) => onActivate());

    public Entry() : base() { }

    public Entry(Builder builder, string? name = null) : base(builder, name) { }
}
