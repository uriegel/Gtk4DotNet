using System.Runtime.InteropServices;
using Gtk4DotNet;

public static class Quark
{
    public static int Diagnostics { get; } = Get("__QDataDiagnostics__");
    public static int ListData { get; } = Get("__QDataListData__");

    [DllImport(Libs.LibGtk, EntryPoint = "g_quark_from_string", CallingConvention = CallingConvention.Cdecl)]
    public extern static int Get(string quark);
}