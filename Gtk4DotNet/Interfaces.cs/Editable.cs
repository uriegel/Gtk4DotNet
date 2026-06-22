using System.Runtime.InteropServices;
using Gtk4DotNet;
using Gtk4DotNet.Extensions;

public interface Editable
{
    string GetText() => GetHandle().GetText().PtrToString(false) ?? "";
    
    public nint GetHandle();
}

static class EditablePinvokes
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint GetText(this nint editable);
}