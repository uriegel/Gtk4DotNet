using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ColumnViewSorter : Sorter
{
    /// <summary>
    /// First parameter is the ordering: ascending false, descending true
    /// </summary>
    public event Action<bool, ColumnViewColumn?, SorterChange> OnChanged
    {
        add
        {
            OnChangedDelegate unmanagedDelegate = (col, sorterChanged, _) =>
            {
                var desc = GetPrimaryOrder(col) != 0;
                var ptr = GetPrimaryColumn(col);
                var column = ptr != 0
                ? new ColumnViewColumn
                {
                    AutoDestroyed = true
                }
                : null;
                column?.SetInternalHandle(ptr);
                value(desc, column, sorterChanged);
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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_sorter_get_primary_sort_column", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetPrimaryColumn(nint col);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_sorter_get_primary_sort_order", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetPrimaryOrder(nint col);
}

delegate void OnChangedDelegate(nint _, SorterChange sorterChange, nint nil);