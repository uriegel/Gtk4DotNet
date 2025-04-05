using System.Net.Http.Headers;
using System.Threading.Tasks;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;


// TODO use only DrawingAreaHandle inherited
// TODO Binding

public class ProgressSpinnerClass(string name, Func<nint, ProgressSpinner> constructor)
    : SubClass<ProgressSpinnerHandle>(GTypeEnum.Box, name, constructor)
{ }

public class ProgressSpinner : SubClassInst<ProgressSpinnerHandle>
{
    public static SubClass<ProgressSpinnerHandle> Subclass()
        => new ProgressSpinnerClass("ProgressSpinner", p => new ProgressSpinner(p));

    protected override ProgressSpinnerHandle CreateHandle(nint obj) => new(obj);

    public ProgressSpinner(nint obj) : base(obj) { }

    protected internal override void OnCreate()
    {
        Handle.HAlign(Align.Center);
        Handle.VAlign(Align.Center);
        Handle.Wi
    }
}
