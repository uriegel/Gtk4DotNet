#pragma once

#include <gtk/gtk.h>

G_BEGIN_DECLS

#define GDN_TYPE_ASPECT_CONTAINER (gdn_aspect_container_get_type())

typedef struct _GdnAspectContainer GdnAspectContainer;
typedef struct _GdnAspectContainerClass GdnAspectContainerClass;

struct _GdnAspectContainerClass
{
    GtkWidgetClass parent_class;
};

GType gdn_aspect_container_get_type(void);

GtkWidget *gdn_aspect_container_new(void);

#define GDN_ASPECT_CONTAINER(obj) \
    ((GdnAspectContainer *)(obj))

void gdn_aspect_container_set_child(GdnAspectContainer *self, GtkWidget *child);

enum
{
    PROP_0,
    PROP_ASPECT_RATIO,
    N_PROPERTIES
};

static GParamSpec *properties[N_PROPERTIES];    

G_END_DECLS