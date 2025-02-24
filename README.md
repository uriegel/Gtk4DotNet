# Gtk4DotNet
A C# wrapper for GTK4 (.NET 8). You can create programs using the GTK4 UI system as a .NET 8.

Gtk4DotNet uses a functional declarative approach to GTK4 similar to REACT or Kotlin Compose:

```
return Application
    .New("org.gtk.example")
    .OnActivate(app => 
        app
            .NewWindow()
            .Title("Hello Gtk👍")
            .SideEffect(win => win
            .Child(
                Grid
                    .New()
                    .Attach(                                
                        Button
                            .NewWithLabel("Button 1")
                            .OnClicked(() => WriteLine("Button1 clicked")), 
                        0, 0, 1, 1)
                    .Attach(                                
                        Button
                            .NewWithLabel("Button 2")
                            .OnClicked(() => WriteLine("Button2 clicked")), 
                        1, 0, 1, 1)
                    .Attach(                                
                        Button
                            .NewWithLabel("Quit")
                            .OnClicked(() => win.CloseWindow()), 
                        0, 1, 2, 1)))
            .Show())
    .Run(0, 0);
}

```
# Table of contents 
1. [Prerequisites](#prerequisites)
2. [Hello World (a minimal GTK4 app)](#helloworld)

## Prerequisites <a name="prerequisites"></a>

### Necessary only depending on the version of Linux

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
### The necessary Gtk4DotNet Nuget package <a name="nuget"></a>

To use these features there is a nuget package  [Gtk4DotNet](https://www.nuget.org/packages/Gtk4DotNet/), which you have to imclude. 


## Hello World (a minimal GTK4 app) <a name="helloworld"></a>

// TODO var app = new Application
// TODO app.run()

// TODO explain warning

// TODO onActivate

// TODO create a Window

// TODO show image

// TODO Now complete Hello World



## DEPRECATED Part




Contained in this Repo are samples how to use Gtk4DotNet. All examples of the official GTK4 are transformed to C# with Gtk4DotNet.


### If you want to use GTK resources

* sudo apt install libglib2.0-dev-bin

## Installation of GTK Schema
```
    sudo install -D ./Test/org.gtk.example.gschema.xml /usr/share/glib-2.0/schemas/
    sudo glib-compile-schemas /usr/share/glib-2.0/schemas/
```     
## Usage

Look at the sample programs (https://github.com/uriegel/Gtk4DotNet/tree/Main/Test)

## Checking if memory is being freed
To check if GObjects are being freed, just run
```
Widget.AddWeakRef(() => Console.WriteLine("... is being freed));
```
If this object is finalized, then the callback will be called.

