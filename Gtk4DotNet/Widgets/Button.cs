using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Button : Widget
{
    /// <summary>
    /// A button can contain an icon by name
    /// </summary>
    public string IconName
    {
        get => GetIconName(this).PtrToString(false) ?? "";
        set => SetIconName(this, value);
    }
    
    /// <summary>
    /// Creates a new button with a label.
    /// </summary>
    /// <param name="label"></param>
    public Button(string label) : this(NewWithLabel(label)) { }

    public event Action OnClicked
    {
        add
        {
            TwoPointerDelegate unmanagedDelegate = (_, __) => value();
            var id = SignalConnectForEvent("clicked", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    public Button(Builder builder, string? name = null) : base(builder, name) { }

    public Button(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    internal Button(nint handle)  : base() 
    {
        SetInternalHandle(handle);
        CheckDiagnostics();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewWithLabel(string label);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetIconName(Button button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(Button button, string iconName);
}


