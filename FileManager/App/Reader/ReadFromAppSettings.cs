using System;


namespace FileManager
{
    /// <summary>
    /// Read application settings from application settings file
    /// </summary>
    class ReadFromAppSettings : ISettingsReader
    {
        public AppData ReadSettings(AppData applicationSettings,  IErrorLog errorLog)
        {
            if (applicationSettings == null)
            {
                applicationSettings = new AppData();
            }

            applicationSettings.AppDimensions.Height = 
                Properties.Settings.Default.WindowHeight == 0 ? 
                Properties.Settings.Default.DefaultWindowHeight : 
                Properties.Settings.Default.WindowHeight;

            applicationSettings.AppDimensions.Width = 
                Properties.Settings.Default.WindowWidth == 0 ? 
                Properties.Settings.Default.DefaultWindowWidth : 
                Properties.Settings.Default.WindowWidth;

            applicationSettings.leftFolderPath = 
                string.IsNullOrWhiteSpace(Properties.Settings.Default.LeftPanelPath) ? 
                AppContext.BaseDirectory : 
                Properties.Settings.Default.LeftPanelPath;

            applicationSettings.rightFolderPath = 
                string.IsNullOrWhiteSpace(Properties.Settings.Default.RightPanelPath) ?
                AppContext.BaseDirectory : 
                Properties.Settings.Default.RightPanelPath;

            return applicationSettings;
        }
    }
}
