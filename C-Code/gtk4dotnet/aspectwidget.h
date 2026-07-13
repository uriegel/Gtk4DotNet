#pragma once

#include <gtk/gtk.h>

G_BEGIN_DECLS

#define GTK_TYPE_ASPECT_WIDGET (gtk_aspect_widget_get_type())

G_DECLARE_FINAL_TYPE(
    GtkAspectWidget,
    gtk_aspect_widget,
    GTK,
    ASPECT_WIDGET,
    GtkWidget)

GtkWidget *gtk_aspect_widget_new(void);

void gtk_aspect_widget_set_child(
    GtkAspectWidget *self,
    GtkWidget *child);

GtkWidget *gtk_aspect_widget_get_child(
    GtkAspectWidget *self);

void gtk_aspect_widget_set_aspect_ratio(
    GtkAspectWidget *self,
    double ratio);

double gtk_aspect_widget_get_aspect_ratio(
    GtkAspectWidget *self);

G_END_DECLS