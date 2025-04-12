using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassTemplateInst<THandle>(nint obj) : SubClassWidgetInst<THandle>(obj)
    where THandle : WidgetHandle, new()
{
    protected internal override void OnCreate()
    {
        Handle.InitTemplate();
        base.OnCreate();
    }

    protected TResultHandle GetTemplateChild<TResultHandle>(string id)
        where TResultHandle : WidgetHandle, new()
        => Handle.GetTemplateChild<TResultHandle, THandle>(id);
}

public class SubClassTemplateClass<THandle>(GTypeEnum parent, string typeName, string templateName, Func<nint, SubClassTemplateInst<THandle>> constructor)
    : SubClass<THandle>(parent, typeName, constructor)
    where THandle : WidgetHandle, new()
{
    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, templateName);
    }  
}
