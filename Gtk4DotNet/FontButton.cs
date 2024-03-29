using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class FontButton
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_font_button_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static FontButtonHandle New();
}
