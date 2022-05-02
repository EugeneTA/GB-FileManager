using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна вывода сообщения об ошибке
    /// </summary>
    public class DialogErrorTemplate : DialogData
    {
        public DialogErrorTemplate(List<string> message, bool showButtons)
        {
            Header = "Ошибка";
            InputData = null;
            Message = message;
 
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
