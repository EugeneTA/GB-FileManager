
namespace FileManager
{
    /// <summary>
    /// Типы кнопок
    /// </summary>
    public enum ButtonType
    {
        Default,
        Cancel,             // отмена
        Confirm,            // подтверждение
        ConfirmCopy,        // подтверждение копирования объекта
        ConfirmDelete,      // подтверждение удаления объекта
        ConfirmRename,      // подтверждение переименования объекта
        ConfirmMove,        // подтверждение перемещения объекта
        ConfirmCreate,      // подтверждение создания объекта
        ConfirmSearch,      // подтверждение поиска
        Replace,            // подтверждение замены объекта
        Skip                // пропустить
    }
}
