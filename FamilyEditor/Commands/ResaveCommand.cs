using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MagicEntry.Core.Interfaces;
using MagicEntry.Core.Models;
using MagicEntry.Core.Services;
using MagicEntry.Plugins.FamilyEditor.Constants;
using MagicEntry.Plugins.FamilyEditor.Services;
using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace MagicEntry.Plugins.FamilyEditor.Commands
{
    // Команда для пересохранения файлов семейств и шаблонов
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ResaveCommand : IExternalCommand, IPlugin
    {
        #region Поля

        private FileOperationService _fileService;

        #endregion

        #region IPlugin Implementation

        public PluginInfo Info { get; set; }
        public bool IsEnabled { get; set; }

        // Инициализирует плагин при загрузке системы
        public bool Initialize()
        {
            try
            {
                var pathService = ServiceProvider.GetService<IPathService>();
                var initService = ServiceProvider.GetService<IPluginInitializationService>();

                if (pathService == null || initService == null)
                    return false;

                var pluginName = Info?.Name ?? AppConstants.PLUGIN_NAME;
                if (!initService.InitializePlugin(pluginName))
                    return false;

                var settingsPath = pathService.GetPluginUserDataFilePath(pluginName,
                    AppConstants.SETTINGS_FILE_NAME);
                if (!File.Exists(settingsPath))
                {
                    File.WriteAllText(settingsPath,
                        $"{AppConstants.PLUGIN_DISPLAY_NAME} Settings\nInitialized: {DateTime.Now}");
                }

                _fileService = new FileOperationService();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Освобождает ресурсы плагина
        public void Shutdown()
        {
            _fileService = null;
        }

        #endregion

        #region IExternalCommand Implementation

        // Выполняет команду пересохранения
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var pathService = ServiceProvider.GetService<IPathService>();
                var initService = ServiceProvider.GetService<IPluginInitializationService>();

                if (pathService == null || initService == null)
                {
                    TaskDialog.Show(Messages.TITLE_ERROR, Messages.ERROR_SERVICES_UNAVAILABLE);
                    return Result.Failed;
                }

                if (_fileService == null)
                    _fileService = new FileOperationService();

                Document doc = commandData.Application.ActiveUIDocument.Document;

                bool success = _fileService.ResaveDocument(doc);
                return success ? Result.Succeeded : Result.Failed;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TaskDialog.Show(Messages.TITLE_ERROR,
                    $"{Messages.ERROR_EXECUTION}: {ex.Message}");
                return Result.Failed;
            }
        }

        #endregion
    }
}
