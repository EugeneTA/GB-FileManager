
namespace FileManager
{
    /// <summary>
    /// Application settings reader interface
    /// </summary>
    public interface ISettingsReader
    {
        /// <summary>
        /// Read application settings
        /// </summary>
        /// <param name="applicationSettings">application data</param>
        /// <param name="errorLog">error logger</param>
        /// <returns></returns>
        AppData ReadSettings(AppData applicationSettings, IErrorLog errorLog);
    }
}
