using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class EditableLabel
{
    public static EditableLabelHandle New() => New(0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static EditableLabelHandle New(string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_editable_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static EditableLabelHandle New(nint _);
}