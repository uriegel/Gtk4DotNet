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
            var res = sel.GetItem<THandle>(pos++);
            if (res.IsInvalid)
                break;
            yield return res;
        }
    }

    /// <summary>
    /// When the selection changes, this signal is emitted.
    /// </summary>
    /// <param name="sel">SelectionHandle</param>
    /// <param name="onSelectionChanged">Callback function: parameters are: position of change, number of changed itrems</param>
    public static void OnSelectionChanged(this SelectionHandle sel, Action<uint, uint> onSelectionChanged)
        => Gtk.SignalConnect<OnSelectionChangedDelegate>(sel, "selection-changed", (_, pos, count) => onSelectionChanged(pos, count));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_select_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SelectAll(this SelectionHandle sel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_select_item", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SelectItem(this SelectionHandle sel, uint pos, bool unselectRest);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_select_range", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SelectRange(this SelectionHandle sel, uint pos, uint count, bool unselectRest);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_unselect_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool UnselectAll(this SelectionHandle sel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_unselect_item", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool UnselectItem(this SelectionHandle sel, uint pos);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_unselect_range", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool UnselectRange(this SelectionHandle sel, uint pos, uint count);

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

delegate void OnSelectionChangedDelegate(nint nil, uint pos, uint count);