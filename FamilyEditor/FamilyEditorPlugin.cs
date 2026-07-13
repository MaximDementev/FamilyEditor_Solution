using System.Reflection;
using Neuroptera.Contracts;
using Neuroptera.Revit.Contracts;
using Neuroptera.Revit.Contracts.Descriptors;
using Neuroptera.Revit.Contracts.PluginLogging;

namespace Neuroptera.Plugins.FamilyEditor;

public sealed class FamilyEditorPlugin : INeuropteraPlugin
{
    public PluginDefinition GetDefinition() =>
        new()
        {
            Commands = new[]
            {
                new PluginCommandDescriptor
                {
                    CommandKey = "Resave",
                    CommandTypeName = typeof(Commands.ResaveCommand).FullName!
                },
                new PluginCommandDescriptor
                {
                    CommandKey = "BatchResave",
                    CommandTypeName = typeof(Commands.BatchResaveCommand).FullName!
                },
                new PluginCommandDescriptor
                {
                    CommandKey = "OpenFolder",
                    CommandTypeName = typeof(Commands.OpenFolderCommand).FullName!
                }
            }
        };

    public void OnInitialized(IPluginContext context)
    {
        RevitPluginReporting.Configure(context, Assembly.GetExecutingAssembly());
        context.Logger.Info("FamilyEditor v{GetVersion()} Р·Р°РіСЂСѓР¶РµРЅ.");
    }

    public void OnShutdown(IPluginContext context)
    {
        context.Logger.Info("FamilyEditor РѕСЃС‚Р°РЅРѕРІР»РµРЅ.");
        RevitPluginReporting.Unregister(context);
    }

    internal static string GetVersion() =>
        Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.0.0";
}
