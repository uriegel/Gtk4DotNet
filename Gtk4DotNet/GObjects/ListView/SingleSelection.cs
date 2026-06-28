using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class SingleSelection : SelectionModel
{
    public int Selected
    {
        get => GetSelected(this);
        set => SetSelected(this, value);
    }
    public static SingleSelection New(ListModel model)
    {
        var res = _New(model);
        model.WeakCopy = true;
        res.CheckDiagnostics();
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_set_selected", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSelected(SingleSelection sel, int pos);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_get_selected", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetSelected(SingleSelection sel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SingleSelection _New(ListModel model);
}

