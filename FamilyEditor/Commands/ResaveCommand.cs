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
    // Команда для пересохранения файлов семейств и шаблонов
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ResaveCommand : IExternalCommand
    {
        #region Поля

        private FileOperationService _fileService;

        #endregion

        #region IExternalCommand Implementation

        // Выполняет команду пересохранения
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var journal = PluginOperationJournal.Start(
                FamilyEditorOperations.PluginId,
                FamilyEditorOperations.Resave,
                doc.Title);

            try
            {
                journal.Step("Запуск команды пересохранения");

                if (_fileService == null)
                    _fileService = new FileOperationService();

                journal.Step("Пересохранение документа");
                bool success = _fileService.ResaveDocument(doc);

                if (!success)
                    return Result.Failed;

                journal.Complete("Документ пересохранён");
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
