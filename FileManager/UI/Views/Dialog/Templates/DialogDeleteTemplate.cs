using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна удаления элемента
    /// </summary>
    class DialogDeleteTemplate : DialogData
    {
        public DialogDeleteTemplate(string sourcePath, bool showButtons)
        {
            Header = "Удаление";
            InputData = null;

            if (showButtons)
            {
                Message = new List<string> {
                                    " ",
                                    "Вы хотите удалить",
                                    $"{sourcePath} ?",
                                    " "
                            };
            }
            else
            {
                Message = new List<string> {
                                    " ",
                                    "Удаление",
                                    $"{sourcePath}",
                                    " "
                            };
            }

            if (showButtons)
            {
                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.ConfirmDelete, ButtonType.Cancel }, ButtonType.Cancel);
            }
            else
            {
                Buttons = null;
            }

        }
    }
}
