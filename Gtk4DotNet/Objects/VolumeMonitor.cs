using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class VolumeMonitor : GObject
{
    public static VolumeMonitor Get()
    {
        var vm = _Get();
        // Shared object
        //vm.CheckDiagnostics();
        return vm;
    }

    public void OnDriveChanged(Action onChanged)
        => driveChangeId = SignalConnect<ThreePointerDelegate>("drive-changed", (_, _, _) => onChanged(), true);

    [DllImport(Libs.LibGtk, EntryPoint = "g_volume_monitor_get", CallingConvention = CallingConvention.Cdecl)]
    extern static VolumeMonitor _Get();

    DelegateId? driveChangeId = null;

    #region IDisposable

    protected override void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            // if (disposing)
            //     ;
            // Verwalteten Zustand (verwaltete Objekte) bereinigen
            if (driveChangeId.HasValue)
                SignalDisconnect(driveChangeId.Value);

            // Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // Große Felder auf NULL setzen

            disposedValue = true;
        }
        base.Dispose(disposing);
    }

    bool disposedValue;

    #endregion
}

