#include <gtk/gtk.h>
#include <gio/gio.h>

static void
on_mount_done(GObject *source_object, GAsyncResult *res, gpointer user_data)
{
    GApplication *app = G_APPLICATION(user_data);
    GError *error = NULL;

    if (!g_volume_mount_finish(G_VOLUME(source_object), res, &error)) {
        g_printerr("Mount failed: %s\n", error->message);
        g_error_free(error);
    } else {
        g_print("Mount succeeded!\n");
    }

    g_application_quit(app);
}

static void
on_ask_password(GMountOperation *op,
                const gchar *message,
                const gchar *default_user,
                const gchar *default_domain,
                GMountOperationResult *result,
                gpointer user_data)
{
    g_print("Password required: %s\n", message);
    g_mount_operation_set_username(op, default_user);
    g_mount_operation_set_password(op, "your-password-here");
    g_mount_operation_reply(op, G_MOUNT_OPERATION_HANDLED);
}

static void
on_activate(GtkApplication *app, gpointer user_data)
{
    GVolumeMonitor *monitor = g_volume_monitor_get();
    GList *volumes = g_volume_monitor_get_volumes(monitor);

    for (GList *l = volumes; l != NULL; l = l->next) {
        GVolume *volume = G_VOLUME(l->data);

        if (g_volume_can_mount(volume)) {
            g_print("Found mountable volume: %s\n", g_volume_get_name(volume));

            GMountOperation *op = g_mount_operation_new();
            // TODO g_signal_connect(op, "ask-password", G_CALLBACK(on_ask_password), NULL);

            g_volume_mount(
                volume,
                G_MOUNT_MOUNT_NONE,
                op,
                NULL,
                on_mount_done,
                app); // pass app so we can quit later

            g_object_unref(op);
            break;
        }
    }

    g_list_free_full(volumes, g_object_unref);
}

int main(int argc, char *argv[])
{
    GtkApplication *app = gtk_application_new("com.example.MountDemo", G_APPLICATION_FLAGS_NONE);
    g_signal_connect(app, "activate", G_CALLBACK(on_activate), NULL);
    int status = g_application_run(G_APPLICATION(app), argc, argv);
    g_object_unref(app);
    return status;
}
