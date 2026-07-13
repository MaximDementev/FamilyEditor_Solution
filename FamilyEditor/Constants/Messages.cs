namespace Neuroptera.Plugins.FamilyEditor.Constants
{
    // Статический класс для хранения всех сообщений плагина
    public static class Messages
    {
        #region Сообщения об ошибках
        public const string ERROR_DOCUMENT_NOT_SAVED = "Документ нигде не сохранен.\nСохраните его вручную";
        public const string ERROR_SAVE_FAILED = "Не получилось сохранить документ по адресу.\nСохраните его вручную";
        public const string ERROR_DELETE_FAILED = "Не получается завершить процесс.\nПроверьте место сохранения вручную. Возможно, потребуется пересохранить файл вручную";
        public const string ERROR_SERVICES_UNAVAILABLE = "Сервисы Neuroptera недоступны";
        public const string ERROR_FOLDER_NOT_FOUND = "Папка с файлом не найдена";
        public const string ERROR_EXECUTION = "Ошибка при выполнении плагина";
        #endregion

        #region Сообщения об успехе
        public const string SUCCESS_FAMILY_SAVED = "Файл сохранен по адресу";
        public const string SUCCESS_FOLDER_OPENED = "Папка открыта";
        #endregion

        #region Заголовки диалогов
        public const string TITLE_ERROR = "Ошибка";
        public const string TITLE_SUCCESS = "Успешно";
        public const string TITLE_INFO = "Информация";
        #endregion
    }
}
