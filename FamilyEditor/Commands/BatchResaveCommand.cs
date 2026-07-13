using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Neuroptera.Contracts.PluginLogging;
using Neuroptera.Plugins.FamilyEditor.Constants;
using Neuroptera.Plugins.FamilyEditor.Services;
using Neuroptera.Plugins.FamilyEditor.Views;
using Neuroptera.Revit.Contracts.PluginLogging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuroptera.Plugins.FamilyEditor.Commands
{
    // Команда для пересохранения файлов семейств и шаблонов
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class BatchResaveCommand : IExternalCommand
    {
        #region Поля

        private FileOperationService _fileService;

        #endregion

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var doc = uiApp.ActiveUIDocument.Document;
            var journal = PluginOperationJournal.Start(
                FamilyEditorOperations.PluginId,
                FamilyEditorOperations.BatchResave,
                doc.Title);

            try
            {
                journal.Step("Запуск пакетного пересохранения");

                if (_fileService == null)
                    _fileService = new FileOperationService();

                journal.Step("Получение списка документов");
                var docs = GetValidDocuments(uiApp);

                if (!docs.Any())
                {
                    RevitPluginErrorHandling.ShowValidation(
                        "Нет подходящих документов",
                        "Откройте семейства или шаблоны для пересохранения.",
                        FamilyEditorOperations.PluginId,
                        FamilyEditorOperations.BatchResave,
                        doc);
                    return Result.Cancelled;
                }

                journal.Step("Отображение окна выбора документов");
                var window = new BatchResaveWindow(docs);
                if (window.ShowDialog() != true)
                    return Result.Cancelled;

                var selected = window.Result;
                var mode = window.Mode;

                journal.Step("Пакетная обработка документов", selected.Count.ToString());
                var fileService = new FileOperationService();

                var success = new List<string>();
                var failed = new List<string>();

                foreach (var item in selected)
                {
                    try
                    {
                        bool result;

                        if (mode == SaveMode.Save)
                        {
                            result = fileService.SaveDocument(item.Document, false);
                        }
                        else if (mode == SaveMode.Resave)
                        {
                            result = fileService.ResaveDocument(item.Document, false);
                        }
                        else
                        {
                            result = false;
                        }

                        if (result)
                            success.Add(item.Name);
                        else
                            failed.Add(item.Name);
                    }
                    catch
                    {
                        failed.Add(item.Name);
                    }
                }

                journal.Step("Отображение результатов");
                ShowResult(success, failed);

                journal.Complete($"Обработано: {success.Count} успешно, {failed.Count} с ошибками");
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                RevitPluginErrorHandling.Handle(ex, journal);
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
