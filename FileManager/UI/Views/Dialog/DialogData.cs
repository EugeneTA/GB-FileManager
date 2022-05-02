
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Структура данных для диалогового экна
    /// </summary>
    public class DialogData
    {
        // Название заголовка диалогового окна
        public string Header { get; set; }
        // Начальный текст для редактирования, если необходим ввод от пользователя
        public string InputData { get; set; }
        // Сообщение выводимое в диалоговом окне. Каждый элемент выводится на новой строке
        public List<string> Message { get; set; }
        // Список кнопок отображаемый в диалоговом окне
        public List<Button> Buttons { get; set; }
    }
}
