using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Задает стиль рисования рамки
    /// </summary>
    public class UIBoxStyle : UIStyle
    {
        // Символы для рисования рамок
        public char RectTopLeftCorner { get; set; } = '\u2554';
        public char RectTopIntresect { get; set; } = '\u2564';
        public char RectTopRightCorner { get; set; } = '\u2557';
        public char RectRightIntersect { get; set; } = '\u2563';
        public char RectBottomRightCorner { get; set; } = '\u255d';
        public char RectBottomIntersect { get; set; } = '\u2567';
        public char RectBottomLeftCorner { get; set; } = '\u255a';
        public char RectLeftIntersect { get; set; } = '\u2560';
        public char RectHorizontalLine { get; set; } = '\u2550';
        public char RectVerticalLine { get; set; } = '\u2551';
    }
}
