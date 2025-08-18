namespace MagicEntry.Plugins.FamilyEditor.Constants
{
    // Статический класс для хранения постоянных данных плагина
    public static class AppConstants
    {
        #region Настройки плагина
        public const string PLUGIN_NAME = "FamilyEditor";
        public const string PLUGIN_DISPLAY_NAME = "Редактор семейств";
        public const string SETTINGS_FILE_NAME = "settings.txt";
        #endregion

        #region Расширения файлов
        public const string FAMILY_EXTENSION = ".rfa";
        public const string TEMPLATE_EXTENSION = ".rte";
        #endregion

        #region Временные файлы
        public const string TEMP_FILE_PREFIX = "_TempFileTimeIs.";
        public const string DATE_FORMAT = "yy.MM.dd_HH.mm.ss";
        #endregion

        #region Задержки
        public const int FILE_OPERATION_DELAY = 2000;
        #endregion
    }
}
