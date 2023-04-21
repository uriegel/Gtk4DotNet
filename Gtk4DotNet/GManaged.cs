using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class GManaged<T>
{
    public static long Type { get; }
    
    public static T GetValue(IntPtr managedType) 
    {
        var intPtr = Marshal.ReadIntPtr(managedType, 28); 
        var handle = GCHandle.FromIntPtr(intPtr);
        return (T)handle.Target;
    }

    public static void SetValue(IntPtr managedType, T value) 
    {
        Free(managedType);
        var handle = GCHandle.Alloc(value);
        Marshal.WriteIntPtr(managedType, 28, GCHandle.ToIntPtr(handle)); 
    }

    public static IntPtr New() 
    {
        var managedType = GObject.New(Type, IntPtr.Zero);
        GObject.AddWeakRef(managedType, (_, obj) => Free(obj));
        return managedType;
    }

    public static IntPtr New(T value) 
    {
        var obj = New();
        SetValue(obj, value);
        return obj;
    }

    static void Free(IntPtr managedType)
    {
        var intPtr = Marshal.ReadIntPtr(managedType, 28); 
        if (intPtr != IntPtr.Zero)
        {
            var handle = GCHandle.FromIntPtr(intPtr);
            if (handle.Target is IDisposable disposable)
                disposable.Dispose();
            handle.Free();
        }
    }

    static GManaged() => Type = GManaged.Type;
}

static class GManaged
{
    public static long Type { get; }

    static GManaged()
    {
        var info = new GTypeInfo();
        info.class_size = 136;
        info.class_size = (ushort)Marshal.SizeOf<IntType2Class>();// 136
        info.instance_size = (ushort)Marshal.SizeOf<IntType>(); // 32
        Type = RegisterStatic(80, "GtkDotNetGManaged", ref info, 0);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct IntType 
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=28)] 
        byte[] parent; 
        public IntPtr intPtr;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct IntType2Class 
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=136)] 
        public byte[] parentClass; 
    }

    [DllImport(Globals.LibGtk, EntryPoint="g_type_register_static", CallingConvention = CallingConvention.Cdecl)]
    extern static long RegisterStatic(int type, string typeName, ref GTypeInfo info, int flags);
}