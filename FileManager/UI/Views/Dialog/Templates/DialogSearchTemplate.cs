using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна поиска
    /// </summary>
    public class DialogSearchTemplate : DialogData
    {
        public DialogSearchTemplate(string editName, bool showButtons)
        {
            Header = "Поиск папки (файла)";

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
                                    "Что искать?",
                                    " ",
                                    "( * - заменяет любое количество символов)",
                                    "( без * ищет четкое соответствие введенной строке)"
                            };
            }
            else
            {
                Message = new List<string> {
                                    " ",
                                    $"Идет поиск \'{editName}\'",
                                    "в названиях файлов и папок",
                                    " ",
                                    " ",
                                    " ",
                                    " ",
                                    " ",
                                    " "
                            };
            }

            if (showButtons)
            {
                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.ConfirmSearch, ButtonType.Cancel }, ButtonType.ConfirmSearch);
            }
            else
            {
                Buttons = null;
            }

        }
    }
}

