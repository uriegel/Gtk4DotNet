#pragma once

#include <gtk/gtk.h>

G_BEGIN_DECLS

#define TGTK_TYPE_ASPECT_CONTAINER (tgtk_aspect_container_get_type())

typedef struct _TgtkAspectContainer TgtkAspectContainer;
typedef struct _TgtkAspectContainerClass TgtkAspectContainerClass;

struct _TgtkAspectContainerClass
{
    GtkWidgetClass parent_class;
};

GType tgtk_aspect_container_get_type(void);

GtkWidget *tgtk_aspect_container_new(void);


#define TGTK_ASPECT_CONTAINER(obj) \
    ((TgtkAspectContainer *)(obj))

void
tgtk_aspect_container_set_child(
    TgtkAspectContainer *self,
    GtkWidget *child);

enum
{
    PROP_0,
    PROP_ASPECT_RATIO,
    N_PROPERTIES
};

static GParamSpec *properties[N_PROPERTIES];    

G_END_DECLS