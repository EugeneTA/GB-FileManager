using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Интерфейс обработки консольных коротких команд пользователя
    /// </summary>
    public interface IConsoleCommand
    {
       // Обработка полученной команды из консоли от пользователя
        void CommandExecute(ConsoleKey key);
    }
}
