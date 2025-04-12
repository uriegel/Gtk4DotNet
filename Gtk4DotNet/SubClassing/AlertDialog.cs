using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class AlertDialog(nint obj) : SubClassTemplateInst<AdwAlertDialogHandle>(obj)
{ 
    public Task<string> PresentAsync(WidgetHandle parent) => Handle.PresentAsync(parent);

    protected override AdwAlertDialogHandle CreateHandle(nint obj) => new(obj);
}

public class AlertDialogClass(string typeName, string templateName, Func<nint, SubClassTemplateInst<AdwAlertDialogHandle>> constructor) 
    : SubClassTemplateClass<AdwAlertDialogHandle>(GTypeEnum.AdwAlertDialog, typeName, templateName, constructor) { }
