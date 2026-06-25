using System.Reflection;
using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class GSettings : GObject
{
    #region Properties

    public string SchemaId { get; private set; } = "";

    public GSettingsValue this[string index]
    {
        get => values.TryGetValue(index, out var val)
            ? val
            : new GSettingsValue(index, this).SideEffect(sv => values.TryAdd(index, sv));
    }

    #endregion

    #region Construction

    public static GSettings New(string schemaId, bool dontCheckDiagnostics = false)
    {
        var settings = _New(schemaId);
        settings.SchemaId = schemaId;
        if (!dontCheckDiagnostics)
            settings.CheckDiagnostics();
        return settings;
    }

    public static GSettings NewFromResource(string applicationId, bool dontCheckDiagnostics = false)
    {
        var schemasCompiled = Environment
            .GetFolderPath(Environment.SpecialFolder.ApplicationData)
            .AppendPath(applicationId)
            .SideEffect(d => d.EnsureDirectoryExists());
        var stream = Assembly.GetEntryAssembly()!.GetManifestResourceStream("gschemas") ?? throw new Exception("There is no compiled schema included");
        using (var gschemaFile = File.OpenWrite(schemasCompiled.AppendPath("gschemas.compiled")))
            stream.CopyTo(gschemaFile);
        using var source = SettingsSchemaSource.FromDirectory(schemasCompiled);
        using var schema = source.Lookup(applicationId, true) ?? throw new Exception("Settings Schema not found");
        var settings = schema.NewSettings();
        settings.SchemaId = applicationId;
        if (!dontCheckDiagnostics)
            settings.CheckDiagnostics();
        return settings;
    }

    #endregion

    #region Methods

    public void Bind(string key, GObject obj, string property, BindFlags flags = BindFlags.Default) => Bind(this, key, obj, property, flags);

    public bool SchemaHasKey(string key)
    {
        var schema = GetSchema(GetDefaultSchema(), SchemaId, true);
        if (schema == 0)
            return false;
        var result = SchemaHasKey(schema, key);
        SchemaUnref(schema);
        return result;
    }

    public GSettings? ValidateKey(string key)
        => SchemaHasKey(key) ? this : null;

    public new string? GetString(string key)
        => GetString(this, key).PtrToString(true);

    public new bool SetString(string key, string value)
        => SetString(this, key, value);

    public new bool GetBool(string key) => GetBool(this, key);

    public new bool SetBool(string key, bool value) => SetBool(this, key, value);

    public SettingsAction CreateAction(string key, string? accelerator = null)
    {
        var res = CreateAction(this, key);
        res.CheckDiagnostics();
        return new(key, res, accelerator);
    }

    public int? GetInt(string key) => GetInt(this, key);

    public bool SetInt(string key, int value) => SetInt(this, key, value);

    #endregion

    #region Class

    public class GSettingsValue
    {
        public string Key { get; }
        public event Action OnChanged
        {
            add
            {
                var id = settings.SignalConnect($"changed::{Key}", value, true);
                delegates.TryAdd(value, id);
            }
            remove
            {
                if (delegates.TryGetValue(value, out var id))
                    settings.SignalDisconnect(id);
            }
        }

        internal GSettingsValue(string key, GSettings settings)
        {
            Key = key;
            this.settings = settings;
        }

        Dictionary<Delegate, DelegateId> delegates = [];

        GSettings settings;
    }

    #endregion

    #region Internals

    Dictionary<string, GSettingsValue> values = [];

    #endregion

    #region PInvoke

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_bind", CallingConvention = CallingConvention.Cdecl)]
    extern static void Bind(GSettings settings, string key, GObject obj, string property, BindFlags flags);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_new", CallingConvention = CallingConvention.Cdecl)]
    extern static GSettings _New(string schemaId);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_has_key", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SchemaHasKey(nint schema, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_source_get_default", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetDefaultSchema();

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_source_lookup", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetSchema(nint schema, string id, bool recursive);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_schema_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void SchemaUnref(nint schema);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_create_action", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle CreateAction(GSettings settings, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetString(GSettings settings, string key);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SetString(GSettings settings, string key, string value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_set_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SetBool(GSettings settings, string name, bool value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetBool(GSettings settings, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_set_int", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SetInt(GSettings settings, string name, int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_settings_get_int", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetInt(GSettings settings, string name);
    
    #endregion
}

