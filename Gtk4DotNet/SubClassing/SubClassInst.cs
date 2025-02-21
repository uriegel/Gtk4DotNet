using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassInst<THandle>
    where THandle : ObjectHandle, IDisposable
{
    public THandle Handle { get; }

    protected SubClassInst(nint obj)
    {
        Handle = CreateHandle(obj);
        objects[obj] = this;
        OnCreate();
    }

    protected virtual void OnCreate() { }
    protected virtual void OnFinalize() { }

    protected abstract THandle CreateHandle(nint obj);

    static internal DisposeCallback finalizeDelegate = FinalizeHandler;

    static void FinalizeHandler(IntPtr obj)
    {
        objects[obj].OnFinalize();
        objects.Remove(obj);
    }

    readonly static Dictionary<IntPtr, SubClassInst<THandle>> objects = [];

    #region IDisposable

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                // Verwalteten Zustand (verwaltete Objekte) bereinigen
                Handle.Dispose();

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