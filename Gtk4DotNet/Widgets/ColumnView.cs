using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet;

public class ColumnView : Widget
{
    public void SetModel(SelectionModel selectionModel) => SetModel(this, selectionModel);

    public void AppendColumn(ColumnViewColumn column) => AppendColumn(this, column);

    public ColumnView(Builder builder, string? name = null) : base(builder, name) { }


    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModel(ColumnView columnView, SelectionModel selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_append_column", CallingConvention = CallingConvention.Cdecl)]
    extern static void AppendColumn(ColumnView columnView, ColumnViewColumn column);
}
