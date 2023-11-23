using GtkDotNet;
using GtkDotNet.SafeHandles;

namespace Dialog4;

static class PreferenceDialog
{
    public static void Show(WindowHandle window)
    {
        var dialog = 
            Dialog.New()
                .TransientFor(window)
                .Modal()
                .Resizable(false)
                .Title("Preferences")
                .Child(Box.New(Orientation.Vertical)
                    .Append(Grid.New()
                        .MarginTop(12)
                        .MarginBottom(12)
                        .MarginStart(12)
                        .MarginEnd(12)
                        .RowSpacing(12)
                        .ColumnSpacing(12)
                        .Attach(Label.New("_Font:"), 0, 0, 1, 1)
                        .Attach(Label.New("_Transition:"), 0, 1, 1, 1)))
                .Show();

    }
}