using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AppInfo
{
    public static AppInfoHandle[] GetAllApps()
    {
        var list = GetALl();
        var result = GetItems().ToArray();
        GList.Free(list);
        return result;

        IEnumerable<AppInfoHandle> GetItems()
        {
            var current = list;
            while (current != 0)
            {
                var node = Marshal.PtrToStructure<GList>(current);
                var app = node.Data;
                yield return new AppInfoHandle(app);

                // IntPtr exePtr = GLib.g_app_info_get_executable(app);
                // if (exePtr != IntPtr.Zero)
                // {
                //     string exe = PtrToStringUtf8(exePtr);
                //     string name = PtrToStringUtf8(GLib.g_app_info_get_name(app));

                //     result.Add((name, exe, app));
                // }

                current = node.Next;
            }        
        }
    }
    
    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint GetALl();
}