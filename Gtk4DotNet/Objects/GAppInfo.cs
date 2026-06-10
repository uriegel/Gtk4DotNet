using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

// TODO check
public class GAppInfo : GObject
{
    public static DisposableEnumerable<GAppInfo> GetAllApps()
    {
        var list = GetALl();
        var result = GetItems(list)
            .ToArray()
            .AsDisposable();

        GList.Free(list);
        return result;
    }

    public static DisposableEnumerable<GAppInfo> GetRecommendedApps(string contentType)
    {
        var list = _GetRecommended(contentType);
        var result = GetItems(list)
            .ToArray()
            .AsDisposable();

        GList.Free(list);
        return result;
    }

    public string? Name { get => _GetName(this).PtrToString(false); }

    public string? GetExecutable { get => _GetExecutable(this).PtrToString(false); }

    // TODO geticons now better!
    public AppIcon? GetIcon()
    {
        var icon = _GetIcon(this);
        if (icon == 0)
            return null;

        var file = GetIconFile(icon);
        if (file != 0)
        {
            var iconPath = GetIconPath(file).PtrToString(true);
            if (iconPath != null)
                return new(iconPath, true);
        }
        var names = GetIconNames(icon);
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

    static IEnumerable<GAppInfo> GetItems(nint list)
    {
        var current = list;
        while (current != 0)
        {
            var node = Marshal.PtrToStructure<GList>(current);
            var app = node.Data;
            yield return new GAppInfo(app);
            current = node.Next;
        }
    }

    public GAppInfo() : base() { }

    public GAppInfo(nint obj) : base()
    {
        SetInternalHandle(obj);
        CheckDiagnostics();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_all", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetALl();

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_recommended_for_type", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetRecommended(string contentType);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(GAppInfo appInfo);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_executable", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetExecutable(GAppInfo appInfo);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_icon", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetIcon(GAppInfo appInfo);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_icon_get_file", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetIconFile(nint icon);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetIconPath(nint iconFile);

    [DllImport(Libs.LibGtk, EntryPoint = "g_themed_icon_get_names", CallingConvention = CallingConvention.Cdecl)]
    static extern nint GetIconNames(nint icon);
}

/// <summary>
/// An icon from AppInfo, <see cref="AppIcon.Name"/> is either a path to the icon file or an icon name   
/// </summary>
/// <param name="Name"></param>
/// <param name="IsPath">Either a path to the icon file (true) or an icon name</param>
public record AppIcon(string Name, bool IsPath);