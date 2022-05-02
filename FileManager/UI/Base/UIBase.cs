using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Базовый класс графического элемента
    /// </summary>
    public class UIBase
    {
        // Стартовые координаты элемента
        public Coordinates Position { get; set; }
        
        // Размеры элемента
        public Dimensions Size { get; set; }

        public UIBase(Coordinates position, Dimensions size)
        {
            Position = position;
            Size = size;
        }
    }
}
