
namespace FileManager
{
    /// <summary>
    /// Contains app related data
    /// </summary>
    public class AppData
    {
        // Application window dimension
        public Dimensions AppDimensions { get; set; } = new Dimensions();

        // Folder path for the left and right folder views
        public string leftFolderPath;
        public string rightFolderPath;

    }
}
