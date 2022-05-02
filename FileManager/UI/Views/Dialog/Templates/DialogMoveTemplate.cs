using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна перемещения объекта
    /// </summary>
    class DialogMoveTemplate : DialogData
    {
        public DialogMoveTemplate(string sourcePath, string destinationPath, bool showButtons)
        {
            Header = "Перемещение";
            InputData = null;

            if (showButtons)
            {
                Message = new List<string> {
                                    " ",
                                    "Вы хотите переместить",
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
                                    "Перемещение",
                                    $"{sourcePath}",
                                    "в",
                                    $"{destinationPath}",
                                    " "
                            };
            }

            if (showButtons)
            {
                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.ConfirmMove, ButtonType.Cancel }, ButtonType.Cancel);
            }
            else
            {
                Buttons = null;
            }

        }
    }
}
