using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

public class ManagedApplicationWindowClass : SubClass<ApplicationWindowHandle>
{
    public static ManagedApplicationWindowClass Register(Func<nint, ManagedApplicationWindow> constructor, string? template = null)
        => new(constructor, template);

    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        if (template != null)
            InitTemplateFromResource(cls, template);
    }

    ManagedApplicationWindowClass(Func<nint, ManagedApplicationWindow> constructor, string? template)
        : base(GTypeEnum.ApplicationWindow, "ManagedApplicationWindow", constructor)
            => this.template = template;

    string? template;
}

public class ManagedApplicationWindow(nint obj) : SubClassInst<ApplicationWindowHandle>(obj)
{
    protected override ApplicationWindowHandle CreateHandle(nint obj) => new(obj);
}
