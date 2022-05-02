namespace FileManager
{
    /// <summary>
    /// Save application settings interface
    /// </summary>
    public interface ISettingsWriter
    {
        /// <summary>
        /// Save application settings
        /// </summary>
        /// <param name="applicationSettings">application data</param>
        /// <param name="errorLog">error logger</param>
        /// <returns></returns>
        bool SaveSettings(AppData applicationSettings, IErrorLog errorLog);
    }
}
