#include "aspectwidget.h"

struct _GtkAspectWidget
{
    GtkWidget parent_instance;

    GtkWidget *child;

    double aspect_ratio;
};

self->aspect_ratio = 1.0;

extern "C" void gtk_aspect_widget_measure(
    GtkWidget      *widget,
    GtkOrientation  orientation,
    int             for_size,
    int            *minimum,
    int            *natural,
    int            *minimum_baseline,
    int            *natural_baseline)
{
    GtkAspectWidget *self = GTK_ASPECT_WIDGET(widget);

    if (!self->child)
    {
        *minimum = 0;
        *natural = 0;
        return;
    }

    gtk_widget_measure(
        self->child,
        orientation,
        for_size,
        minimum,
        natural,
        minimum_baseline,
        natural_baseline);
}


extern "C" void gtk_aspect_widget_size_allocate(
    GtkWidget *widget,
    int width,
    int height,
    int baseline)
{
    GtkAspectWidget *self = GTK_ASPECT_WIDGET(widget);

    if (!self->child)
        return;

    double aspect = self->aspect_ratio;

    int child_width;
    int child_height;

    if ((double)width / height > aspect)
    {
        child_height = height;
        child_width = (int)(height * aspect + 0.5);
    }
    else
    {
        child_width = width;
        child_height = (int)(width / aspect + 0.5);
    }

    GskTransform *transform =
        gsk_transform_translate(
            NULL,
            &(graphene_point_t)
            {
                (width - child_width) / 2.0f,
                (height - child_height) / 2.0f
            });

    gtk_widget_allocate(
        self->child,
        child_width,
        child_height,
        baseline,
        transform);

    gsk_transform_unref(transform);
}

void gtk_aspect_widget_set_child(
    GtkAspectWidget *self,
    GtkWidget *child)
{
    g_return_if_fail(GTK_IS_ASPECT_WIDGET(self));

    if (self->child)
        gtk_widget_unparent(self->child);

    self->child = child;

    if (child)
        gtk_widget_set_parent(child, GTK_WIDGET(self));
}

void gtk_aspect_widget_set_aspect_ratio(
    GtkAspectWidget *self,
    double ratio)
{
    self->aspect_ratio = ratio;

    gtk_widget_queue_allocate(GTK_WIDGET(self));
}

static void gtk_aspect_widget_dispose( GObject *object)
{
    GtkAspectWidget *self =
        GTK_ASPECT_WIDGET(object);

    if (self->child)
    {
        gtk_widget_unparent(self->child);
        self->child = NULL;
    }

    G_OBJECT_CLASS(
        gtk_aspect_widget_parent_class)->dispose(object);
}

GtkWidgetClass *widget_class = GTK_WIDGET_CLASS(klass);

widget_class->measure = gtk_aspect_widget_measure;
widget_class->size_allocate = gtk_aspect_widget_size_allocate;

GObjectClass *object_class = G_OBJECT_CLASS(klass);

object_class->dispose = gtk_aspect_widget_dispose;

// g++ -shared -fPIC aspectwidget.cpp -o libtgtk4dotnet.so $(pkg-config --cflags --libs gtk4)