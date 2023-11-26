using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public class PropertyAction
{
    [DllImport(Libs.LibGtk, EntryPoint="g_property_action_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ActionHandle New(string name, ObjectHandle obj, string propertyName);
}

