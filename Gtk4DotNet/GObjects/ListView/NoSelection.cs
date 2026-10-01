using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class NoSelection : SelectionModel
{
    public NoSelection(ListModel model)
    {
        var handle = New(model);
        model.AutoDestroyed = true;
        SetInternalHandle(handle);
        CheckDiagnostics();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_no_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(ListModel model);
}

