using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AppInfo
{
    public static DisposableEnumerable<AppInfoHandle> GetAllApps()
    {
        var list = GetALl();
        var result = list.GetItems()
            .ToArray()
            .AsDisposable();

        GList.Free(list);
        return result;
    }

    public static DisposableEnumerable<AppInfoHandle> GetRecommendedApps(string contentType)
    {
        var list = GetRecommended(contentType);
        var result = list.GetItems()
            .ToArray()
            .AsDisposable();

        GList.Free(list);
        return result;
    }

    public static string? GetName(this AppInfoHandle appInfo)
        => appInfo._GetName().PtrToString(false);

    public static string? GetExecutable(this AppInfoHandle appInfo)
        => appInfo._GetExecutable().PtrToString(false);

    public static AppIcon? GetIcon(this AppInfoHandle appInfo)
    {
        var icon = appInfo._GetIcon();
        if (icon == 0)
            return null;

        var file = GetIconFile(icon);
        if (file != 0)
        {
            var iconPath = file.GetIconPath().PtrToString(true);
            if (iconPath != null)
                return new(iconPath, true);
        }
        var names = getIconNames(icon);
        if (names == 0)
            return null;
        var firstName = Marshal.ReadIntPtr(names);
        if (firstName == 0)
            return null;
        var iconName = firstName.PtrToString(false);
        if (iconName != null)
            return new(iconName, false);
        
        return null;
    }

    static IEnumerable<AppInfoHandle> GetItems(this nint list)
    {
        var current = list;
        while (current != 0)
        {
            var node = Marshal.PtrToStructure<GList>(current);
            var app = node.Data;
            yield return new AppInfoHandle(app);
            current = node.Next;
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_all", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetALl();
    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_recommended_for_type", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetRecommended(string contentType);
    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(this AppInfoHandle appInfo);
    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_executable", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetExecutable(this AppInfoHandle appInfo);
    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_icon", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetIcon(this AppInfoHandle appInfo);
    [DllImport(Libs.LibGtk, EntryPoint = "g_file_icon_get_file", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetIconFile(nint icon);
    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetIconPath(this nint iconFile);
    [DllImport(Libs.LibGtk, EntryPoint = "g_themed_icon_get_names", CallingConvention = CallingConvention.Cdecl)]
    static extern IntPtr getIconNames(IntPtr icon);
}

/// <summary>
/// An icon from AppInfo, <see cref="AppIcon.Name"/> is either a path to the icon file or an icon name   
/// </summary>
/// <param name="Name"></param>
/// <param name="IsPath">Either a path to the icon file (true) or an icon name</param>
public record AppIcon(string Name, bool IsPath);