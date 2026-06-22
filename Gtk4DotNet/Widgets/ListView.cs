using System.Runtime.InteropServices;
using Gtk4DotNet;

public class ListView : Widget
{
    public void SetModel(SingleSelection selectionModel) => SetModel(this, selectionModel);

    public void SetFactory(SignalListItemFactory factory) => SetFactory(this, factory);

    public ListView() : base() { }

    public ListView(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModel(ListView listview, SingleSelection selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_set_factory", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFactory(ListView listview, SignalListItemFactory factory);
}