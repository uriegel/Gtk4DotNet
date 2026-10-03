using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class WebKitUriSchemeResponse : GObject
{
    public WebKitUriSchemeResponse(InputStream stream, long length)
    {
        SetInternalHandle(New(stream, length));
        CheckDiagnostics();
    }

    public void HttpHeaders(SoupMessageHeaders headers)
    {
        headers.AutoDestroyed = true;
        SetHttpHeaders(this, headers);
    }

    public void Status(int status, string statusPhrase)
        => SetStatus(this, status, statusPhrase);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_response_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(InputStream stream, long length);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_response_set_content_type", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetContentType(WebKitUriSchemeResponse response, string contentType);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_response_set_http_headers", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHttpHeaders(WebKitUriSchemeResponse response, SoupMessageHeaders headers);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_response_set_status", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetStatus(WebKitUriSchemeResponse response, int status, string statusPhrase);
}

