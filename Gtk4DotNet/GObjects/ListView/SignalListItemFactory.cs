using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class SignalListItemFactory : ListItemFactory
{
    public static SignalListItemFactory New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }

    public void Setup(Action<ListItem> onSetup)
        => SignalConnect<ThreePointerDelegate>("setup", (_, o, ___) =>
            {
                var li = new ListItem();
                li.SetInternalHandle(o);
                li.AutoDestroyed = true;
                onSetup(li);
            });

    public void Bind(Action<ListItem> onBind)
        => SignalConnect<ThreePointerDelegate>("bind", (_, o, ___) =>
            {
                var li = new ListItem();
                li.SetInternalHandle(o);
                li.AutoDestroyed = true;
                onBind(li);
                var liChild = li.GetChild<Widget>();
                if (liChild != null)
                {
                    var parent = liChild?.GetParent();
                    parent = parent?.GetParent();
                    if (parent?.Name == "GtkColumnViewRowWidget")
                        parent.SetManagedRawData(ListStore.DATA, li.GetRawItem());
                }
            });

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_signal_list_item_factory_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SignalListItemFactory _New();
}


