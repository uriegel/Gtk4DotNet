using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SignalListItemFactory
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_signal_list_item_factory_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SignalListItemFactoryHandle New();

    public static SignalListItemFactoryHandle Setup(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onSetup)
        => factory.SideEffect(a => Gtk.SignalConnect<ThreePointerDelegate>(a, "setup", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onSetup(li);
            }));

    public static SignalListItemFactoryHandle Bind(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onBind)
        => factory.SideEffect(a => Gtk.SignalConnect<ThreePointerDelegate>(a, "bind", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onBind(li);
            }));

    public static SignalListItemFactoryHandle Unbind(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onUnbind)
        => factory.SideEffect(a => Gtk.SignalConnect<ThreePointerDelegate>(a, "unbind", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onUnbind(li);
            }));

    public static SignalListItemFactoryHandle TearDown(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onTearDown)
        => factory.SideEffect(a => Gtk.SignalConnect<ThreePointerDelegate>(a, "teardown", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onTearDown(li);
            }));
}


