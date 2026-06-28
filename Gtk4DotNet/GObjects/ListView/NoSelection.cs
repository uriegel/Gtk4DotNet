using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class NoSelection : SelectionModel
{
    public static NoSelection New(ListModel model)
    {
        var res = _New(model);
        model.AutoDestroyed = true;
        res.CheckDiagnostics();
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_no_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static NoSelection _New(ListModel model);
}

