using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Button
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static ButtonHandle NewWithLabel(string label);

    public static ButtonHandle OnClicked(this ButtonHandle button, Action click)
    {
        void onClick(IntPtr _, IntPtr __)  => click();
        return button.SideEffect(a => Gtk.SignalConnect(a, "clicked", Marshal.GetFunctionPointerForDelegate((TwoPointerDelegate)onClick), IntPtr.Zero, IntPtr.Zero, 0));
    }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetLabel(this IntPtr button, string label);

    public static string? GetLabel(this IntPtr button)
    {
        var ptr = _GetLabel(button);
        var result = Marshal.PtrToStringUTF8(ptr);
        return result;
    }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_label", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetLabel(IntPtr button);
}

