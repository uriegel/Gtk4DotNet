#include "tgtk_aspect_container.h"

struct _TgtkAspectContainer
{
    GtkWidget parent_instance;

    double aspect_ratio;
    GtkWidget *child;
};

static void
tgtk_aspect_container_buildable_init(GtkBuildableIface *iface);

G_DEFINE_TYPE_WITH_CODE(
    TgtkAspectContainer,
    tgtk_aspect_container,
    GTK_TYPE_WIDGET,
    G_IMPLEMENT_INTERFACE(
        GTK_TYPE_BUILDABLE,
        tgtk_aspect_container_buildable_init))


static void
tgtk_aspect_container_init(
    TgtkAspectContainer *self)
{
    self->aspect_ratio = 1.0 / 1.0;
    self->child = NULL;
}


static void
tgtk_aspect_container_measure(
    GtkWidget *widget,
    GtkOrientation orientation,
    int for_size,
    int *minimum,
    int *natural,
    int *minimum_baseline,
    int *natural_baseline)
{
    *minimum = 100;
    *natural = 400;

    *minimum_baseline = -1;
    *natural_baseline = -1;
}

static void
tgtk_aspect_container_size_allocate(
    GtkWidget *widget,
    int width,
    int height,
    int baseline)
{
    TgtkAspectContainer *self =
        TGTK_ASPECT_CONTAINER(widget);

    if (self->child == NULL)
        return;

    double ratio = self->aspect_ratio;

    int child_width;
    int child_height;

    if ((double)width / height > ratio)
    {
        // too wide: limit by height
        child_height = height;
        child_width = (int)(height * ratio);
    }
    else
    {
        // too high: limit by width
        child_width = width;
        child_height = (int)(width / ratio);
    }


    int x = (width - child_width) / 2;
    int y = (height - child_height) / 2;


    GtkAllocation allocation =
    {
        .x = x,
        .y = y,
        .width = child_width,
        .height = child_height
    };


    gtk_widget_size_allocate(
        self->child,
        &allocation,
        baseline);
}

static void
tgtk_aspect_container_set_property(
    GObject *object,
    guint property_id,
    const GValue *value,
    GParamSpec *pspec)
{
    TgtkAspectContainer *self =
        TGTK_ASPECT_CONTAINER(object);

    switch (property_id)
    {
        case PROP_ASPECT_RATIO:
            self->aspect_ratio =
                g_value_get_double(value);

            gtk_widget_queue_resize(
                GTK_WIDGET(self));
            break;

        default:
            G_OBJECT_WARN_INVALID_PROPERTY_ID(
                object,
                property_id,
                pspec);
    }
}


static void
tgtk_aspect_container_get_property(
    GObject *object,
    guint property_id,
    GValue *value,
    GParamSpec *pspec)
{
    TgtkAspectContainer *self =
        TGTK_ASPECT_CONTAINER(object);

    switch (property_id)
    {
        case PROP_ASPECT_RATIO:
            g_value_set_double(
                value,
                self->aspect_ratio);
            break;

        default:
            G_OBJECT_WARN_INVALID_PROPERTY_ID(
                object,
                property_id,
                pspec);
    }
}


static void
tgtk_aspect_container_dispose(
    GObject *object)
{
    TgtkAspectContainer *self =
        TGTK_ASPECT_CONTAINER(object);

    if (self->child != NULL)
    {
        gtk_widget_unparent(self->child);
        self->child = NULL;
    }

    G_OBJECT_CLASS(tgtk_aspect_container_parent_class)->dispose(object);    
}

static void
tgtk_aspect_container_class_init(
    TgtkAspectContainerClass *klass)
{
    GtkWidgetClass *widget_class =
        GTK_WIDGET_CLASS(klass);

    widget_class->measure =
        tgtk_aspect_container_measure;

    widget_class->size_allocate =
        tgtk_aspect_container_size_allocate;        

GObjectClass *object_class =
        G_OBJECT_CLASS(klass);

        object_class->dispose =
        tgtk_aspect_container_dispose;

    object_class->set_property =
        tgtk_aspect_container_set_property;

    object_class->get_property =
        tgtk_aspect_container_get_property;


    properties[PROP_ASPECT_RATIO] =
        g_param_spec_double(
            "aspect-ratio",
            "Aspect ratio",
            "Width / height ratio",
            0.1,
            10.0,
            16.0 / 9.0,
            G_PARAM_READWRITE |
            G_PARAM_EXPLICIT_NOTIFY);


    g_object_class_install_properties(
        object_class,
        N_PROPERTIES,
        properties);        
}


GtkWidget *
tgtk_aspect_container_new(void)
{
    return g_object_new(
        TGTK_TYPE_ASPECT_CONTAINER,
        NULL);
}

void
tgtk_aspect_container_set_child(
    TgtkAspectContainer *self,
    GtkWidget *child)
{
    if (self->child)
    {
        gtk_widget_unparent(self->child);
        self->child = NULL;
    }

    if (child)
    {
        self->child = child;

        gtk_widget_set_parent(
            child,
            GTK_WIDGET(self));
    }

    gtk_widget_queue_resize(
        GTK_WIDGET(self));
}





static void
tgtk_aspect_container_buildable_add_child(
    GtkBuildable *buildable,
    GtkBuilder *builder,
    GObject *child,
    const char *type)
{
    TgtkAspectContainer *self =
        TGTK_ASPECT_CONTAINER(buildable);

    if (GTK_IS_WIDGET(child))
    {
        self->child = GTK_WIDGET(child);

        gtk_widget_set_parent(
            self->child,
            GTK_WIDGET(self));
    }
}

static void
tgtk_aspect_container_buildable_init(
    GtkBuildableIface *iface)
{
    iface->add_child =
        tgtk_aspect_container_buildable_add_child;
}

void tgtk_aspect_container_set_aspect_ratio(TgtkAspectContainer *self, double ratio)
{
    self->aspect_ratio = ratio;
}
