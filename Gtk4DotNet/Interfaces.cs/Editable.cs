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
    public string GetText() => GetText(editable).PtrToString(false) ?? "";

    internal Editable(nint editable) => this.editable = editable;

    nint editable;

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetText(nint editable);
}

