using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class WebKitWebContext : FloatingObject
{
    public static WebKitWebContext GetDefault()
    {
        var context = _GetDefault();
        context.CheckDiagnostics();
        return context;
    }

    public void RegisterUriScheme(string scheme, Action<WebkitUriSchemeRequest> callback)
    {
        CustomSchemeRequestDelegate delelegat = request => callback(new WebkitUriSchemeRequest(request));
        GtkDelegates.Add(delelegat);
        RegisterUriScheme(this, scheme, Marshal.GetFunctionPointerForDelegate((Delegate)callback));
    }

    public WebKitWebContext() : base() { }
    
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_context_get_default", CallingConvention = CallingConvention.Cdecl)]
    extern static WebKitWebContext _GetDefault();

    // [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_context_get_security_manager", CallingConvention = CallingConvention.Cdecl)]
    // extern static WebKitSecurityManagerHandle GetSecurityManager(this WebKitWebContextHandle context);
                        
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_context_register_uri_scheme", CallingConvention = CallingConvention.Cdecl)]
    extern static void RegisterUriScheme(WebKitWebContext context, string scheme, IntPtr callback, nint _ = 0, nint __ = 0);
}
