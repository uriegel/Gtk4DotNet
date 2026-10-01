using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MultiSelection : SelectionModel
{
    public MultiSelection(ListModel model)
    {
        var handle = New(model);
        model.AutoDestroyed = true;
        SetInternalHandle(handle);
        CheckDiagnostics();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(ListModel model);
}

