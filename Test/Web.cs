using GtkDotNet;
using CsTools.Extensions;

using static System.Console;

static class Web
{
    public static int Run()
        =>  Application
            .New("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Hello Web View👍")
                    .DefaultSize(800, 600)
                    .Child(
                        WebKit
                            .New()
                            .SideEffect(wk => 
                                wk.AddController(
                                EventControllerKey
                                    .New()
                                    .OnKeyPressed((k, kc, m) => {
                                        if (kc == 73)
                                        {
                                            // prevent blink_cb crash!
                                            wk.RunJavascript(
"""
    console.log("Der F7")
    const keyboardEvent = document.createEvent('KeyboardEvent')
    const initMethod = typeof keyboardEvent.initKeyboardEvent !== 'undefined' ? 'initKeyboardEvent' : 'initKeyEvent'
    keyboardEvent[initMethod](
        'keydown', // event type: keydown, keyup, keypress
        true, // bubbles
        true, // cancelable
        window, // view: should be window
        false, // ctrlKey
        false, // altKey
        false, // shiftKey
        false, // metaKey
        118, // keyCode: unsigned long - the virtual key code, else 0
        0, // charCode: unsigned long - the Unicode character associated with the depressed key, else 0
    )
    document.dispatchEvent(keyboardEvent) 
""");
                                            return true;
                                        }
                                        else
                                            return false;
                                    })))
                            .SideEffect(w => w.GetSettings()
                                .SideEffect(s => 
                                {
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                    WriteLine($"CursiveFontFamily: {s.CursiveFontFamily}");
                                    s.EnableDeveloperExtras = true;
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                }))
                            .OnLoadChanged((w, e) => 
                                e.SideEffectIf(e == WebViewLoad.Finished, 
                                    _ => w.RunJavascript("console.log('called from C#')")))
                            .DisableContextMenu()
                            .OnAlert((w, text) => 
                                text
                                    .SideEffectIf(text == "showDevTools",
                                        _ => w.GetInspector().Show())
                                    .SideEffect(text => WriteLine($"on alert: {text}")))
                            .LoadUri($"file://{Directory.GetCurrentDirectory()}/webroot/index.html")
                    )
                    .Show())
            .Run(0, IntPtr.Zero);
}


