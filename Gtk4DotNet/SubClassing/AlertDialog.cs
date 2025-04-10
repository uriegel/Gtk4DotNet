using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class AlertDialog(nint obj) : SubClassTemplateInst<AdwAlertDialogHandle>(obj)
{ 
    protected override AdwAlertDialogHandle CreateHandle(nint obj) => new(obj);
}

public class AlertDialogClass : SubClassTemplateInstClass<AdwAlertDialogHandle>
{
    public AlertDialogClass(string typeName, string templateName, Func<nint, SubClassInst<AdwAlertDialogHandle>> constructor)
        : base(GTypeEnum.AdwAlertDialog, typeName, templateName, constructor)
        => this.typeName = typeName;

    public Task<string> PresentAsync(WidgetHandle parent)
    {
        var dialogHandle = GObject.New<AdwAlertDialogHandle>(typeName.TypeFromName());
        return dialogHandle.PresentAsync(parent);
    }

    readonly string typeName;
}
