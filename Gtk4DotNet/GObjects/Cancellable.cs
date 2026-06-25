using System.Runtime.InteropServices;

namespace Gtk4DotNet;

class Cancellable : GObject
{
    public static Cancellable New(CancellationToken? cancellationToken = null)
    {
        if (cancellationToken.HasValue && cancellationToken.Value.CanBeCanceled)
        {
            var cancellable = new Cancellable(cancellationToken.Value);
            cancellable.SetInternalHandle(New());
            cancellable.CheckDiagnostics();
            return cancellable;
        }
        else
            return None();
    }

    public static Cancellable None() => new();

    public Cancellable() : base() { }

    public void Cancel() => Cancel(this);

    Cancellable(CancellationToken cancellationToken) : base()
        => cancellationTokenRegistration = cancellationToken.Register(Cancel);

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