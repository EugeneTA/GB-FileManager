using System;


namespace FileManager
{
    /// <summary>
    /// Save application settings to the application settings file
    /// </summary>
    public class SaveToAppSettings : ISettingsWriter
    {
        public bool SaveSettings(AppData applicationSettings, IErrorLog errorLog)
        {
            try
            {
                // Сохраняем размеры окна
                if (applicationSettings != null)
                {
                    Properties.Settings.Default.WindowHeight = applicationSettings.AppDimensions.Height;
                    Properties.Settings.Default.WindowWidth = applicationSettings.AppDimensions.Width;
                    Properties.Settings.Default.LeftPanelPath = applicationSettings.leftFolderPath;
                    Properties.Settings.Default.RightPanelPath = applicationSettings.rightFolderPath;
                    Properties.Settings.Default.Save();
                    return true;
                }
            }
            catch (Exception e)
            {
                if (errorLog != null)
                {
                    errorLog.Log($"Ошибка при сохранении настроек приложения. {e.Message}\n");
                }
            }
            return false;
        }
    }
}
