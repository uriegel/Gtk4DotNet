using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Revealer
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_revealer_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static RevealerHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_revealer_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static RevealerHandle TransitionType(this RevealerHandle revealer, RevealerTransition transition)
        => revealer.SideEffect(r => r.SetTransitionType(transition));

    public static RevealerHandle Child(this RevealerHandle revealer, WidgetHandle widget)
        => revealer.SideEffect(r => r.SetChild(widget));

    public static RevealerHandle RevealChild(this RevealerHandle revealer, bool reveal = true)
        => revealer.SideEffect(r => r._RevealChild(reveal));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_revealer_set_reveal_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void _RevealChild(this RevealerHandle revealer, bool reveal);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_revealer_get_child_revealed", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsChildRevealed(this RevealerHandle revealer);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_revealer_set_transition_type", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTransitionType(this RevealerHandle revealer, RevealerTransition transition);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_revealer_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this RevealerHandle revealer, WidgetHandle widget);
}

