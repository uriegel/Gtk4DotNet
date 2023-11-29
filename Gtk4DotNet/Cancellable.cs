using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using GtkDotNet;
using GtkDotNet.SafeHandles;

public class Cancellable : IDisposable
{
    public Cancellable(CancellationToken cancellationToken) : this(true)
        => cancellationToken.Register(Cancel);
    
    public static Cancellable Zero() => new(false);

    public void Cancel() => Cancel(handle);

    [DllImport(Libs.LibGio, EntryPoint = "g_cancellable_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CancellableHandle New();

    [DllImport(Libs.LibGio, EntryPoint = "g_cancellable_cancel", CallingConvention = CallingConvention.Cdecl)]
    extern static void Cancel(CancellableHandle handle);

    Cancellable(bool create)
    {
        if (create)
            handle = New();
        else
            handle = new();
    }

    internal CancellableHandle handle;

    #region IDisposable

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                handle.Dispose();
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

    public void Dispose()
    {
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    bool disposedValue;
    
    #endregion
}