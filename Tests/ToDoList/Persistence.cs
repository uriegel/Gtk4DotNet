using CsTools;
using CsTools.Extensions;
using Gtk4DotNet;

static class Persistence
{
    public static TaskItem[] Retrieve()
        => GetPath()
            .ReadAllTextFromFilePath()
            ?.Deserialize<TaskItem[]>(Json.Defaults) 
            ?? [];

    public static void Save(IEnumerable<TaskItem> items)
        => GetPath().WriteAllTextToFilePath(items.Serialize(Json.Defaults));

    static string GetPath()
        => Environment
            .GetFolderPath(Environment.SpecialFolder.ApplicationData)
            .AppendPath(Application.ApplicationId)
            .SideEffect(d => d.EnsureDirectoryExists())
            .AppendPath("todolist.json");
}