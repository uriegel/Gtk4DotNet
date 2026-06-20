using System.Drawing;
using System.Text;
using CsTools.Extensions;
using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("WebView from Resource👍")
        .DefaultSize(800, 600)
        .Child(WebView
            .New()
            .SideEffect(webview =>
            {
                WebKitWebContext.GetDefault().RegisterUriScheme("res", OnResRequest);
                webview.OnFinalize(WebKitWebContext.DisposeUriSchemes);
            })
            .BackgroundColor(Color.Transparent)
            .LoadUri("res://"))
        .Show()
    ).Run();

static void OnResRequest(WebkitUriSchemeRequest request)
{
    try
    {
        var uri = request.GetUri()[6..].SubstringAfter('/').SubstringUntil('?');
        uri = uri.Length > 0 ? uri : "index.html";
        var res = Resources.Get(uri);
        if (res != null)
        {
            var bytes = new byte[res.Length];
            var read = res.Read(bytes, 0, bytes.Length);
            using var gbytes = GBytes.New(bytes);
            using var gstream = MemoryInputStream.New(gbytes);
            request.Finish(gstream, bytes.Length, uri?.GetFileExtension()?.ToMimeType() ?? "text/html");
        }
        else
            SendNotFound(request);
    }
    catch
    {
        SendNotFound(request);
    }
}

static void SendNotFound(WebkitUriSchemeRequest request)
    => SendResponse(request, 404, "Not Found", "I can't find what you're looking for!");

static void SendResponse(WebkitUriSchemeRequest request, int code, string status, string text)
{
    using var bytes = GBytes.New(Encoding.UTF8.GetBytes(text));
    using var stream = MemoryInputStream.New(bytes);
    using var response = WebKitUriSchemeResponse.New(stream, text.Length);
    using var respondHeaders = SoupMessageHeaders.New(SoupMessageHeaderType.Response);
    respondHeaders.Set([new("Access-Control-Allow-Origin", "*")]);
    response.HttpHeaders(respondHeaders);
    response.Status(code, status);
    request.Finish(response);
}

