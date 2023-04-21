using System;
using System.Runtime.InteropServices;

namespace GtkDotNet
{
    public class CssProvider
    {
        [DllImport(Globals.LibGtk, EntryPoint = "gtk_css_provider_new", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr New();

        [DllImport(Globals.LibGtk, EntryPoint = "gtk_css_provider_load_from_resource", CallingConvention = CallingConvention.Cdecl)]
        public extern static void LoadFromResource(IntPtr handle, string path);
    }
}
