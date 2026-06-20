using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ListBoxRow : Widget
{
    public void SetHeader(Widget header) => SetHeader(this, header);

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

