 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна информации о папке
    /// </summary>
    class DialogInfoTemplate : DialogData
    {
        public DialogInfoTemplate(FolderInfo info, bool showButtons)
        {
            Header = "Информация";
            InputData = null;

            Message = new List<string>
            {
                " ",
                $"Кол-во папок:  {info?.Dirs ?? 0}",
                $"Кол-во файлов: {info?.Files ?? 0}",
                $"Общий размер: {StringHelper.FileSizeToString(info?.Space ?? 0)}",
                " "
            };

            if (showButtons)
            {
                Buttons = ButtonFactory.GetButtons(ButtonType.Confirm);
            }
            else
            {
                Buttons = null;
            }

        }
    }
}
