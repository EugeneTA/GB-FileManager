using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс создания диалогового окна
    /// </summary>
    public class DialogViewFactory : UIFactory<UIDialogView>
    {
        Coordinates _coordinates;
        Dimensions _dimensions;
        int _lineWidth;

        public DialogViewFactory(Coordinates coordinates, Dimensions dimensions, int lineWidth)
        {
            _coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
            _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
            _lineWidth = lineWidth;
        }

        /// <summary>
        /// Создание диалогового окна
        /// </summary>
        /// <returns></returns>
        public override UIDialogView CreateView()
        {
            Coordinates position = _coordinates == null ? new Coordinates() : _coordinates;
            Dimensions size = _dimensions == null ? new Dimensions() : _dimensions;

            // Задаем стиль линии
            UILineStyle dialogLineStyle = new UILineStyle();
            
            // Задаем размер линии
            Dimensions dialogLineDimensions = new Dimensions(size.Width, _lineWidth);
            
            // Задаем координаты для линии отделяющей заголовок, от области сообщений
            Coordinates headerDividerPosition = new Coordinates(position.Left, position.Top + 2);
            
            // Задаем координаты для линии отжедляющей область кнопок, от области сообщений
            Coordinates buttonsDividerPosition = new Coordinates(position.Left, position.Top + size.Height - 5);
            
            // Создаем соответсвующие линии
            UILine headerDivider = new UILine(headerDividerPosition, dialogLineDimensions, dialogLineStyle);
            UILine buttonsDivider = new UILine(buttonsDividerPosition, dialogLineDimensions, dialogLineStyle);

            // Задаем стиль, размер и координаты рамки диалогового окна
            UIBoxStyle borderStyle = new UIBoxStyle();
            Dimensions borderSize = size;
            Coordinates borderPosition = position;
            
            // Создаем рамку
            UIBox border = new UIBox(borderPosition, borderSize, _lineWidth, borderStyle);

            // Создаем диалоговое окно
            return new UIDialogView(border, headerDivider, buttonsDivider);
        }

    }
}
