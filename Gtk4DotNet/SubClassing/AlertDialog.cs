using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class AlertDialog(nint obj) : SubClassTemplateInst<AdwAlertDialogHandle>(obj)
{ 
    public Task<string> PresentAsync(WidgetHandle parent) => Handle.PresentAsync(parent);

    protected override AdwAlertDialogHandle CreateHandle(nint obj) => new(obj);
}

public class AlertDialogClass : SubClassTemplateInstClass<AdwAlertDialogHandle>
{
    public AlertDialogClass(string typeName, string templateName, Func<nint, SubClassInst<AdwAlertDialogHandle>> constructor)
        : base(GTypeEnum.AdwAlertDialog, typeName, templateName, constructor)
    {}
}
