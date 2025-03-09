using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class EventControllerFocus
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_event_controller_focus_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static EventControllerFocusHandle New();

    public static EventControllerFocusHandle OnEnter(this EventControllerFocusHandle eventControllerFocus, Action onEnter)
        => eventControllerFocus.SideEffect(ecf => Gtk.SignalConnect<TwoPointerDelegate>(ecf, "enter", 
            (nint _, nint __)  => onEnter()));
    public static EventControllerFocusHandle OnLeave(this EventControllerFocusHandle eventControllerFocus, Action onLeave)
        => eventControllerFocus.SideEffect(ecf => Gtk.SignalConnect<TwoPointerDelegate>(ecf, "leave", 
            (nint _, nint __)  => onLeave()));
}

