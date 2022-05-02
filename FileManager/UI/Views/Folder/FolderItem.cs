
using System;
using System.IO;

namespace FileManager
{
    /// <summary>
    /// Хранит информацию о элементе файловой структуры (имя, путь, тип)
    /// </summary>
    public class FolderItem : FolderItemBase
    {
        // Название
        public string Name { get; set; }
        // Путь
        public string Path { get; set; }
        // Размер
        public string Size { get; set; }
        // Аттриьуты
        public string Attr { get; set; }
        // Дата создания
        public string Date { get; set; }
        // Флаг, что элемент выбран в панели просмотра
        public bool IsActive { get; set; }
    }
}
