using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс вывода информации о горячих клавишах приложения
    /// </summary>
    public class UIInfoView : IDraw, IContent
    {
        List<string> Data { get; set; }
        public UIBox Border { get; set; }
        public UIBase Body { get; private set; }

        public UIInfoView(UIBox border, List<string> data)
        {
            Border = border ?? throw new ArgumentNullException(nameof(border));
            Data = data ?? throw new ArgumentNullException(nameof(data));

            Body = new UIBase(
                new Coordinates(
                    Border.Position.Left + Border.LineWidth,
                    Border.Position.Top + Border.LineWidth
                    ),
                new Dimensions(
                    Border.Size.Width - (2 * Border.LineWidth),
                    Border.Size.Height - (2 * Border.LineWidth)
                    )
                );
        }

        public bool Draw()
        {
            bool result = false;

            if (Border != null)
            {
                result = Border.Draw();
            }

            return result;
        }

        public void RefreshContent()
        {
            if (Data != null)
            {
                //Console.SetCursorPosition(Body.Position.Left, Body.Position.Top+1);
                int width = Body.Size.Width / Data.Count;
                int offset = 0;
                Console.SetCursorPosition(Body.Position.Left + offset, Body.Position.Top + 1);
                Console.Write(StringHelper.AlignString(Data[0], width - 2, AlignType.Center));
                offset += width;

                for (int i = 1; i < Data.Count; i++)
                {
                    Console.Write("|");
                    Console.SetCursorPosition(Body.Position.Left + offset, Body.Position.Top + 1);
                    Console.Write(StringHelper.AlignString(Data[i], width - 2, AlignType.Center));
                    offset += width;
                }
            }
        }

        public void UpdateContent()
        {
            RefreshContent();
        }
    }
}
