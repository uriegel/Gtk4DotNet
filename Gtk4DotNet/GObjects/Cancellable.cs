using System.Runtime.InteropServices;

namespace Gtk4DotNet;

class Cancellable : GObject
{
    public Cancellable(CancellationToken? cancellationToken = null)
    {
        SetInternalHandle(New());
        CheckDiagnostics();
        if (cancellationToken.HasValue && cancellationToken.Value.CanBeCanceled)
            cancellationTokenRegistration = cancellationToken.Value.Register(Cancel);
    }

    public void Cancel() => Cancel(this);

    [DllImport(Libs.LibGio, EntryPoint = "g_cancellable_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();

    [DllImport(Libs.LibGio, EntryPoint = "g_cancellable_cancel", CallingConvention = CallingConvention.Cdecl)]
    extern static void Cancel(Cancellable handle);

    readonly CancellationTokenRegistration cancellationTokenRegistration;

    #region IDisposable

    protected override void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                cancellationTokenRegistration.Unregister();
            // Verwalteten Zustand (verwaltete Objekte) bereinigen


            // Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // Große Felder auf NULL setzen

            disposedValue = true;
        }
        base.Dispose(disposing);
    }

    bool disposedValue;

    #endregion
}