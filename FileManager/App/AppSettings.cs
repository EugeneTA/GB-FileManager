

namespace FileManager
{

    /// <summary>
    /// Application settings control class. Used to save and read application settings
    /// </summary>
    public class AppSettings
    {
        // Application settings
        public AppData Settings { set; get; }
        // Error logger
        public IErrorLog ErrorLoger;
        // Default application settings reader
        private readonly ISettingsReader _settingsReader;
        // Default application settins writer
        private readonly ISettingsWriter _settingsWriter;

        public AppSettings() : this(
            new ErrorLogToFile() { FileName = "errorlog.txt" }, 
            new AppData(), 
            new ReadFromAppSettings(),
            new SaveToAppSettings())
        {

        }
        public AppSettings(IErrorLog errorLogger, AppData settings, ISettingsReader settingsReader, ISettingsWriter settingsWriter)
        {
            Settings = settings;
            ErrorLoger = errorLogger;
            _settingsReader = settingsReader;
            _settingsWriter = settingsWriter;
        }

        /// <summary>
        ///  Save application settings using default settings writer
        /// </summary>
        public void SaveSettings()
        {
            SaveSettings(_settingsWriter);
        }

        /// <summary>
        /// Save application settings using defined settings writer
        /// </summary>
        /// <param name="saveSettings">settihgs writer</param>
        public void SaveSettings(ISettingsWriter saveSettings)
        {
            if (saveSettings != null)
            {
                saveSettings.SaveSettings(Settings, ErrorLoger);
            }
        }

        /// <summary>
        /// Save application settings using default settings reader
        /// </summary>
        /// <returns></returns>
        public AppData ReadSettings()
        {
            return ReadSettings(_settingsReader);

        }

        /// <summary>
        /// Save application settings using defined settings reader
        /// </summary>
        /// <param name="readSettings">settings reader</param>
        /// <returns></returns>
        public AppData ReadSettings(ISettingsReader readSettings)
        {
            if (readSettings != null)
            {
                readSettings.ReadSettings(Settings, ErrorLoger);
            }
            return Settings;
        }
    }
}
