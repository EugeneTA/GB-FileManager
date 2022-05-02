using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна перемименования объекта
    /// </summary>
    public class DialogRenameTemplate : DialogData
    {
        public DialogRenameTemplate(string editName, bool showButtons)
        {
            Header = "Переименование папки (файла)";

            if (showButtons)
            {
                InputData = editName;
            }
            else
            {
                InputData = "";
            }


            if (showButtons)
            {
                Message = new List<string> {
                                    " ",
                                    $"Введите название папки (файла):",
                                    " "
                            };
            }
            else
            {
                Message = new List<string> {
                                    " ",
                                    "Переименовываем папку (файл)",
                                    $"{editName}",
                                    " "
                            };
            }

            if (showButtons)
            {
                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.ConfirmRename, ButtonType.Cancel }, ButtonType.ConfirmRename);
            }
            else
            {
                Buttons = null;
            }

        }
    }
}


