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

    public Revealer Child(Widget child) =>
        this.SideEffect(_ => SetChild(this, child));

    public bool IsRevealed
    {
        get => GetRevealChild(this);
        set => SetRevealChild(this, value);
    }

    public Revealer TransitionType(RevealerTransition transition) =>
        this.SideEffect(_ => SetTransitionType(this, transition));

    public Revealer RevealChild(bool reveal = true)
        => this.SideEffect(r => r.IsRevealed = reveal);

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

