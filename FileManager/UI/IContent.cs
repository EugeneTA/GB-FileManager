using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Интрефейс обработки контента
    /// </summary>
    public interface IContent
    {
        // Вывод старого содержимого на экране
        void RefreshContent();
        // Получение нового содержимого и вывод его на экран
        void UpdateContent();
    }
}
