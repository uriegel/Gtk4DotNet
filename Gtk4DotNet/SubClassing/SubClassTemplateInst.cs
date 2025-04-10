using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassTemplateInst<THandle>(nint obj) : SubClassInst<THandle>(obj)
    where THandle : WidgetHandle, new()
{
    protected internal async override void OnCreate()
    {
        Handle.InitTemplate();
        await Task.Delay(1);
        OnInitialize();
    }

    protected TResultHandle GetTemplateChild<TResultHandle>(string id)
        where TResultHandle : WidgetHandle, new()
        => Handle.GetTemplateChild<TResultHandle, THandle>(id);

    protected virtual void OnInitialize() { }
}

class SubClassTemplateInstClass<THandle>(GTypeEnum parent, string typeName, string templateName, Func<nint, SubClassInst<THandle>> constructor)
    : SubClass<THandle>(parent, typeName, constructor)
    where THandle : WidgetHandle, new()
{
    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, templateName);
    }  
}
