using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class StringList
{
    public static ListModelHandle New(string[] strings)
    {
        var unmanagedStrings = new nint[strings.Length + 1];
        for (int i = 0; i < strings.Length; i++)
            unmanagedStrings[i] = Marshal.StringToHGlobalAnsi(strings[i]);
        unmanagedStrings[strings.Length] = 0;

        // Allocate unmanaged memory for the array itself
        var unmanagedStringsPtr = Marshal.AllocHGlobal(nint.Size * unmanagedStrings.Length);
        for (int i = 0; i < unmanagedStrings.Length; i++)
            Marshal.WriteIntPtr(unmanagedStringsPtr, i * IntPtr.Size, unmanagedStrings[i]);

        // Call GTK function
        var res = New(unmanagedStringsPtr);

        // Free unmanaged memory
        foreach (var ptr in unmanagedStrings)
            if (ptr != IntPtr.Zero) Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(unmanagedStringsPtr);
        return res;
    }

    public static void Splice(this ListModelHandle listModel, uint pos, uint removals)
        => _Splice(listModel, pos, removals, 0);

    public static void Splice(this ListModelHandle listModel, uint pos, uint removals, string[] strings)
    {
        var unmanagedStrings = new nint[strings.Length + 1];
        for (int i = 0; i < strings.Length; i++)
            unmanagedStrings[i] = Marshal.StringToHGlobalAnsi(strings[i]);
        unmanagedStrings[strings.Length] = 0;

        // Allocate unmanaged memory for the array itself
        var unmanagedStringsPtr = Marshal.AllocHGlobal(nint.Size * unmanagedStrings.Length);
        for (int i = 0; i < unmanagedStrings.Length; i++)
            Marshal.WriteIntPtr(unmanagedStringsPtr, i * IntPtr.Size, unmanagedStrings[i]);

        _Splice(listModel, pos, removals, unmanagedStringsPtr);

        // Free unmanaged memory
        foreach (var ptr in unmanagedStrings)
            if (ptr != IntPtr.Zero) Marshal.FreeHGlobal(ptr);
        Marshal.FreeHGlobal(unmanagedStringsPtr);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_string_list_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListModelHandle New(nint strings);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_string_list_splice", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Splice(ListModelHandle listModel, uint pos, uint removalCount, nint strings);
}
