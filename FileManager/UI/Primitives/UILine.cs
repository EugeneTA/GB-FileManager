using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Графический примитив - линия
    /// </summary>
    public class UILine : UIBase, IDraw
    {
        // стиль, которым рисовать линию
        public UILineStyle DrawStyle { get; set; }

        public UILine(Coordinates coordinates, Dimensions dimensions, UILineStyle drawStyle) : base(coordinates, dimensions)
        {
            DrawStyle = drawStyle;
        }

        // Рисование линии
        public bool Draw()
        {
            if (DrawStyle != null)
            {
                int left;
                int top;

                // Если высота линии больше ширины, то рисуем вертикальную линию
                // Если ширина больше высоты, то рисуем горизонтальную линию

                if (base.Size.Height > base.Size.Width)
                {
                    char startSymbol = DrawStyle.LineVerticalStart;
                    char bodySymbol = DrawStyle.LineVerticalBody;
                    char endSymbol = DrawStyle.LineVerticalEnd;

                    for (int offsetX = 0; offsetX < base.Size.Width; offsetX++)
                    {
                        left = base.Position.Left + offsetX;
                        top = base.Position.Top;

                        Console.SetCursorPosition(left, top);
                        Console.Write(startSymbol);

                        for (int i = 2; i < base.Size.Height; i++)
                        {
                            Console.SetCursorPosition(left, ++top);
                            Console.Write(bodySymbol);
                        }
                        Console.SetCursorPosition(left, ++top);
                        Console.Write(endSymbol);
                    }

                }
                else
                {
                    for (int offsetY = 0; offsetY < base.Size.Height; offsetY++)
                    {
                        char startSymbol = DrawStyle.LineHorizontalStart;
                        char bodySymbol = DrawStyle.LineHorizontalBody;
                        char endSymbol = DrawStyle.LineHorizontalEnd;

                        left = base.Position.Left;
                        top = base.Position.Top + offsetY;

                        Console.SetCursorPosition(left++, top);
                        Console.Write(startSymbol);

                        for (int i = 1; i < base.Size.Width; i++)
                        {
                            Console.SetCursorPosition(left++, top);
                            Console.Write(bodySymbol);
                        }

                        Console.SetCursorPosition(left, top);
                        Console.Write(endSymbol);
                    }
                }
                return true;
            }
            return false;
        }
    }
}
