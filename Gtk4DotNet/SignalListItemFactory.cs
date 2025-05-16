using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SignalListItemFactory
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_signal_list_item_factory_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SignalListItemFactoryHandle New();

    public static SignalData Setup(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onSetup)
        => Gtk.SignalConnect<ThreePointerDelegate>(factory, "setup", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onSetup(li);
            });

    public static SignalData Bind(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onBind)
        => Gtk.SignalConnect<ThreePointerDelegate>(factory, "bind", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onBind(li);
            });

    public static SignalData Unbind(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onUnbind)
        => Gtk.SignalConnect<ThreePointerDelegate>(factory, "unbind", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onUnbind(li);
            });

    public static SignalData TearDown(this SignalListItemFactoryHandle factory, Action<ListItemHandle> onTearDown)
        => Gtk.SignalConnect<ThreePointerDelegate>(factory, "teardown", (_, o, ___) =>
            {
                var li = new ListItemHandle();
                li.SetInternalHandle(o);
                onTearDown(li);
            });
}


