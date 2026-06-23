using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public abstract class ListModel : GObject
{
    public IEnumerable<T> GetItems<T>() 
        where T : class
    {
        int i = 0;
        while(true)
        {
            var item = GetItem<T>(i++);
            if (item == null)
                yield break;
            yield return item;
        }
    }
    
    public T? GetItem<T>(int position) where T : class
    {
        using var obj = GetItem(this, position);
        if (obj.IsInvalid)
            return null;
        return obj.GetManagedData<T>(ListStore.DATA);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static GObject GetItem(ListModel model, int position);
}
