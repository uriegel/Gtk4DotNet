using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet.Controls;

public class ManagedAdwApplicationWindowClass : SubClass<AdwApplicationWindowHandle>
{
    public static ManagedAdwApplicationWindowClass Register(Func<nint, ManagedAdwApplicationWindow> constructor, string? template = null)
        => new(constructor, template);

    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        if (template != null)
            InitTemplateFromResource(cls, template);
    }

    ManagedAdwApplicationWindowClass(Func<nint, ManagedAdwApplicationWindow> constructor, string? template)
        : base(GTypeEnum.AdwApplicationWindow, Application.MANAGED_ADW_APPLICATION_WINDOW, constructor)
            => this.template = template;

    readonly string? template;
}

public class ManagedAdwApplicationWindow(nint obj) : SubClassInst<AdwApplicationWindowHandle>(obj)
{
    protected override AdwApplicationWindowHandle CreateHandle(nint obj) => new(obj);
    protected internal virtual void Initialize() {}
}
