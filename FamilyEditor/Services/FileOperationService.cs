using System;
using System.IO;
using System.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MagicEntry.Plugins.FamilyEditor.Constants;

namespace MagicEntry.Plugins.FamilyEditor.Services
{
    // Сервис для операций с файлами семейств и шаблонов
    public class FileOperationService
    {
        #region Методы пересохранения

        // Пересохраняет файл семейства или шаблона
        public bool ResaveDocument(Document doc, bool showMessage = true)
        {
            if (doc == null)
                return false;

            string originalPath = doc.PathName;

            if (string.IsNullOrEmpty(originalPath))
            {
                TaskDialog.Show(Messages.TITLE_ERROR, Messages.ERROR_DOCUMENT_NOT_SAVED);
                return false;
            }
            return ResaveFamilyDocument(doc, originalPath, showMessage);
        }

        // Пересохраняет документ семейства
        private bool ResaveFamilyDocument(Document doc, string originalPath, bool showMessage)
        {
            try
            {
                string tempPath = GenerateTempPath(originalPath);

                doc.SaveAs(tempPath);
                Thread.Sleep(AppConstants.FILE_OPERATION_DELAY);

                File.Delete(originalPath);
                doc.SaveAs(originalPath);
                File.Delete(tempPath);

                if(showMessage)
                TaskDialog.Show(Messages.TITLE_SUCCESS,
                    $"{Messages.SUCCESS_FAMILY_SAVED}\n{originalPath}");
                return true;
            }
            catch (Exception)
            {
                TaskDialog.Show(Messages.TITLE_ERROR, Messages.ERROR_SAVE_FAILED);
                return false;
            }
        }

        #endregion

        #region Методы работы с папками

        // Открывает папку с файлом
        public bool OpenFileFolder(Document doc)
        {
            if (doc == null)
                return false;

            string filePath = doc.PathName;

            if (string.IsNullOrEmpty(filePath))
            {
                TaskDialog.Show(Messages.TITLE_ERROR, Messages.ERROR_DOCUMENT_NOT_SAVED);
                return false;
            }

            try
            {
                string folderPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(folderPath))
                {
                    TaskDialog.Show(Messages.TITLE_ERROR, Messages.ERROR_FOLDER_NOT_FOUND);
                    return false;
                }

                System.Diagnostics.Process.Start("explorer.exe", folderPath);
                return true;
            }
            catch (Exception)
            {
                TaskDialog.Show(Messages.TITLE_ERROR, Messages.ERROR_FOLDER_NOT_FOUND);
                return false;
            }
        }

        #endregion

        #region Вспомогательные методы

        // Генерирует путь для временного файла
        private string GenerateTempPath(string originalPath)
        {
            int originalPathLength = originalPath.Length;
            string time = DateTime.Now.ToString(AppConstants.DATE_FORMAT);
            return originalPath.Insert(originalPathLength - 4,
                AppConstants.TEMP_FILE_PREFIX + time);
        }

        #endregion
    }
}
