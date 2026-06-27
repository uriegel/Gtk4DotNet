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
    /// <returns></returns>
    public static Button NewWithLabel(string label)
    {
        var res = _NewWithLabel(label);
        res.CheckDiagnostics();
        return res;
    }

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

    public Button() : base() { }

    public Button(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    extern static Button _NewWithLabel(string label);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetIconName(Button button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(Button button, string iconName);
}

public static class ButtonExtensions
{
    /// <summary>
    /// A button can contain an icon by name
    /// </summary>
    /// <typeparam name="THandle"></typeparam>
    /// <param name="button"></param>
    /// <param name="iconName"></param>
    /// <returns>This button so that chained method calls are possible</returns>
    public static THandle IconName<THandle>(this THandle button, string iconName)
        where THandle : Button
        => button.SideEffect(b => b.IconName = iconName);
}

