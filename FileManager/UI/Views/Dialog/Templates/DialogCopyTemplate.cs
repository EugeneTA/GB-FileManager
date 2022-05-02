using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна копирования
    /// </summary>
    class DialogCopyTemplate : DialogData
    {
        public DialogCopyTemplate(string sourcePath, string destinationPath, bool showButtons)
        {
            Header = "Копирование";
            InputData = null;

            if (showButtons)
            {
                Message = new List<string> {
                                    " ",
                                    "Вы хотите скопировать",
                                    $"{sourcePath}",
                                    "в",
                                    $"{destinationPath} ?",
                                    " "
                            };
            }
            else
            {
                Message = new List<string> {
                                    " ",
                                    "Копирование",
                                    $"{sourcePath}",
                                    "в",
                                    $"{destinationPath}",
                                    " "
                            };
            }

            if (showButtons)
            {
                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.ConfirmCopy, ButtonType.Cancel }, ButtonType.Cancel);
            }
            else
            {
                Buttons = null;
            }

        }
    }
}

