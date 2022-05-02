using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Графический примитив - рамка
    /// </summary>
    public class UIBox : UIBase , IDraw
    {
        // Задает стиль линий
        private UIBoxStyle _drawStyle;
        // Толщина линий
        public int LineWidth { get; private set; }

        public UIBox(Coordinates coordinates, Dimensions dimensions, int lineWidth, UIBoxStyle drawStyle) : base(coordinates, dimensions)
        {
            LineWidth = Math.Abs(lineWidth);
            _drawStyle = drawStyle;
        }

        // Рисование прямоугольника
        public bool Draw()
        {
            if (_drawStyle != null)
            {
                UILine line;
                UILineStyle lineStyle;
                Coordinates coordinates;
                Dimensions dimensions;

                // Top
                coordinates = new Coordinates() { Left = Position.Left, Top = Position.Top };
                dimensions = new Dimensions() { Width = Size.Width, Height = LineWidth };
                lineStyle = GetStyle(_drawStyle.RectTopLeftCorner, _drawStyle.RectHorizontalLine, _drawStyle.RectTopRightCorner);
                line = new UILine(coordinates, dimensions, lineStyle);
                line.Draw();

                // Right
                coordinates = new Coordinates() { Left = Position.Left + Size.Width, Top = Position.Top };
                dimensions = new Dimensions() { Width = LineWidth, Height = Size.Height };
                lineStyle = GetStyle(_drawStyle.RectTopRightCorner, _drawStyle.RectVerticalLine, _drawStyle.RectBottomRightCorner);
                line = new UILine(coordinates, dimensions, lineStyle);
                line.Position = coordinates;
                line.Draw();
              
                // Bottom
                coordinates = new Coordinates() { Left = Position.Left, Top = Position.Top + Size.Height - 1};
                dimensions = new Dimensions() { Width = Size.Width, Height = LineWidth };
                lineStyle = GetStyle(_drawStyle.RectBottomLeftCorner, _drawStyle.RectHorizontalLine, _drawStyle.RectBottomRightCorner);
                line = new UILine(coordinates, dimensions, lineStyle);
                line.Position = coordinates;
                line.Draw();
            
                // Left
                coordinates = new Coordinates() { Left = Position.Left, Top = Position.Top };
                dimensions = new Dimensions() { Width = LineWidth, Height = Size.Height };
                lineStyle = GetStyle(_drawStyle.RectTopLeftCorner, _drawStyle.RectVerticalLine, _drawStyle.RectBottomLeftCorner);
                line = new UILine(coordinates, dimensions, lineStyle);
                line.Position = coordinates;
                line.Draw();

                return true;
            }
            return false;
        }


        /// <summary>
        /// Создает стиль рисования линии в зависимости от стиля рисования рамки
        /// </summary>
        /// <param name="start">Символ начала линии</param>
        /// <param name="body">Символ тела линии</param>
        /// <param name="end">Символ окончания линии</param>
        /// <returns></returns>
        private UILineStyle GetStyle(char start, char body, char end)
        {
            return new UILineStyle() 
            { 
                LineHorizontalStart = start, 
                LineHorizontalBody = body, 
                LineHorizontalEnd = end,
                LineVerticalStart = start,
                LineVerticalBody = body,
                LineVerticalEnd = end
            };
        }
    }
}
