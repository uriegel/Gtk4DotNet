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

The following tutorial contains the ExampleApp (and others) from the original [GTK4 documentation](https://docs.gtk.org/gtk4/getting_started.html) as well the ToDo List app from [GUI development with Rust and GTK 4](https://gtk-rs.org/gtk4-rs/git/book/), all ported to C#.

### Remarks to Version 9.0:
Version 9.0 is a breaking change to older versions of this C# class library. That was necessary because the focus was shifted from functional building of the UI to easy subclassing of parts of the UI as C# objects so that bigger projects can be better modularized.

More emphasis was placed on changing UI state and reacting on UI actions than on building the UI.

The functional builder concept has been partially retained, but now it is strongly recommended to use Gtk template.ui in connection with subbclassed Gtk widgets.
 
# Hello World app and introduction to Gtk4DotNet

## Necessary prerequisites only depending on the version of Linux

On modern Linux like Ubuntu 24.04 or Fedora 40 Gtk4DotNet apps will run out of the box (if you create a full contained single file exe), otherwise you have to install the necessary dotnet runtime.

libadwaita is only necessary if you want to create Adwaita apps, and webkitgtk6 you only need when integrating a webview.

On older/other Linux systems perhaps you have to install one of the following packages in order to make the app runnable. 

``` 
sudo apt install libgtk-4-dev
sudo apt install libadwaita-1-dev
sudo apt install libwebkitgtk-6.0-dev
```

For example on Linux Mint 22 you only have to install 

``` 
sudo apt install libwebkitgtk-6.0-dev
```
if you want to use webkit webview whereas for KDE neon 6.0 you have to install 

``` 
sudo apt install libadwaita-1-dev
sudo apt install libwebkitgtk-6.0-dev
``` 
## Setup to a Gtk4DotNet program

You have to setup a .NET 10 console app.To access the library, you need a reference to the nuget package  [Gtk4DotNet](https://www.nuget.org/packages/Gtk4DotNet/). In your project, add it with the help of this command line command:
```
dotnet add package Gtk4DotNet
``` 
Thats all to build the simple HelloWorld app.

## Application object

The most essential class is ```Application``` (together with ```Window```).

You have to create an instance of Application, then call at least the method ```Run```. Gtk4 will then be initialized, and the main event loop is started.

Almost all Gtk Objects are created with a static method, mostly ```New```:
```cs
using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .Run();
```

New demands the ApplicationID, a string that represents your app domain in reverse order.

This is the simplest Gtk Application. When you run the app, it stops immediatly with the following maeesage in command line:

```

(HelloWorld.dll:189798): GLib-GIO-WARNING **: 19:07:43.524: Your application does not implement g_application_activate() and has no handlers connected to the 'activate' signal.  It should do one of these.
```

When the app is being activated, you have to implement the activate method. You can do this with a injected C# callback with the help of ```Application.OnActivate```. Let's do this:

```cs 
    var app = Application.New("de.uriegel.first");  
    app.OnActivate(app => Console.WriteLine("App is being activated"));
    return app.Run(0, 0);
```     

When you debug the program, OnActivate is being called and returns immediately. When app.Run() is being executed, the injected callback is being called and the text is being displayed in the terminal. However, the app also stops immediately.
Of cource some kind of UI has to be created. 

Let's create a window, this has to be done in the Application.OnActivate callback:

```cs 
app.OnActivate(app =>
{
    var windows = app.NewWindow();
    windows.Show();
});
``` 
Now an empty default window is being shown and the function call ```Applicatio.Run()``` will only return when the window is being closed.

And now your first Gtk window is being shown!

## Hello World

For a Hello World app it is used to display the Text "Hello World". We set thewindow title to this string, and set the default size of the window, and our Hello World app is finished:

```cs
using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .OnActivate(app => app
        .NewWindow()
        .Title("Hello World👍")
        .DefaultSize(600, 200)
        .Show()
    ).Run();
```
Many Methods returns their own instance, so that you can chain function calls in a builder way. 

![custom titlebar](https://raw.githubusercontent.com/uriegel/Gtk4DotNet/refs/heads/Beta/Readme/helloworld.png) 

If you download the project from https://github.com/uriegel/Gtk4DotNet/ you can start the Test program 'HelloWorld' from Visual Studio Code.

## Including Widgets to the Window - Memory management

A Window can have a child widget, and widgets can also have children/a single child. 

Every widget is inherited from GObject. GObject uses reference counting as a mechanism for lifetime management. In Gtk4DotNet every GObject and inherited class implements IDisposable to unref a reference. But Gtk takes over lifetime management when a widget is member of a window hierarchy that is presented. Every widget such as ```Label```, ```Button``` or ```CheckButton``` is inherited from ```Widget```, and Widget has the property ```AutoDestroyed``` set to true. In this case ```Dispose()``` does nothing. 

So every widget implements IDisposable like the GObject base class, but memory management is in the hand of GTK. This means when you create a widget like a Button, and you don't add t to a window, the object is never being freed! But it makes no sense to create a widget and don't display it!

To check if all objects are being freed after the app has exited, there is a control mechanism. You can  enable it with the help of the method ```Application.WithDiagnostics()```. It should be the first method called on the application object: 

```cs
Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
    ...
```

If WithDiagnostics is called with parameter true, every object that is being freed will be logged in the console. Otherwise only the dangling objects are being displayed after the app has exited.

In the next test program 'PackButtons', three ```Button```s are being included in a ```Grid```, which is the child of the window. It is the transformation of the sample 'Packing Buttons' from the [GTK4 documentation](https://docs.gtk.org/gtk4/getting_started.html):

```cs
using Gtk4DotNet;
using CsTools.Extensions;

using static System.Console;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .Title("Pack👍")
        .Pipe(win => win.Child(
            Grid
                .New()
                .Attach(
                    Button
                        .NewWithLabel("Button 1")
                        .SideEffect(b => b.OnClicked += () => WriteLine("Button1 clicked")), 0, 0, 1, 1)
                .Attach(
                    Button
                        .NewWithLabel("Button 2")
                        .SideEffect(b => b.OnClicked += () => WriteLine("Button2 clicked")), 1, 0, 1, 1)
                .Attach(
                    Button
                        .NewWithLabel("Quit")
                        .SideEffect(b => b.OnClicked += () => win.CloseWindow()), 0, 1, 2, 1)))
        .Show()
  ).Run();
  ```
Gtk4DotNet has the nuget package CsTools included, which has some functional extensions like ```Pipe()``` or ```SideEffect()``` to be used in the functional flow of the builder. pattern.

## Using an UI template from .NET resource 

the same program with template Cambalache
Adwaita

### TODO


test app opening new custom windows inherited from Window, add to Application

