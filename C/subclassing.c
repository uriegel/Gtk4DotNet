#include <gtk/gtk.h>

#define EXAMPLE_APP_WINDOW_TYPE (example_app_window_get_type ())
G_DECLARE_FINAL_TYPE (ExampleAppWindow, example_app_window, EXAMPLE, APP_WINDOW, GtkApplicationWindow)


ExampleAppWindow       *example_app_window_new          (GtkApplication *app);


struct _ExampleAppWindow
{
  GtkApplicationWindow parent;
};

G_DEFINE_TYPE(ExampleAppWindow, example_app_window, GTK_TYPE_APPLICATION_WINDOW);

static void
example_app_window_init (ExampleAppWindow *app)
{
  gtk_window_set_title (GTK_WINDOW (app), "Hello Subclass 1");  
}

static void
example_app_window_class_init (ExampleAppWindowClass *class)
{
}

ExampleAppWindow *
example_app_window_new (GtkApplication *app)
{
  return g_object_new (EXAMPLE_APP_WINDOW_TYPE, "application", app, NULL);
}




/*
C#:
using System.Runtime.InteropServices;
using GtkDotNet;
using GtkDotNet.SafeHandles;

static class First
{
    public static int Run()
        => Application
            .New("de.uriegel.first")
            .OnActivate(app =>
                MachFenster(app)
                .Show())
            .Run(0, IntPtr.Zero);
            
        //var ret = Main();
    
    [DllImport("../C/first.so", EntryPoint = "mach_fenster")]
    static extern ApplicationWindowHandle MachFenster(ApplicationHandle app);    

    [DllImport("../C/first.so", EntryPoint = "main")]
    static extern int Main();    
        // => Application
    //     .New("de.uriegel.first")
    //         .OnActivate(app =>
    //             app
    //                 .NewWindow()
    //                     .Title("Hello Gtk👍")
    //                     .DefaultSize(600, 200)
    //                     .Show())
    //         .Run(0, IntPtr.Zero);
}

*/








static void
print_hello (GtkWidget *widget,
             gpointer   data)
{
  g_print ("Hello World\n");
}

ExampleAppWindow* mach_fenster(GtkApplication *app)
{
  return example_app_window_new (app);
}

static void
activate (GtkApplication *app,
          gpointer        user_data)
{
  ExampleAppWindow *win;

  win = mach_fenster (app);
  gtk_window_present (GTK_WINDOW (win));

  // GtkWidget *window;
  // GtkWidget *button;

  // window = gtk_application_window_new (app);
  // gtk_window_set_title (GTK_WINDOW (window), "Hello");
  // gtk_window_set_default_size (GTK_WINDOW (window), 200, 200);

  // button = gtk_button_new_with_label ("Hello World");
  // g_signal_connect (button, "clicked", G_CALLBACK (print_hello), NULL);
  // gtk_window_set_child (GTK_WINDOW (window), button);

  // gtk_window_present (GTK_WINDOW (window));
}

int add(int a, int b) {
  return a + b;
}

int
main ()
      
{
  GtkApplication *app;
  int status;
  char *affe = "./first";

  app = gtk_application_new ("org.gtk.example", G_APPLICATION_DEFAULT_FLAGS);
  g_signal_connect (app, "activate", G_CALLBACK (activate), NULL);
  status = g_application_run (G_APPLICATION (app), 1, &affe);
  g_object_unref (app);

  return status;
}

// gcc $(pkg-config --cflags gtk4) -o first first.c $(pkg-config --libs gtk4)
// gcc $(pkg-config --cflags gtk4)  -shared -o first.so -fPIC first.c $(pkg-config --libs gtk4)