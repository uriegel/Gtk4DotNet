namespace GtkDotNet;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
struct GErrorStruct  
{
    public uint Domain;
    public int Code;
    public string Message;

    internal GErrorStruct(uint domain, int code, string message)
    {
        Domain = domain;
        Code = code;
        Message = message;
    }

    internal GErrorStruct(nint error)
    {
        if (error != 0)
        {
            this = Marshal.PtrToStructure<GErrorStruct>(error);
            Free(error);
        }
        else
        {
            Domain = 0;
            Code = 0;
            Message = "";
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_error_free", CallingConvention = CallingConvention.Cdecl)]
    extern static void Free(nint handle);
}

