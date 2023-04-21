using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;
public class Dialog
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibGtk, EntryPoint="gtk_dialog_new_with_buttons", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string title, IntPtr parent, DialogFlags flags, string firstButton, string secondButton = null, string thirdButton = null);

    // [DllImport(Globals.LibGtk, EntryPoint="gtk_file_chooser_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr NewFileChooser(string title, IntPtr parent, FileChooserAction action, 
    //     string firstButtonText, ResponseId firstButtonId, string secondButtonText, ResponseId secondButtonId, IntPtr zero);

    // [DllImport(Globals.LibGtk, EntryPoint="gtk_dialog_run", CallingConvention = CallingConvention.Cdecl)]
    // public extern static ResponseId Run(IntPtr dialog);

    // [DllImport(Globals.LibGtk, EntryPoint="gtk_file_chooser_get_filename", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr FileChooserGetFileName(IntPtr dialog);
}

