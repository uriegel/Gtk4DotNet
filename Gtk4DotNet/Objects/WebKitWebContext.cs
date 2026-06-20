using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class WebKitWebContext : GObject
{
    // Do not call CheckDiagnostics because app hangs indefinetely
    public static WebKitWebContext GetDefault() 
    {
        var res = GetDefault();
        res.AutoDestroyed = true;
        return res;
    }

    public static void DisposeUriSchemes()
    {
        foreach (var scheme in uriSchemes)
            GtkDelegates.Instance.Remove(scheme);
        uriSchemes.Clear();
    }

    public void RegisterUriScheme(string scheme, Action<WebkitUriSchemeRequest> callback)
        => RegisterUriScheme(scheme, request => callback(new WebkitUriSchemeRequest(request)));

    void RegisterUriScheme(string scheme, CustomSchemeRequestDelegate callback)
    {
        uriSchemes.Add(GtkDelegates.Instance.Add(callback, "UriScheme"));
        RegisterUriScheme(this, scheme, Marshal.GetFunctionPointerForDelegate((Delegate)callback));
    }

    public WebKitWebContext() : base() { }
    
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_context_get_default", CallingConvention = CallingConvention.Cdecl)]
    extern static WebKitWebContext _GetDefault();

    // [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_context_get_security_manager", CallingConvention = CallingConvention.Cdecl)]
    // extern static WebKitSecurityManagerHandle GetSecurityManager(this WebKitWebContextHandle context);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_context_register_uri_scheme", CallingConvention = CallingConvention.Cdecl)]
    extern static void RegisterUriScheme(WebKitWebContext context, string scheme, IntPtr callback, nint _ = 0, nint __ = 0);

    static List<long> uriSchemes = [];
}
