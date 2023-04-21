# Gtk4DotNet
.Net 6 bindings for GTK 4

## Prerequisites

### Ubuntu
* sudo apt install libgtk-4-dev

for use with WebView only:
* sudo apt install libwebkit2gtk-5.0-dev

## Installation of GTK Schema
```
    sudo install -D org.gtk.example.gschema.xml /usr/share/glib-2.0/schemas/
    sudo glib-compile-schemas /usr/share/glib-2.0/schemas/
```     
## Checking if memory is being freed
To check if GObjects are being freed, just run
```
GObject.AddWeakRef(obj, (_, obj) => Console.WriteLine("... is being freed));
```
If this object is finalized, then the callback will be called.