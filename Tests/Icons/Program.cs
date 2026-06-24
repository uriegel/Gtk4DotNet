using CsTools.Extensions;
using Gtk4DotNet;
using WebServerLight;
using WebServerLight.Routing;
using Gtk4DotNet.Extensions;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("Hello Icons👍")
        .DefaultSize(600, 200)
        .Child(LinkButton
            .New("http://localhost:9865", "Open Website to show Icons"))
        .SideEffect(_ => WebServer
                            .New()
                            .Logging(LogLevel.Info)
                            .Http(9865)
                            .WebsiteFromResource()
                            .Route(MethodRoute
                            .New(Method.Get)
                                .Add(PathRoute.New("/iconfromname").Request(GetIconFromName))
                                .Add(PathRoute.New("/iconfromext").Request(GetIconFromExtension)))
                            .Build()
                            .Start())
        .Show()
    ).Run();

static async Task<bool> GetIconFromName(IRequest request)
{
    var subPath = request.SubPath;
    if (subPath == null)
        return true;
    var size = request.QueryParts.GetValue("size")?.ParseInt() ?? 64;
    return await GetIcon(request, subPath, size);
}

static async Task<bool> GetIconFromExtension(IRequest request)
{
    var subPath = request.SubPath;
    if (subPath == null)
        return false;
    var size = request.QueryParts.GetValue("size")?.ParseInt() ?? 64;
    using var icon = GIcon.Get(Gio.GuessContentType(subPath) ?? "none");
    var names = icon.ThemedNames().ToArray();
    return await GetIcon(request, names[0], size);
}

static async Task<bool> GetIcon(IRequest request, string name, int size)
{
    using var paintable = Display.GetDefault().GetIconTheme().LookupIcon(name, size);
    using var gfile = paintable.GetFile();
    var path = gfile.Path;
    if (path == null)
        return false;
    using var file = File.OpenRead(path);
    var payload = new byte[file.Length];
    int v = await file.ReadAsync(payload, 0, payload.Length);
    await request.SendAsync(payload, payload.IsSvg() ? "image/svg+xml" : "image/png");
    return true;
}

