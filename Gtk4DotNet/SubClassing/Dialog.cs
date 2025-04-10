using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class Dialog<T>(nint obj) : SubClassTemplateInst<AdwDialogHandle>(obj)
{
    internal readonly TaskCompletionSource<T> completionSource = new();
}

class DialogClass<T> : SubClassTemplateInstClass<AdwDialogHandle>
{
    public DialogClass(string typeName, string templateName, Func<nint, SubClassInst<AdwDialogHandle>> constructor)
        : base(GTypeEnum.AdwDialog, typeName, templateName, constructor)
        => this.typeName = typeName;

    public Task<T> PresentAsync(WidgetHandle parent)
    {
        var dialogHandle = GObject.New<AdwDialogHandle>(typeName.TypeFromName());
        dialogHandle.Present(parent);
        var dialog = Dialog<T>.GetInstance(dialogHandle.GetInternalHandle()) as Dialog<T>;
        return dialog!.completionSource.Task;
    }

    readonly string typeName;
}
