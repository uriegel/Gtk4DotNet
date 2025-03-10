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

    public static IEnumerable<THandle> GetItems<THandle>(this SelectionHandle sel)
        where THandle : ObjectHandle, new()
    {
        uint pos = 0;
        while (true)
        {
            var res = sel.GetItem<THandle>(pos);
            if (res.IsInvalid)
                break;
            yield return res;
        }
    }

    /// <summary>
    /// Gets the number of items in list.
    /// Depending on the model implementation, calling this function may be less efficient than iterating the list with GetItems().
    /// </summary>
    /// <param name="sel"></param>
    /// <returns></returns>
    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_n_items", CallingConvention = CallingConvention.Cdecl)]
    public extern static uint GetItemCount(this SelectionHandle sel);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetItem(this SelectionHandle sel, uint pos);
}