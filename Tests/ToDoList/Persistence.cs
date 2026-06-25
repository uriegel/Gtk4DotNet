using CsTools;
using CsTools.Extensions;

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
            .AppendPath(Globals.ApplicationId)
            .SideEffect(d => d.EnsureDirectoryExists())
            .AppendPath("todolist.json");
}