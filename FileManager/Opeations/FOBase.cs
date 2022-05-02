
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Абстрактный класс для описания операций над папками и файлами
    /// </summary>
    public abstract class FOBase
    {
        // Структура даных, необходима для выполнения операции.
        public FOData Data { get; set; }

        // Выполнение операции
        public abstract bool Execute();

        // Обработчик ошибок
        protected void ErrorHandler(List<string> message)
        {
            if (Data.ErrorLogger != null)
            {
                Data.ErrorLogger.Log(message);
            }

            Data.Dialog.Draw(new DialogErrorTemplate(message, true));
        }
    }
}
