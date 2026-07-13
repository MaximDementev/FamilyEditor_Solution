using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Neuroptera.Contracts.PluginLogging;
using Neuroptera.Plugins.FamilyEditor.Constants;
using Neuroptera.Plugins.FamilyEditor.Services;
using Neuroptera.Revit.Contracts.PluginLogging;
using System;

namespace Neuroptera.Plugins.FamilyEditor.Commands
{
    // Команда для открытия папки с файлом семейства или шаблона
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class OpenFolderCommand : IExternalCommand
    {
        #region Поля

        private FileOperationService _fileService;

        #endregion

        #region IExternalCommand Implementation

        // Выполняет команду открытия папки
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var journal = PluginOperationJournal.Start(
                FamilyEditorOperations.PluginId,
                FamilyEditorOperations.OpenFolder,
                doc.Title);

            try
            {
                journal.Step("Запуск команды открытия папки");

                if (_fileService == null)
                    _fileService = new FileOperationService();

                journal.Step("Открытие папки с файлом документа");
                bool success = _fileService.OpenFileFolder(doc);

                if (!success)
                    return Result.Failed;

                journal.Complete("Папка с файлом открыта");
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                RevitPluginErrorHandling.Handle(ex, journal);
                return Result.Failed;
            }
        }

        #endregion
    }
}
