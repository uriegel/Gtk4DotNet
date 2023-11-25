using GtkDotNet;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace Dialog5;

class PreferenceDialog
{
    public void Show(WindowHandle window, SettingsHandle settings)
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
                        .Ref(font)
                        , 1, 0, 1, 1)
                    .Attach(Label
                        .New("_Transition:")
                        .XAlign(1)
                        .MnemonicWidget(transition)
                        .UseUnderline(), 0, 1, 1, 1)
                    .Attach(ComboBoxText
                        .New()
                        .Ref(transition)
                        .Append("none", "None")
                        .Append("crossfade", "Fade")
                        .Append("slide-left-right", "Slide"), 1, 1, 1, 1)))
            .SideEffect(_ => Settings.Bind(settings, "font", font.Ref, "font", BindFlags.Default))                        
            .SideEffect(_ => Settings.Bind(settings, "transition", transition.Ref, "active-id", BindFlags.Default))
            .Show();
            

    readonly ObjectRef<FontButtonHandle> font = new();
    readonly ObjectRef<ComboBoxTextHandle> transition = new();
}