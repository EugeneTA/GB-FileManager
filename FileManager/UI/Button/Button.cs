using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Структура данных описывающая кнопку
    /// </summary>
    public class Button
    {
        public bool isVisible;          // Видна ли кнопка (показывать или нет)
        public bool isActive;           // Активна ли кнопка (выбрана)
        public string Name;             // Название кнопки
        public ButtonType ButtonType;   // Тип кнопки
        public ButtonBehavior ButtonBehavior; // Поведение при нажатии кнопки. Confirm - кнопка подтрверждает действие, Reject - отменяет действие
    }
}
