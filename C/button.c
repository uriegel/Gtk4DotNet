#include <gtk/gtk.h>

G_DECLARE_FINAL_TYPE(CustomButton, custom_button, CUSTOM, BUTTON, GtkButton)
// GType custom_button_get_type(void) G_GNUC_CONST;
// typedef struct _CustomButton CustomButton;
// typedef struct _CustomButtonClass CustomButtonClass;
// G_DEFINE_AUTOPTR_CLEANUP_FUNC(CustomButton, g_object_unref)


//CustomButton *custom_button_new(void);
GtkWidget *custom_button_new(void);


struct _CustomButton {
    GtkButton parent_instance;
    guint click_count; // Custom field to store click count
};

G_DEFINE_TYPE(CustomButton, custom_button, GTK_TYPE_BUTTON)

// Callback when the button is clicked
//static void custom_button_clicked(GtkButton *button, gpointer user_data) {
static void custom_button_clicked(GtkButton *button) {
    CustomButton *self = CUSTOM_BUTTON(button);
    self->click_count++;
    
    char label[50];
    snprintf(label, sizeof(label), "Clicked %u times", self->click_count);
    gtk_button_set_label(button, label);
}

// Class initialization function
static void custom_button_class_init(CustomButtonClass *klass) {
    GtkWidgetClass *widget_class = GTK_WIDGET_CLASS(klass);
    GtkButtonClass *button_class = GTK_BUTTON_CLASS(klass);

    // Connect default click event
    button_class->clicked = custom_button_clicked;
}

// Instance initialization function
static void custom_button_init(CustomButton *self) {
    self->click_count = 0;
    gtk_button_set_label(GTK_BUTTON(self), "Click Me");
}

// Constructor function
//CustomButton *custom_button_new(void) {
GtkWidget *custom_button_new(void) {

    return (GtkWidget *)g_object_new(custom_button_get_type(), NULL);
}

static void activate(GtkApplication *app, gpointer user_data) {
    GtkWidget *window = gtk_application_window_new(app);
    gtk_window_set_title(GTK_WINDOW(window), "Custom Button Example");
    gtk_window_set_default_size(GTK_WINDOW(window), 300, 200);

    GtkWidget *button = custom_button_new();
    gtk_window_set_child(GTK_WINDOW(window), button);

    gtk_window_present(GTK_WINDOW(window));
}

int main(int argc, char **argv) {
    GtkApplication *app = gtk_application_new("com.example.CustomButton", G_APPLICATION_FLAGS_NONE);
    g_signal_connect(app, "activate", G_CALLBACK(activate), NULL);
    
    int status = g_application_run(G_APPLICATION(app), argc, argv);
    g_object_unref(app);
    
    return status;
}