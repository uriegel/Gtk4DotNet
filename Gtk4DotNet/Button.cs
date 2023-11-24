using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Button
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static ButtonHandle NewWithLabel(string label);

    public static ButtonHandle Label(this ButtonHandle button, string label)
        => button.SideEffect(b => b.SetLabel(label));

    public static ButtonHandle OnClicked(this ButtonHandle button, Action click)
        => button.SideEffect(a => Gtk.SignalConnect<TwoPointerDelegate>(a, "clicked", (IntPtr _, IntPtr __)  => click()));

    public static string? GetLabel(this ButtonHandle button)
    {
        var ptr = _GetLabel(button);
        var result = Marshal.PtrToStringUTF8(ptr);
        return result;
    }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_label", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetLabel(ButtonHandle button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLabel(this ButtonHandle button, string label);
}

