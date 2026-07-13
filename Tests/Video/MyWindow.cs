using System.Reflection;
using System.Runtime.InteropServices;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        // NativeLibrary.SetDllImportResolver(Assembly.GetEntryAssembly()!, ResolveLibrary);

        // test("Hallo Wörld");
        mediaFile = MediaFile.New("/mnt/Home/uwe/Büro.mkv");
        var asp = (mediaFile as IPaintable).IntrinsicAspectRatio;
        var w = (mediaFile as IPaintable).IntrinsicWidth;
        var h = (mediaFile as IPaintable).IntrinsicHeight;
        mediaControls.SetMediaStream(mediaFile);
        video.SetPaintable(mediaFile);
        video.KeepAspectRatio = false;
        OnFinalize(mediaFile.Dispose);

        später();
        async void später()
        {
            await Task.Delay(500);
            var asp = (mediaFile as IPaintable).IntrinsicAspectRatio;
            var w = (mediaFile as IPaintable).IntrinsicWidth;
            var h = (mediaFile as IPaintable).IntrinsicHeight;
        }
    }

    [Widget]
    readonly Picture video = null!;

    [Widget]
    readonly MediaControls mediaControls = null!;

    readonly MediaFile mediaFile;

    static nint ResolveLibrary( string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName == "libtgtk4dotnet.so")
        {
            string path = "/mnt/Home/Projekte/Gtk4DotNet/C-Code/gtk4dotnet/libtgtk4dotnet.so";
            return NativeLibrary.Load(path);
        }

        return IntPtr.Zero;
    }

    [DllImport("libtgtk4dotnet.so", CallingConvention = CallingConvention.Cdecl)]
    extern static void test(string text);

}

