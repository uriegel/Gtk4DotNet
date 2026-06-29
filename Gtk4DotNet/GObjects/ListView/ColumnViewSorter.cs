using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ColumnViewSorter : Sorter
{
    /// <summary>
    /// First parameter is the ordering: ascending false, descending true
    /// </summary>
    public event Action<bool, SorterChange> OnChanged
    {
        add
        {
            OnChangedDelegate unmanagedDelegate = (col, sorterChanged, _) =>
            {
                bool desc = GetPrimaryOrder(col) != 0;
                value(desc, sorterChanged);
            };
            var id = SignalConnectForEvent("changed", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_sorter_get_primary_sort_order", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetPrimaryOrder(nint col);
}

delegate void OnChangedDelegate(nint _, SorterChange sorterChange, nint nil);