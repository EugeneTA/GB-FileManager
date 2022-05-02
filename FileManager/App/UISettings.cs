
namespace FileManager
{
    /// <summary>
    /// Структура данных с параметрами графическго интерфейса (позиции элементов и их размеры)
    /// </summary>
    public class UISettings
    {
        // Параметры графицеской области
        public UIBase UIDimension { get; set; }

        // Параметры диалогового окна
        public UIBase DialogView { get; set; }

        // Параметры панели просмотра дерева каталогов
        public UIBase FolderView { get; set; }

        // Параметры смещения правой панели просмотра дерева каталогов
        public UIBase RightFolderOffset { get; set; }

        // Параметры информационной панели
        public UIBase InfoView { get; set; }

    }
}
