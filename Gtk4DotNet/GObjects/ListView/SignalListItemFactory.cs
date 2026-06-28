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
                li.WeakCopy = true;
                onSetup(li);
            });

    public void Bind(Action<ListItem> onBind)
        => SignalConnect<ThreePointerDelegate>("bind", (_, o, ___) =>
            {
                var li = new ListItem();
                li.SetInternalHandle(o);
                li.WeakCopy = true;
                onBind(li);
                var liChild = li.GetChild<Widget>();
                if (liChild != null)
                {
                    var name = liChild?.GetName();
                    var parent = liChild?.GetParent();
                    name = parent?.GetName();
                    parent = parent?.GetParent();
                    if (parent?.GetName() == "GtkColumnViewRowWidget")
                        parent.SetManagedRawData(ListStore.DATA, li.GetRawItem());
                }
            });

    // public static SignalData Unbind(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onUnbind)
    //     => Gtk.SignalConnect<ThreePointerDelegate>(factory, "unbind", (_, o, ___) =>
    //         {
    //             var li = new ListItemHandle();
    //             li.SetInternalHandle(o);
    //             onUnbind(li);
    //         });

    // public static SignalData TearDown(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onTearDown)
    //     => Gtk.SignalConnect<ThreePointerDelegate>(factory, "teardown", (_, o, ___) =>
    //         {
    //             var li = new ListItemHandle();
    //             li.SetInternalHandle(o);
    //             onTearDown(li);
    //         });

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_signal_list_item_factory_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SignalListItemFactory _New();
}


