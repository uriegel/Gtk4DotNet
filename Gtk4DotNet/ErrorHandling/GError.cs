using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

class GError
{
    public string Domain { get; }
    public int Code { get; }
    public string Message { get; }

    public static GError? Get(nint error, bool free)
    {
        if (error != 0)
        {
            var errStruct = Marshal.PtrToStructure<GErrorStruct>(error);
            var res = new GError(errStruct);
            if (free)
                Free(error);
            return res;
        }
        else
            return null;
    }

    GError(GErrorStruct errStruct)
    {
        Domain = QuarkToString(errStruct.Domain).PtrToString(false) ?? "";
        Code = errStruct.Code;
        Message = errStruct.Message.PtrToString(false) ?? "Unknown error";
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_error_free", CallingConvention = CallingConvention.Cdecl)]
    extern static void Free(nint handle);

    [DllImport(Libs.LibGtk, EntryPoint = "g_quark_to_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint QuarkToString(int quark);
}

[StructLayout(LayoutKind.Sequential)]
struct GErrorStruct
{
    public int Domain;
    public int Code;
    public nint Message;
}

