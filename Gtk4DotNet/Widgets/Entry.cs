using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// A single-line text entry widget
/// </summary>
public class Entry : Widget
{
    public Entry()
    {
        SetInternalHandle(New());
        CheckDiagnostics();
    }

    public Editable AsEditable() => new(this);

    public event Action OnActivate
    {
        add
        {
            TwoPointerDelegate unmanagedDelegate = (_, __) => value();
            var id = SignalConnectForEvent("activate", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    public Entry(Builder builder, string? name = null) : base(builder, name) { }

    public Entry(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_entry_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();
}
