using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class SingleSelection : GObject
{
    public static SingleSelection New(ListStore model)
    {
        var res = _New(model);
        res.CheckDiagnostics();
        return res;
    }

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_set_selected", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetSelected(this SingleSelectionHandle ssh, int pos);
    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_get_selected", CallingConvention = CallingConvention.Cdecl)]
    // public extern static int GetSelected(this SingleSelectionHandle ssh);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SingleSelection _New(ListStore model);
}

