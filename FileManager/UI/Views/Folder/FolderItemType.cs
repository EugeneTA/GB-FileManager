

namespace FileManager
{
    /// <summary>
    /// Тип элемента файловой структуры 
    /// </summary>
    public enum FolderItemType
    {
        Empty,      // Неопределенный тип
        Return,     // Возврат в предыдущую папку
        Drive,      // Диск
        Folder,     // Папка
        File        // Файл
    }
}
