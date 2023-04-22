using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

// TODO Gtk3
public class DragDrop
{
    [Flags]
    public enum DefaultDestination
    {
        Motion = 1,
        Highlight = 2,
        Drop = 4
    }

    [Flags]
    public enum DragActions
    {
        Default = 1,
        Copy = 2,
        Move = 4,
        Link = 8,
        Private = 0x10,
        Ask = 0x20
    }

    [Flags]
    public enum ModifierType
    {
        ShiftMask = 1,
        LockMask = 2,
        ControlMask = 4,
        Mod1Mask = 8,
        Mod2Mask = 0x10,
        Mod3Mask = 0x20,
        Mod4Mask = 0x40,
        Mod5Mask = 0x80,
        Button1Mask = 0x100,
        Button2Mask = 0x200,
        Button3Mask = 0x400,
        Button4Mask = 0x800,
        Button5Mask = 0x1000,

    }

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_drag_dest_set", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetDestination(IntPtr widget, DefaultDestination defaultDestination, IntPtr targets, int targetCount, DragActions actions);
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_drag_dest_unset", CallingConvention = CallingConvention.Cdecl)]
    public extern static void UnSet(IntPtr widget);
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_drag_source_set", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSource(IntPtr widget, DefaultDestination defaultDestination, IntPtr targets, int targetCount, DragActions actions);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_drag_begin_with_coordinates", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Begin(IntPtr widget, IntPtr targetList, DragActions actions, int button, IntPtr zero, int x, int y);
}

