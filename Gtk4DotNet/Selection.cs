using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Selection
{
    public static T? GetItem<T>(this SelectionHandle sel, int pos)
        where T : class
    {
        var item = sel.GetItem(pos);
        if (!item.IsInvalid)
        {
            var ptr = item.GetData(ListItem.MANAGED_OBJECT);
            var gcHandle = GCHandle.FromIntPtr(ptr);
            return gcHandle.Target as T;
        }
        else
            return null;
    }

    /// <summary>
    /// The raw GTK pointer to the object. It is ony valid until the item is included the model!
    /// </summary>
    /// <param name="sel"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static nint GetRawItem(this SelectionHandle sel, int pos)
    {
        using var item = sel.GetItem(pos);
        return item.GetInternalHandle();
    }

    public static IEnumerable<T> GetItems<T>(this SelectionHandle sel)
        where T : class
    {
        var pos = 0;
        while (true)
        {
            var res = sel.GetItem<T>(pos++);
            if (res == null)
                break;
            yield return res;
        }
    }

    public static IEnumerable<nint> GetRawItems(this SelectionHandle sel)
    {
        var pos = 0;
        while (true)
        {
            var res = sel.GetRawItem(pos++);
            if (res == 0)
                break;
            yield return res;
        }
    }

    /// <summary>
    /// When the selection changes, this signal is emitted.
    /// </summary>
    /// <param name="sel">SelectionHandle</param>
    /// <param name="onSelectionChanged">Callback function: parameters are: selection model, position of change, number of changed items</param>
    public static void OnSelectionChanged(this SelectionHandle sel, Action<nint, int, int> onSelectionChanged)
        => Gtk.SignalConnect<OnSelectionChangedDelegate>(sel, "selection-changed", (model, pos, count) => onSelectionChanged(model,pos, count));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_select_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SelectAll(this SelectionHandle sel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_select_item", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SelectItem(this SelectionHandle sel, int pos, bool unselectRest);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_select_range", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SelectRange(this SelectionHandle sel, int pos, int count, bool unselectRest);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_unselect_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool UnselectAll(this SelectionHandle sel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_unselect_item", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool UnselectItem(this SelectionHandle sel, int pos);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_unselect_range", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool UnselectRange(this SelectionHandle sel, int pos, int count);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_unselect_range", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool UnselectRange(this nint sel, int pos, int count);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_selection_model_is_selected", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsSelected(this SelectionHandle sel, int pos);
    
    /// <summary>
    /// Gets the number of items in list.
    /// Depending on the model implementation, calling this function may be less efficient than iterating the list with GetItems().
    /// </summary>
    /// <param name="sel"></param>
    /// <returns></returns>
    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_n_items", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetItemCount(this SelectionHandle sel);

    /// <summary>
    /// The caller of the method takes ownership of the returned data, and is responsible for freeing it.
    /// The return value can be NULL.
    /// </summary>
    /// <param name="sel"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static ObjectHandle GetItem(this SelectionHandle sel, int pos);
}

delegate void OnSelectionChangedDelegate(nint nil, int pos, int count);