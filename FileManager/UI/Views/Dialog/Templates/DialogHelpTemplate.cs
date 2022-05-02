using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Шаблон для создания данных диалогового окна справки
    /// </summary>
    class DialogHelpTemplate : DialogData
    {
        public DialogHelpTemplate()
        {
            Header = "Справка";
            InputData = null;
            Message = new List<string> {
                                                " ",
                                                "Навигация по интерфейсу:",
                                                @"Стрелки - перемещение по каталогу внутри панели.",
                                                @"Enter   - перейти в выбранный каталог или запустить файл.",
                                                @"Tab     - для перехода между панелями.",
                                                @"Пробел  - вывод информации со свойствами каталога.",
                                                @" ",
                                                @"Атрибуты папок/файлов:",
                                                @"A - Archive",
                                                @"S - System",
                                                @"R - Read only",
                                                @"H - Hiden",
                                                @"E - Encrypted",
                                                " "
            };
            Buttons = ButtonFactory.GetButtons(ButtonType.Confirm);
        }
    }
}
