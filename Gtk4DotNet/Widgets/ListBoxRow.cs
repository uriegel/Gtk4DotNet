using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// The kind of widget that can be added to a GtkListBox.
/// <see cref="ListBox"/> will automatically wrap its children in a <see cref="ListBoxRow"/> when necessary.
/// </summary>
public class ListBoxRow : Widget
{
    /// <summary>
    /// Sets the current header of the row.
    /// This is only allowed to be called from the callback of <see cref="ListBox.SetHeaderFunc(Action{ListBoxRow, ListBoxRow})"/>. It will replace any existing header in the row, and be shown in front of the row in the listbox.
    /// </summary>
    /// <param name="header"></param>
    public void SetHeader(Widget header) => SetHeader(this, header);

    /// <summary>
    /// Wraps the child handle of this ListBoxItem in a TWidget class to access the Gtk data./>
    /// </summary>
    /// <typeparam name="TWidget"></typeparam>
    /// <returns></returns>
    public TWidget? GetChild<TWidget>() where TWidget : Widget
        => GetRegistered<TWidget>(RowGetWidgetKey(handle));

    public ListBoxRow() : base() { }

    internal ListBoxRow(nint raw) : base()
    {
        SetInternalHandle(raw);
        CheckDiagnostics();
    }

    static nint RowGetWidgetKey(nint row) => row != 0 ? _RowGetWidgetKey(row) : 0;

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_row_set_header", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHeader(ListBoxRow row, Widget header);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_row_get_child", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _RowGetWidgetKey(nint row);
}

