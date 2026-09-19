using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

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

    public event Action OnInsertText
    {
        add
        {
            InsertTextDelegate unmanagedDelegate = (_, text, length, position, __) =>
            {
                value();
            };
            
            var id = editable.SignalConnectForEvent("insert-text", unmanagedDelegate);
            Widget.eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (Widget.eventDatas.Remove(value, out var data))
                editable.SignalDisconnectEvent(data.Id);
        }
    }

    public readonly void SelectRegion(int start, int end) => SelectRegion(editable, start, end);

    internal Editable(Widget editable) => this.editable = editable;

    Widget editable;

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetText(Widget editable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetText(Widget editable, string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_select_region", CallingConvention = CallingConvention.Cdecl)]
    extern static void SelectRegion(Widget editable, int start, int end);
}

delegate void InsertTextDelegate(nint _, string text, int length, nint position, nint __);