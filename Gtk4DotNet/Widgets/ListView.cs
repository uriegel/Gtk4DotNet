using System.Runtime.InteropServices;
using Gtk4DotNet;

public class ListView : Widget
{
    public void SetModel(SelectionModel selectionModel) => SetModel(this, selectionModel);

    public void SetFactory(ListItemFactory factory) => SetFactory(this, factory);

    public ListView() : base() { }

    public ListView(Builder builder, string? name = null) : base(builder, name) { }

    public ListView(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModel(ListView listview, SelectionModel selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_set_factory", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFactory(ListView listview, ListItemFactory factory);
}