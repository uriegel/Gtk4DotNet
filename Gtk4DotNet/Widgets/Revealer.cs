using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class Revealer : Widget
{
    public static Revealer New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }
    
    public Revealer() : base() { }

    public Revealer(Builder builder, string? name = null) : base(builder, name) { }

    public Revealer(nint obj) : base() => SetInternalHandle(obj);

    public void SetChild(Widget child) => SetChild(this, child);

    public bool IsRevealed
    {
        get => GetRevealChild(this);
        set => SetRevealChild(this, value);
    }

    public void SetTransitionType(RevealerTransition transition) => SetTransitionType(this, transition);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_revealer_set_reveal_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetRevealChild(Revealer revealer, bool reveal);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_revealer_get_child_revealed", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetRevealChild(Revealer revealer);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_revealer_set_transition_type", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTransitionType(Revealer revealer, RevealerTransition transition);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_revealer_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(Revealer revealer, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_revealer_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Revealer _New();
}

public static class RevealerExtensions
{
    public static Revealer TransitionType(this Revealer revealer, RevealerTransition transition)
        => revealer.SideEffect(r => r.SetTransitionType(transition));

    public static Revealer Child(this Revealer revealer, Widget widget)
        => revealer.SideEffect(r => r.SetChild(widget));

    public static Revealer RevealChild(this Revealer revealer, bool reveal = true)
        => revealer.SideEffect(r => r.IsRevealed = reveal);
        
}
