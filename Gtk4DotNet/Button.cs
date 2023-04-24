using System;
using System.Runtime.InteropServices;

namespace GtkDotNet
{
    public static class Button
    {
        [DllImport(Globals.LibGtk, EntryPoint="gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr NewWithLabel(string label);


        [DllImport(Globals.LibGtk, EntryPoint="gtk_button_set_label", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetLabel(this IntPtr button, string label);

        public static string GetLabel(this IntPtr button)
        {
            var ptr = _GetLabel(button);
            var result = Marshal.PtrToStringUTF8(ptr);
            return result;
        }

        [DllImport(Globals.LibGtk, EntryPoint="gtk_button_get_label", CallingConvention = CallingConvention.Cdecl)]
        extern static IntPtr _GetLabel(IntPtr button);
    }
}
