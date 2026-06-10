using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class WebkitUriSchemeRequest : BaseHandle
{
    // TODO Leaking!
    public WebkitUriSchemeRequest(nint nativeHandle) : base()
    {
        SetInternalHandle(nativeHandle);
    }

    public string GetUri() => _GetUri(this).PtrToString(false) ?? "";

    public string GetHttpMethod() => _GetHttpMethod(this).PtrToString(false) ?? "";

    public void Finish(InputStream stream, long length, string content) => Finish(this, stream, length, content);

    public void Finish(WebKitUriSchemeResponse response) => Finish(this, response);

    public SoupMessageHeaders GetHttpHeaders() => GetHttpHeaders(this);

    public Stream GetHttpBody() => new ManagedGInputStream(_GetHttpBody(this));

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_http_body", CallingConvention = CallingConvention.Cdecl)]
    extern static InputStream _GetHttpBody(WebkitUriSchemeRequest request);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_uri", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetUri(WebkitUriSchemeRequest request);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_http_method", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetHttpMethod(WebkitUriSchemeRequest request);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static void Finish(WebkitUriSchemeRequest request, InputStream stream, long length, string content);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_finish_with_response", CallingConvention = CallingConvention.Cdecl)]
    extern static void Finish(WebkitUriSchemeRequest request, WebKitUriSchemeResponse response);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_http_headers", CallingConvention = CallingConvention.Cdecl)]
    extern static SoupMessageHeaders GetHttpHeaders(WebkitUriSchemeRequest request);
}
