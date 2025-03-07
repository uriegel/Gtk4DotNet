using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Selection
{
    public static THandle GetItem<THandle>(this SelectionHandle sel, uint pos)
        where THandle : ObjectHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(sel.GetItem(pos));
        res.IsFloating = true;
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetItem(this SelectionHandle sel, uint pos);
}