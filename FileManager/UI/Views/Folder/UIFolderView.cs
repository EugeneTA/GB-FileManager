using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс просмотра структуры каталога
    /// </summary>
    public class UIFolderView : IDraw , IContent
    {
        // Содержание заголовка
        public string HeaderContent { get; set; }
        
        // Элементы каталога
        public List<FolderItem> BodyContent { get; set; }

        // Флаг активности панели. Если активна, то выбранный элемент выделяется отдельным фоном
        public bool IsActive { get; set; }

        // Внешняя рамка панели дерева каталогов
        public UIBox Border { get; set; }
        // Линия, разделяющая шапку от дерева каталогов
        public UILine HeaderDivider { get; set; }
        // Линия, отделяющая дерево каталогов от нижней информационной панели
        public UILine FooterDivider { get; set; }
        // Горизонтальная линия, разделеяющая название элемента и информацию о его размере
        public UILine InfoSizeSeparatorLine { get; set; }
        // Горизонтальная линия, разделеяющая информацию о его размере и типе
        public UILine InfoTypeSeparatorLine { get; set; }
        // Горизонтальная линия, разделеяющая информацию о типе и аттрибуты
        public UILine InfoAttrSeparatorLine { get; set; }


        // Размеры и положения информационных частей соответсвующих разделов
        public UIBase Header { get; private set; }
        public UIBase Body { get; private set; }
        public UIBase BodyInfoName { get; private set; }
        public UIBase BodyInfoSize { get; private set; }
        public UIBase BodyInfoDate { get; private set; }
        public UIBase BodyInfoAttr { get; private set; }
        public UIBase Footer { get; private set; }

        public UIFolderView(UIBox border, UILine headerDivider, UILine footerDivider, UILine infoSizeSeparatorLine, UILine infoTypeSeparatorLine, UILine infoAttrSeparatorLine)
        {
            Border = border ?? throw new ArgumentNullException(nameof(border));
            HeaderDivider = headerDivider ?? throw new ArgumentNullException(nameof(headerDivider));
            FooterDivider = footerDivider ?? throw new ArgumentNullException(nameof(footerDivider));
            InfoSizeSeparatorLine = infoSizeSeparatorLine ?? throw new ArgumentNullException(nameof(infoSizeSeparatorLine));
            InfoTypeSeparatorLine = infoTypeSeparatorLine ?? throw new ArgumentNullException(nameof(infoTypeSeparatorLine));
            InfoAttrSeparatorLine = infoAttrSeparatorLine ?? throw new ArgumentNullException(nameof(infoAttrSeparatorLine));


            // Рассчитываем размеры и положения элементов

            Header = new UIBase(
                new Coordinates(
                    Border.Position.Left + Border.LineWidth + 1,
                    Border.Position.Top + Border.LineWidth
                    ),
                new Dimensions(
                    Border.Size.Width - (2 * Border.LineWidth) - 1,
                    HeaderDivider.Position.Top  - Border.Position.Top - Border.LineWidth
                    )
                );

            Body = new UIBase(
                new Coordinates(
                    Header.Position.Left,
                    HeaderDivider.Position.Top + Border.LineWidth
                    ),
                new Dimensions(
                    Border.Size.Width - (2 * Border.LineWidth),
                    FooterDivider.Position.Top - HeaderDivider.Position.Top - Border.LineWidth
                    )
                );

            Footer = new UIBase(
                new Coordinates(
                    Header.Position.Left,
                    FooterDivider.Position.Top + Border.LineWidth
                    ),
                new Dimensions(
                    Border.Size.Width - (2 * Border.LineWidth) - 1,
                    Border.Position.Top + Border.Size.Height - FooterDivider.Position.Top - Border.LineWidth - 1
                    )
                );

            BodyInfoName = new UIBase(
                new Coordinates(
                    Body.Position.Left,
                    Body.Position.Top
                    ),
                new Dimensions(
                    InfoSizeSeparatorLine.Position.Left - Border.Position.Left - Border.LineWidth - 2,
                    Body.Size.Height
                    )
                );

            BodyInfoSize = new UIBase(
                new Coordinates(
                    InfoSizeSeparatorLine.Position.Left + Border.LineWidth + 1,
                    Body.Position.Top
                    ),
                new Dimensions(
                    InfoTypeSeparatorLine.Position.Left - InfoSizeSeparatorLine.Position.Left - Border.LineWidth - 2,
                    Body.Size.Height)
                );

            BodyInfoDate = new UIBase(
                new Coordinates(
                    InfoTypeSeparatorLine.Position.Left + Border.LineWidth + 1,
                    Body.Position.Top
                    ),
                new Dimensions(
                    InfoAttrSeparatorLine.Position.Left - InfoTypeSeparatorLine.Position.Left - Border.LineWidth - 2,
                    Body.Size.Height)
                );

            BodyInfoAttr = new UIBase(
                new Coordinates(
                    InfoAttrSeparatorLine.Position.Left + Border.LineWidth +1,
                    Body.Position.Top
                    ),
                new Dimensions(
                    Border.Position.Left + Border.Size.Width - InfoAttrSeparatorLine.Position.Left - Border.LineWidth - 2,
                    Body.Size.Height)
                );
        }


        /// <summary>
        /// Рисуем границы элементов панели просмотра дерева каталогов в консоли
        /// </summary>
        /// <returns></returns>
        public bool Draw()
        {
            bool result = false;

            if (Border != null)
            {
                result = Border.Draw();

                if (HeaderDivider != null)
                {
                    HeaderDivider.Draw();
                }

                if (FooterDivider != null)
                {
                    FooterDivider.Draw();
                }

                if (InfoSizeSeparatorLine != null)
                {
                    InfoSizeSeparatorLine.Draw();
                }

                if (InfoTypeSeparatorLine != null)
                {
                    InfoTypeSeparatorLine.Draw();
                }

                if (InfoAttrSeparatorLine != null)
                {
                    InfoAttrSeparatorLine.Draw();
                }

            }

            return result;
        }

        /// <summary>
        /// Обновляет содержимое элементов в соответствии с данными в BodyContent
        /// </summary>
        public void RefreshContent()
        {
            if (BodyContent != null)
            {
                ConsoleColor defaultBGcolor = Console.BackgroundColor;
                ConsoleColor defaultTextColor = Console.ForegroundColor;

                string type;

                Console.CursorVisible = false;
                
                ClearContent(Header, ConsoleColor.Cyan);
                RefreshContent(Header, HeaderContent, -1, ConsoleColor.Black, ConsoleColor.Cyan);

                for (int i = 0; i < BodyContent.Count; i++)
                {
                    switch (BodyContent[i].Type)
                    {
                        case FolderItemType.Drive:
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.BackgroundColor = defaultBGcolor;
                                type = BodyContent[i].Size;
                            }
                            break;
                        case FolderItemType.Folder:
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.BackgroundColor = defaultBGcolor;
                                type = "< ПАПКА >";
                            }
                            break;
                        case FolderItemType.Return:
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.BackgroundColor = defaultBGcolor;
                                type = "< НАЗАД >";
                            }
                            break;
                        default:
                            {
                                Console.ForegroundColor = defaultTextColor;
                                Console.BackgroundColor = defaultBGcolor;
                                type = BodyContent[i].Size;
                            }
                            break;
                    }

                    if (BodyContent[i].IsActive && IsActive)
                    {
                        ClearContent(Footer);
                        RefreshContent(Footer, BodyContent[i].Path, -1);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    }

                    RefreshContent(BodyInfoName, StringHelper.ShrinkStringEnd(BodyContent[i].Name, BodyInfoName.Size.Width), i);
                    RefreshContent(BodyInfoSize, StringHelper.AlignString(type, BodyInfoSize.Size.Width+1, AlignType.Right), i);
                    RefreshContent(BodyInfoDate, BodyContent[i].Date, i);
                    RefreshContent(BodyInfoAttr, BodyContent[i].Attr, i);
                }

                Console.BackgroundColor = defaultBGcolor;
                Console.ForegroundColor = defaultTextColor;
            }
        }

        public void UpdateContent()
        {
            RefreshContent();
        }

        /// <summary>
        /// Очищает содержимое всех элементов панели просмотра дерева каталогов
        /// </summary>
        public void ClearContent()
        {
            ClearContent(Header);
            ClearContent(BodyInfoName);
            ClearContent(BodyInfoSize);
            ClearContent(BodyInfoDate);
            ClearContent(BodyInfoAttr);
            ClearContent(Footer);
        }

        /// <summary>
        /// Очищает содержимое заданного элемента панели просмотра дерева каталогов
        /// </summary>
        /// <param name="item">элемент, в котором необходимо очистить содержимое</param>
        private void ClearContent(UIBase item)
        {
            if (item != null)
            {
                string emptyStr = new string('\x20', item.Size.Width + 2);

                for (int i = 0; i < item.Size.Height; i++)
                {
                    Console.SetCursorPosition(item.Position.Left - 1, item.Position.Top + i);
                    Console.Write(emptyStr);
                }
            }
        }

        /// <summary>
        /// Очищает содержимое заданного элемента панели просмотра дерева каталогов с заменой фона на указанный
        /// </summary>
        /// <param name="item">элемент, в котором необходимо очистить содержимое</param>
        /// <param name="backgroundColor">цвет фона</param>
        private void ClearContent(UIBase item, ConsoleColor backgroundColor)
        {
            if (item != null)
            {
                ConsoleColor defaultBGcolor = Console.BackgroundColor;

                Console.BackgroundColor = backgroundColor;

                ClearContent(item);

                Console.BackgroundColor = defaultBGcolor;
            }
        }

        /// <summary>
        /// Выводит информацию в заданный элемент
        /// </summary>
        /// <param name="item">элемент, в который выводить информацию</param>
        /// <param name="content">выводимая информация</param>
        /// <param name="offsetY">вертикальное смещение относительно начальной точки элемента item</param>
        private void RefreshContent(UIBase item, string content, int offsetY)
        {
            if (item != null)
            {
                if (offsetY == -1)
                {
                    // Зполняем в центре элемента по вертикали, если смещение -1 
                    Console.SetCursorPosition(item.Position.Left, item.Position.Top + (item.Size.Height / 2));
                    Console.Write(StringHelper.ShrinkStringMiddle(content, item.Size.Width));
                }
                else
                {
                    // Очищаем здесь, для того, чтобы фон выбранного элемента показывался на всю ширину поля
                    Console.SetCursorPosition(item.Position.Left - 1, item.Position.Top + offsetY);
                    Console.Write(new string('\x20', item.Size.Width + 2));
                    // Заполняем информацией
                    Console.SetCursorPosition(item.Position.Left, item.Position.Top + offsetY);
                    Console.Write(content);
                }

            }
        }

        /// <summary>
        /// Выводит информацию в заданный элемент укзанным цветом
        /// </summary>
        /// <param name="item">элемент, в который выводить информацию</param>
        /// <param name="content">выводимая информация</param>
        /// <param name="offsetY">вертикальное смещение относительно начальной точки элемента item</param>
        /// <param name="foregroundColor">цвет текста</param>
        /// <param name="backgroundColor">цвет фона</param>
        private void RefreshContent(UIBase item, string content, int offsetY, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (item != null)
            {
                ConsoleColor defaultBGcolor = Console.BackgroundColor;
                ConsoleColor defaultTextColor = Console.ForegroundColor;

                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;

                RefreshContent(item, content, offsetY);

                Console.BackgroundColor = defaultBGcolor;
                Console.ForegroundColor = defaultTextColor;
            }
        }


    }
}
