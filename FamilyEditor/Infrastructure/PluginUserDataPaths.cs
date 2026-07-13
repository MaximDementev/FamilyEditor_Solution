using System;
using System.IO;

namespace Neuroptera.Plugins.FamilyEditor.Infrastructure;

public static class PluginUserDataPaths
{
    public static string GetDirectory(string pluginId)
    {
        var root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Neuroptera",
            "PluginData",
            pluginId);
        Directory.CreateDirectory(root);
        return root;
    }

    public static string GetFilePath(string pluginId, string fileName) =>
        Path.Combine(GetDirectory(pluginId), fileName);

}
