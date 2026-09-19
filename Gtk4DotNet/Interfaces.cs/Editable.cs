using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// Interface for single-line text editing widgets.
/// Typical examples of editable widgets are GtkEntry and GtkSpinButton. 
/// It contains functions for generically manipulating an editable widget, a large number of action signals used for key bindings, 
/// and several signals that an application can connect to modify the behavior of a widget.
/// </summary>
public struct Editable
{
    public string Text
    {
        get => GetText(editable).PtrToString(false) ?? "";
        set => SetText(editable, value);
    }

    public event Action<string> OnInsertText
    {
        add
        {
            InsertTextDelegate unmanagedDelegate = (_, text, length, position, __) => value(text);
            var id = editable.SignalConnectForEvent(delegat, "insert-text", unmanagedDelegate);
            Widget.eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (Widget.eventDatas.Remove(value, out var data))
                editable.SignalDisconnectEvent(data.Id);
        }
    }

    public event Action<int, int> OnDeleteText
    {
        add
        {
            DeleteTextDelegate unmanagedDelegate = (_, start, length, position) => value(start, length);
            var id = editable.SignalConnectForEvent(delegat, "delete_text", unmanagedDelegate);
            Widget.eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (Widget.eventDatas.Remove(value, out var data))
                editable.SignalDisconnectEvent(data.Id);
        }
    }

    public event Action OnChanged
    {
        add
        {
            TwoPointerDelegate unmanagedDelegate = (_, __) => value();
            var id = editable.SignalConnectForEvent("changed", unmanagedDelegate);
            Widget.eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (Widget.eventDatas.Remove(value, out var data))
                editable.SignalDisconnectEvent(data.Id);
        }
    }

    public void StopInserting() => GObject.StopSignalEmissionByName(delegat, "insert-text");

    public readonly void SelectRegion(int start, int end) => SelectRegion(editable, start, end);

    internal Editable(Widget editable)
    {
        this.editable = editable;
        delegat = GetDelegate(this.editable);
    }

    Widget editable;
    nint delegat;

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetText(Widget editable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetText(Widget editable, string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_select_region", CallingConvention = CallingConvention.Cdecl)]
    extern static void SelectRegion(Widget editable, int start, int end);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_delegate", CallingConvention = CallingConvention.Cdecl)]
    static extern nint GetDelegate(Widget widget);
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void InsertTextDelegate(nint _, string text, int length, nint position, nint __);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void DeleteTextDelegate(nint _, int __, int ___, nint ____);