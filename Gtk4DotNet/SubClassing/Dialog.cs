using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class Dialog<T>(nint obj) : SubClassTemplateInst<AdwDialogHandle>(obj)
    where T : notnull
{
    public Task<T?> PresentAsync(WidgetHandle parent)
    {
        Handle.Present(parent);
        var dialog = Dialog<T>.GetInstance(Handle.GetInternalHandle()) as Dialog<T>;
        return dialog!.completionSource.Task;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Handle.OnClosed(() => completionSource.TrySetResult(default));
    }

    protected override AdwDialogHandle CreateHandle(nint obj) => new(obj);

    protected void Close(T t)
    {
        completionSource.TrySetResult(t);
        Handle.CloseDialog();
    }

    internal readonly TaskCompletionSource<T?> completionSource = new();
}

public class DialogClass<T>(string typeName, string templateName, Func<nint, SubClassTemplateInst<AdwDialogHandle>> constructor) 
    : SubClassTemplateClass<AdwDialogHandle>(GTypeEnum.AdwDialog, typeName, templateName, constructor) { }
