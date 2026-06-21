namespace Gtk4DotNet;

[Flags]
public enum ApplicationFlags
{
    /// <summary>
    /// Default flags
    /// </summary>
    None,
    /// <summary>
    /// Run as a service. In this mode, registration fails if the service is already running, and the application 
    /// will initially wait up to 10 seconds for an initial activation message to arrive.
    /// </summary>
    IsService,
    /// <summary>
    /// Don’t try to become the primary instance.
    /// </summary>
    IsLauncher,
    /// <summary>
    /// This application handles opening files (in the primary instance). Note that this flag only affects the default implementation
    /// of local_command_line(). It can be useful even when using G_APPLICATION_HANDLES_COMMAND_LINE to handle org.freedesktop.Application.open. See g_application_run() for details.
    /// </summary>
    HandlesOpen = 4,
    /// <summary>
    /// This application handles command line arguments (in the primary instance). Note that this flag only affect the default implementation
    /// of local_command_line(). See g_application_run() for details.
    /// </summary>
    HandlesCommandLine = 8,
    /// <summary>
    /// Send the environment of the launching process to the primary instance. 
    /// Set this flag if your application is expected to behave differently depending on certain environment variables. 
    /// For instance, an editor might be expected to use the GIT_COMMITTER_NAME environment variable when editing a 
    /// git commit message. The environment is available to the GApplication::command-line signal handler, via g_application_command_line_getenv().
    /// </summary>
    SendEnvironment = 16,
    /// <summary>
    /// Make no attempts to do any of the typical single-instance application negotiation, even if the application ID is given. 
    /// The application neither attempts to become the owner of the application ID nor does it check if an existing owner already exists. 
    /// Everything occurs in the local process.
    /// </summary>
    NonUnique = 32,
    /// <summary>
    /// Allow users to override the application ID from the command line with --gapplication-app-id. 
    /// </summary>
    CanOverrideAppId = 64,
    /// <summary>
    /// Allow another instance to take over the bus name.
    /// </summary>
    AllowReplacemant = 128,
    /// <summary>
    /// Take over from another instance. This flag is usually set by passing --gapplication-replace on the commandline. 
    /// </summary>
    Replace = 256
}