using GtkDotNet;
using GtkDotNet.SafeHandles;

namespace Dialog4;

static class PreferenceDialog
{
    public static void Show(WindowHandle window)
        => Dialog.New()
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
                    .Attach(Label
                        .New("_Font:")
                        .XAlign(1)
                        .MnemonicWidget(font)
                        .UseUnderline(), 0, 0, 1, 1)
                    .Attach(FontButton
                        .New()
                        .Ref(font), 1, 0, 1, 1)
                    .Attach(Label
                        .New("_Transition:")
                        .XAlign(1)
                        .UseUnderline(), 0, 1, 1, 1)))
            .Show();

    static readonly WidgetRef<FontButtonHandle> font = new();
}