# Gtk4DotNet
.Net 6 bindings for GTK 4

Mostly functional approach like this:

```
using GtkDotNet;

using LinqTools;

return Application.Run("org.gtk.example", app =>
    Application
        .NewWindow(app)
        .SideEffect(win => win.SetTitle("Hello Gtk👍"))
        .SideEffect(win => win.SetChild(
            Grid
                .New()
                .SideEffect(g => g.Attach(
                    Button
                        .NewWithLabel("Button 1")
                        .SideEffect(btn => btn.SignalConnect("clicked", clicked))
                    , 0, 0, 1, 1))
                .SideEffect(g => g.Attach(
                    Button
                        .NewWithLabel("Button 2")
                        .SideEffect(btn => btn.SignalConnect("clicked", clicked))
                    , 1, 0, 1, 1))
                .SideEffect(g => g.Attach(
                    Button
                        .NewWithLabel("Quit")
                        .SideEffect(btn => btn.SignalConnect("clicked", () => win.Close()))
                    , 0, 1, 2, 1))
        ))
        .Show());

void clicked() => Console.WriteLine("Clicked button");    
```

## Prerequisites

### Ubuntu
* sudo apt install libgtk-4-dev

for use with WebView only:
* sudo apt install libwebkitgtk-6.0-dev

## Installation of GTK Schema
```
    sudo install -D org.gtk.example.gschema.xml /usr/share/glib-2.0/schemas/
    sudo glib-compile-schemas /usr/share/glib-2.0/schemas/
```     
## Usage

Look at the sample programs

## Checking if memory is being freed
To check if GObjects are being freed, just run
```
GObject.AddWeakRef(obj, (_, obj) => Console.WriteLine("... is being freed));
```
If this object is finalized, then the callback will be called.

