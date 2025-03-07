using CsTools.Extensions;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet.Controls;

class GManagedObjectClass<T>(string name, Func<nint, GManagedObject<T>> constructor)
    : SubClass<GObjectHandle>(GTypeEnum.GObject, name, constructor)
{
    readonly Dictionary<string, object> registeredObjects = [];
}

class GManagedObject<T>(nint obj) : SubClassInst<GObjectHandle>(obj)
{
    public static GManagedObject<T> New(T t)
    {
        using var handle = GObject.New<GObjectHandle>(GType);
        handle.IsFloating = true;
        var res = handle.GetInstance() as GManagedObject<T>;
        if (res != null)
            res.Value = t;
        return res!;
    }

    public static GTypeHandle GType { get => _GType ?? ("GManagedObjectClass" + typeof(T).Name).TypeFromName().SideEffect(n => _GType = n); }
    static GTypeHandle? _GType;
    
    public T? Value { get; set; }
    protected override void OnFinalize() => Console.WriteLine("GManagedObject finalized");

    protected override GObjectHandle CreateHandle(nint obj) => new(obj);
}
