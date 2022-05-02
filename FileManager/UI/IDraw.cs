using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Интерфейс рисования элемента
    /// </summary>
    public interface IDraw
    {
        // Рисование элемента в консоли
        bool Draw();
    }
}
