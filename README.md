# Gtk4DotNet
C# .NET 10 bindings for GTK4. You can create programs using the GTK4 UI system as a .NET 10 app.

Highlights:

* Very lightweight approach, functional and object oriented.
* Support of template.ui resources as .NET resources so that the UI can be designed with [Cambalache](https://github.com/ag-python/cambalache).
* Sub classing of Gtk Widget the C# way, not the Gtk way. It is very simple to create a Widget in a C# class that is inhherited from a GTK Widget. The UI of this custom widget can be defined in a UI template if it is a composite Widget.
* Mapping of the Gtk Threading and Gtk Main Event Loop to async/await with Synchronization context, so that asynchronous workflows or running in UI thread can be completely solved with async/await in C#.
* GTk4 property bindings to C# properties in a DataContext implementing INotifyProperty like WPF.
* Support for Adwaita
* GSettings support without the need to install them as super user. 

### Remarks to Version 9.0:
Version 9.0 is a breaking change to older versions of this C# class library. That was necessary because the focus was shifted from functional building of the UI to easy subclassing of parts of the UI as C# objects so that bigger projects can be better modularized.

More emphasis was placed on the Changing UI and reacting on UI actions than on building the UI.

The functional builder concept has been partially retained, but now it is strongly recommended to use Gtk template.ui in connection with subbclassed Gtk widgets.
 
