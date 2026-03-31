using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MagicEntry.Core.Models;
using MagicEntry.Core.Services;
using MagicEntry.Plugins.FamilyEditor.Constants;
using MagicEntry.Plugins.FamilyEditor.Services;
using MagicEntry.Plugins.FamilyEditor.Views;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MagicEntry.Plugins.FamilyEditor.Commands
{
    // Команда для пересохранения файлов семейств и шаблонов
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class BatchResaveCommand : IExternalCommand
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



                var uiApp = commandData.Application;

                var docs = GetValidDocuments(uiApp);

                if (!docs.Any())
                {
                    TaskDialog.Show("Info", "Нет подходящих документов");
                    return Result.Cancelled;
                }

                var window = new BatchResaveWindow(docs);
                if (window.ShowDialog() != true)
                    return Result.Cancelled;

                var selected = window.Result;

                var fileService = new FileOperationService();

                var success = new List<string>();
                var failed = new List<string>();

                foreach (var item in selected)
                {
                    try
                    {
                        if (fileService.ResaveDocument(item.Document, false))
                            success.Add(item.Name);
                        else
                            failed.Add(item.Name);
                    }
                    catch
                    {
                        failed.Add(item.Name);
                    }
                }

                ShowResult(success, failed);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                TaskDialog.Show(Messages.TITLE_ERROR,
                    $"{Messages.ERROR_EXECUTION}: {ex.Message}");
                return Result.Failed;
            }
        }


        private List<Document> GetValidDocuments(UIApplication app)
        {
            return app.Application.Documents
                .Cast<Document>()
                .Where(d =>
                    !d.IsLinked &&
                    !d.IsWorkshared &&
                    !string.IsNullOrEmpty(d.PathName))
                .ToList();
        }

        private void ShowResult(List<string> success, List<string> failed)
        {
            string msg =
                $"Успешно:\n{string.Join("\n", success)}\n\n" +
                $"Ошибки:\n{string.Join("\n", failed)}";

            TaskDialog.Show("Batch Resave", msg);
        }
    }
}