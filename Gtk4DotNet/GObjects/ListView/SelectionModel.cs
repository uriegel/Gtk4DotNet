using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class SelectionModel : ListModel
{
    public bool UnselectAll() => UnselectAll(this);

    internal SelectionModel() : base() { }
    
    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_selection_model_select_all")]
    static extern bool SelectAll(SelectionModel model);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_selection_model_unselect_all")]
    static extern bool UnselectAll(SelectionModel model);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_selection_model_select_item")]
    static extern bool SelectItem(SelectionModel model, int pos, bool unselectRest);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_selection_model_select_range")]
    static extern bool SelectRange(SelectionModel model, int pos, int count, bool unselectRest);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_selection_model_unselect_item")]
    static extern bool UnselectItem(SelectionModel model, int pos);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_selection_model_unselect_range")]
    static extern bool UnselectRange(SelectionModel model, int pos, int count);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_selection_model_is_selected")]
    static extern bool IsSelected(SelectionModel model, int pos);
}