using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Стиль рисования линии
    /// </summary>
    public class UILineStyle : UIStyle
    {
        // Стиль горизонтальной линии
        public char LineHorizontalStart { get; set; } = '\u2560';
        public char LineHorizontalBody { get; set; } = '\u2550';
        public char LineHorizontalEnd { get; set; } = '\u2563';

        // Стиль вертикальной линии
        public char LineVerticalStart { get; set; } = '\u2564';
        public char LineVerticalBody { get; set; } = '\u2551';
        public char LineVerticalEnd { get; set; } = '\u2567';
    }
}
