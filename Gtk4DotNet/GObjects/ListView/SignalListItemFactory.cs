using System.Runtime.InteropServices;
using CsTools.Extensions;
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

    public SignalListItemFactory Setup(Action<ListItem> onSetup)
        => this.SideEffect(_ => SignalConnect<ThreePointerDelegate>("setup", (_, o, ___) =>
            {
                var li = new ListItem();
                li.SetInternalHandle(o);
                li.AutoDestroyed = true;
                onSetup(li);
            }));

    public SignalListItemFactory Bind(Action<ListItem> onBind)
        => this.SideEffect(_ => SignalConnect<ThreePointerDelegate>("bind", (_, o, ___) =>
            {
                var li = new ListItem();
                li.SetInternalHandle(o);
                li.AutoDestroyed = true;
                onBind(li);
                var liChild = li.GetChild<Widget>();
                if (liChild != null)
                {
                    var parent = liChild.GetParent();
                    parent = parent?.GetParent();
                    if (parent?.WidgetName == "GtkColumnViewRowWidget")
                        parent.SetManagedRawData(Quark.ListData, li.GetRawItem());
                }
            }));

    public SignalListItemFactory Unbind(Action<ListItem> onUnbind)
        => this.SideEffect(_ => SignalConnect<ThreePointerDelegate>("unbind", (_, o, ___) =>
            {
                var li = new ListItem();
                li.SetInternalHandle(o);
                li.AutoDestroyed = true;
                onUnbind(li);
            }));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_signal_list_item_factory_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SignalListItemFactory _New();
}


