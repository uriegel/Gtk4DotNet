using GtkDotNet;
using GtkDotNet.SafeHandles;

namespace Dialog4;

static class PreferenceDialog
{
    public static void Show(WindowHandle window)
    {
        var dialog = 
            Dialog
                .New("Hello World beenden?", window, DialogFlags.DestroyWithParent | DialogFlags.Modal, "Ok", Dialog.RESPONSE_OK)
                .Show();

    }
}