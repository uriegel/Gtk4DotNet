using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO refactor
class Cancellable : GObject
{
    public Cancellable(CancellationToken cancellationToken) : this(true)
        => cancellationTokenRegistration = cancellationToken.Register(Cancel);

    public static Cancellable Zero() => new(false);

    public void Cancel() => Cancel(this);

    [DllImport(Libs.LibGio, EntryPoint = "g_cancellable_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();

    [DllImport(Libs.LibGio, EntryPoint = "g_cancellable_cancel", CallingConvention = CallingConvention.Cdecl)]
    extern static void Cancel(Cancellable handle);

    Cancellable(bool create)
    {
        if (create)
        {
            var handle = New();
            SetInternalHandle(handle);
        }
    }

    readonly CancellationTokenRegistration cancellationTokenRegistration;

    #region IDisposable

    protected override void Dispose(bool disposing)
    {
        Dispose();
        
        if (!disposedValue)
        {
            if (disposing)
                cancellationTokenRegistration.Unregister();
            // Verwalteten Zustand (verwaltete Objekte) bereinigen


            // Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // Große Felder auf NULL setzen

            disposedValue = true;
        }
    }

    //  Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    // ~Cancellable()
    //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
    //     => Dispose(disposing: false);

    bool disposedValue;

    #endregion
}