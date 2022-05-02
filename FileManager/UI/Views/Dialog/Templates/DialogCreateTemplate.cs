using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна сощдания элемента
    /// </summary>
    public class DialogCreateTemplate : DialogData
    {
        public DialogCreateTemplate(string editName, bool showButtons)
        {
            Header = "Создание папки (файла)";

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
                                    "Создаем папку (файл)",
                                    $"{editName}",
                                    " "
                            };
            }

            if (showButtons)
            {
                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.ConfirmCreate, ButtonType.Cancel }, ButtonType.Cancel);
            }
            else
            {
                Buttons = null;
            }

        }
    }
}
