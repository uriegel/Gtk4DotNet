#include <gtk/gtk.h>

#define MY_TYPE_SQUARE_BOX my_square_box_get_type()
G_DECLARE_FINAL_TYPE(MySquareBox, my_square_box, MY, SQUARE_BOX, GtkBox)

struct _MySquareBox {
    GtkBox parent_instance;
};

G_DEFINE_TYPE(MySquareBox, my_square_box, GTK_TYPE_BOX)

static void
my_square_box_measure(GtkWidget *widget,
                      GtkOrientation orientation,
                      int for_size,
                      int *minimum,
                      int *natural,
                      int *minimum_baseline,
                      int *natural_baseline)
{

    g_print("allocated width/height: %d x %d %d\n", orientation, minimum, natural);


    // Call the default measure first
    GTK_WIDGET_CLASS(my_square_box_parent_class)->measure(
        widget, orientation, for_size,
        minimum, natural, minimum_baseline, natural_baseline
    );

    // Get the other orientation's size too
    int min_other = 0, nat_other = 0;
    GTK_WIDGET_CLASS(my_square_box_parent_class)->measure(
        widget, orientation == GTK_ORIENTATION_HORIZONTAL ? GTK_ORIENTATION_VERTICAL : GTK_ORIENTATION_HORIZONTAL,
        for_size, &min_other, &nat_other, NULL, NULL);

    // Force both dimensions to be the maximum
    int min_size = MAX(*minimum, min_other);
    int nat_size = MAX(*natural, nat_other);

    *minimum = *natural = min_size;
    if (minimum_baseline) *minimum_baseline = -1;
    if (natural_baseline) *natural_baseline = -1;
}

static void my_square_box_init(MySquareBox *self)
{
}

static void my_square_box_class_init(MySquareBoxClass *klass)
{
    GtkWidgetClass *widget_class = GTK_WIDGET_CLASS(klass);
    widget_class->measure = my_square_box_measure;
    g_print("Bin da");
}

static void activate (GtkApplication *app, gpointer user_data)
{
    GtkWidget *window;
    GtkWidget *square_box;
    GtkWidget *label;

    GtkCssProvider *provider = gtk_css_provider_new();
    gtk_css_provider_load_from_string(provider, ".my-colored-box { background-color: #3498db; }");
    
    gtk_style_context_add_provider_for_display(
        gdk_display_get_default(),
        GTK_STYLE_PROVIDER(provider),
        GTK_STYLE_PROVIDER_PRIORITY_APPLICATION
    );    

    GtkWidget *box = gtk_box_new(GTK_ORIENTATION_VERTICAL, 0);

    window = gtk_application_window_new (app);
    gtk_window_set_title (GTK_WINDOW (window), "Hello");
    gtk_window_set_default_size (GTK_WINDOW (window), 200, 400);

//    square_box = gtk_box_new(GTK_ORIENTATION_VERTICAL, 0);
    square_box = g_object_new(MY_TYPE_SQUARE_BOX, "orientation", GTK_ORIENTATION_VERTICAL, NULL);


    gtk_widget_set_hexpand(box, TRUE);
    gtk_widget_set_vexpand(box, TRUE);
    gtk_widget_set_hexpand(square_box, FALSE);
    gtk_widget_set_vexpand(square_box, FALSE);    
    gtk_widget_add_css_class(GTK_WIDGET(square_box), "my-colored-box");
    label = gtk_label_new("I'm in a square box!");
    gtk_box_append(GTK_BOX(square_box), label);
    gtk_box_append(GTK_BOX(box), square_box);

    gtk_window_set_child (GTK_WINDOW (window), box);

    gtk_window_present (GTK_WINDOW (window));
}

int main (int    argc, char **argv)
{
    GtkApplication *app;
    int status;

    app = gtk_application_new ("org.gtk.example", G_APPLICATION_DEFAULT_FLAGS);
    g_signal_connect (app, "activate", G_CALLBACK (activate), NULL);
    status = g_application_run (G_APPLICATION (app), argc, argv);
    g_object_unref (app);

    return status;
}

/* 
gcc $(pkg-config --cflags gtk4) -o QuadraticBox QuadraticBox.c $(pkg-config --libs gtk4)
*/