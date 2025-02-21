using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Dialog
{
    /// <summary>
    /// Returned if an action widget has no response id, or if the dialog gets programmatically hidden or destroyed.
    /// </summary>
    public const int RESPONSE_NONE = -1;
    /// <summary>
    /// Generic response id, not used by GTK dialogs.
    /// </summary>
    public const int RESPONSE_REJECT = -2;
    /// <summary>
    /// Generic response id, not used by GTK dialogs.
    /// </summary>
    public const int RESPONSE_ACCEPT = -3;
    /// <summary>
    /// Returned if the dialog is deleted.
    /// </summary>
    public const int RESPONSE_DELETE_EVENT = -4;
    /// <summary>
    /// Returned by OK buttons in GTK dialogs.
    /// </summary>
    public const int RESPONSE_OK = -5;
    /// <summary>
    /// Returned by Cancel buttons in GTK dialogs.
    /// </summary>
    public const int RESPONSE_CANCEL = -6;
    /// <summary>
    /// Returned by Close buttons in GTK dialogs.
    /// </summary>
    public const int RESPONSE_CLOSE = -7;
    /// <summary>
    /// Returned by Yes buttons in GTK dialogs.
    /// </summary>
    public const int RESPONSE_YES = -8;
    /// <summary>
    /// Returned by No buttons in GTK dialogs.
    /// </summary>
    public const int RESPONSE_NO = -9;
    /// <summary>
    /// Returned by Apply buttons in GTK dialogs.
    /// </summary>
    public const int RESPONSE_APPLY = -10;
    /// <summary>
    /// Returned by Help buttons in GTK dialogs.
    /// </summary>
    public const int RESPONSE_HELP = -11;

    [DllImport(Libs.LibGtk, EntryPoint="gtk_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static DialogHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_dialog_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();        

    public static DialogHandle New(string title, WindowHandle parent, DialogFlags flags, string button, int response)
        => New(title, parent, flags, button, response, IntPtr.Zero);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_dialog_new_with_buttons", CallingConvention = CallingConvention.Cdecl)]
    extern static DialogHandle New(string title, WindowHandle parent, DialogFlags flags, string firstButton, int response, IntPtr zero);
}

