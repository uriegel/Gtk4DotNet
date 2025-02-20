using System.Runtime.InteropServices;
using GtkDotNet;

[StructLayout(LayoutKind.Sequential)]
public struct GObjectClass
{
    public IntPtr g_type_class; // GTypeClass* (pointer to parent type class)

    IntPtr construct_properties;
    // Virtual function pointers
    public IntPtr constructor;
    public IntPtr set_property;
    public IntPtr get_property;
    public IntPtr dispose;
    public IntPtr finalize;

    public IntPtr dispatch_properties_changed;
    public IntPtr notify;
    IntPtr constructed;
    ulong flags;
    ulong n_construct_properties;
    IntPtr specs;
    ulong size;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public IntPtr[] padding; // Reserved for future expansion
}

//   GTypeClass   g_type_class;

//   /*< private >*/
//   GSList      *construct_properties;

//   /*< public >*/
//   /* seldom overridden */
//   GObject*   (*constructor)     (GType                  type,
//                                  guint                  n_construct_properties,
//                                  GObjectConstructParam *construct_properties);
//   /* overridable methods */
//   void       (*set_property)		(GObject        *object,
//                                          guint           property_id,
//                                          const GValue   *value,
//                                          GParamSpec     *pspec);
//   void       (*get_property)		(GObject        *object,
//                                          guint           property_id,
//                                          GValue         *value,
//                                          GParamSpec     *pspec);
//   void       (*dispose)			(GObject        *object);
//   void       (*finalize)		(GObject        *object);
//   /* seldom overridden */
//   void       (*dispatch_properties_changed) (GObject      *object,
// 					     guint	   n_pspecs,
// 					     GParamSpec  **pspecs);
//   /* signals */
//   void	     (*notify)			(GObject	*object,
// 					 GParamSpec	*pspec);

//   /* called when done constructing */
//   void	     (*constructed)		(GObject	*object);

//   /*< private >*/
//   gsize		flags;

//   gsize         n_construct_properties;

//   gpointer pspecs;
//   gsize n_pspecs;

//   /* padding */
//   gpointer	pdummy[3];


[StructLayout(LayoutKind.Sequential)]
public struct TDoubleClass
{
    GObjectClass parent_class;
}

[StructLayout(LayoutKind.Sequential)]
public struct TDouble
{
    GObjectI parent;
   double value;
}

[StructLayout(LayoutKind.Sequential)]
struct GObjectI
{
    public IntPtr g_type_instance; // Pointer to GObjectClass
    public uint ref_count;         // Reference count
    public IntPtr qdata;
}

static class First
{
    [DllImport("libgtk-4.so.1", EntryPoint = "g_object_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr New(long type, IntPtr zero);
    
    [DllImport("libgtk-4.so.1", EntryPoint="g_object_get_type", CallingConvention = CallingConvention.Cdecl)]
    static extern ulong ObjectGetType();        

    [DllImport("libgtk-4.so.1", EntryPoint = "g_type_register_static", CallingConvention = CallingConvention.Cdecl)]
    extern static ulong RegisterStaticType(ulong parent_type, string type_name, ref GInfoType info, int flags);


    static GClassInitDelegate classInitDelegate = i =>
    {

    };

    static GInstanceInitDelegate instanceDelegate = i =>
    {

    };
    public static int Run()
    {
        var typ = ObjectGetType();

        ulong t_double_get_type()
        {
            GInfoType info = new()
            {
                class_size = (ushort)Marshal.SizeOf<TDoubleClass>(),
                instance_size = (ushort)Marshal.SizeOf<TDouble>(),
                class_init = Marshal.GetFunctionPointerForDelegate(classInitDelegate),
                instance_init = Marshal.GetFunctionPointerForDelegate(instanceDelegate)
            };
            return RegisterStaticType(typ, "TDouble", ref info, 0);
        }

        var tdoubleType = t_double_get_type();

        var d = New((long)tdoubleType, 0);
        var d2 = New((long)tdoubleType, 0);

        

        return Application
                .New("org.gtk.example")
                .OnActivate(app =>
                    app
                        .NewWindow()
                            .Title("Hello Gtk👍")
                            .DefaultSize(1200, 1200)
                            .Show())
                .Run(0, IntPtr.Zero);
    }
}

delegate void GClassInitDelegate(nint cls);
delegate void GInstanceInitDelegate(nint cls);



[StructLayout(LayoutKind.Sequential)]
public struct GInfoType
{
    /* interface types, classed types, instantiated types */
    public ushort class_size;
    public nint base_init;
    public nint base_finalize;

    /* interface types, classed types, instantiated types */
    public nint class_init;
    public nint class_finalize;
    public nint class_data;

    /* instantiated types */
    public ushort instance_size;
    public ushort n_preallocs;
    public nint instance_init;

    /* value handling */
    public nint value_table;
}

