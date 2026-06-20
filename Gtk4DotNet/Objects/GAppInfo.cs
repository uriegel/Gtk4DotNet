using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class GAppInfo : GObject
{
    public bool ShouldShow { get => _ShouldShow(this); }

    public static GAppInfo? GetDefault(string contentType, bool mustSupportUris = false)
    {
        var app = _GetDefault(contentType, mustSupportUris);
        if (app.IsInvalid)
            return null;
        app.CheckDiagnostics();
        return app;
    }

    public static DisposableEnumerable<GAppInfo> GetAllApps()
        => GetApps(GetAll());

    public static DisposableEnumerable<GAppInfo> GetAllApps(string contentType)
        => GetApps(GetAll(contentType));

    public static DisposableEnumerable<GAppInfo> GetRecommendedApps()
        => GetApps(GetRecommended());

    public static DisposableEnumerable<GAppInfo> GetRecommendedApps(string contentType)
        => GetApps(GetRecommended(contentType));

    static DisposableEnumerable<GAppInfo> GetApps(nint list)
    {
        var result = GetItems(list)
            .ToArray()
            .AsDisposable();

        GList.Free(list);
        return result;
    }

    public string? Name { get => GetName(this).PtrToString(false); }

    public string? Executable { get => GetExecutable(this).PtrToString(false); }

    public GIcon GetIcon()
    {
        var icon = GetIcon(this);
        icon.CheckDiagnostics();
        icon.AutoDestroyed = true;
        return icon;
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

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_default_for_type", CallingConvention = CallingConvention.Cdecl)]
    extern static GAppInfo _GetDefault(string contentType, bool supportUris);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_all", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetAll();

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_all_for_type", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetAll(string contentType);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_recommended", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetRecommended();

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_recommended_for_type", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetRecommended(string contentType);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetName(GAppInfo appInfo);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_executable", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetExecutable(GAppInfo appInfo);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_get_icon", CallingConvention = CallingConvention.Cdecl)]
    extern static GIcon GetIcon(GAppInfo appInfo);

    [DllImport(Libs.LibGtk, EntryPoint = "g_app_info_should_show", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _ShouldShow(GAppInfo appInfo);
}

