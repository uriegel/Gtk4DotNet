using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MultiSelection : SelectionModel
{
    public static MultiSelection New(ListStore model)
    {
        var res = _New(model);
        res.CheckDiagnostics();
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static MultiSelection _New(ListStore model);
}

