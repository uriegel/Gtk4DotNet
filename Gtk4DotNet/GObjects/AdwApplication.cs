using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// TODO OnCommandLine

/// <summary>
/// The GTK Application. 
/// </summary>
public class AdwApplication : Application
{
    /// <summary>
    /// Creates a new Adwaita application
    /// </summary>
    /// <param name="applicationId">The Gtk Application ID this application is connected to</param>
    /// <param name="flags"></param>
    public AdwApplication(string applicationId, ApplicationFlags flags = ApplicationFlags.None)
        : base(applicationId)
    {
        var app = NewAdw(applicationId, flags);
        Gtk.Init();
        CheckDiagnostics();
        SetInternalHandle(app);
    }

    [DllImport(Libs.LibAdw, EntryPoint = "adw_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewAdw(string id, ApplicationFlags flags);
}
