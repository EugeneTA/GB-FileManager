using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс диалогового окна
    /// </summary>
    public class UIDialogView : IDraw , IContent
    {
        // Данные диалогового окна
        public DialogData Data { get; set; }
        
        // Рамка диалогового окна
        public UIBox Border { get; set; }

        // Линия, отделяющяя заголовок диалогового окна
        public UILine HeaderDivider { get; set; }

        // Линия, отделяющая область кнопок диалогового окна
        public UILine ButtonsDivider { get; set; }

        // Область вывода названия диалогового окна
        public UIBase Header { get; set; }

        // Область вывода сообщения диалогового окна
        public UIBase Body { get; set; }

        // Область ввода информации от пользователя
        public UIBase Input { get; set; }

        // Область вывода кнопок диалогового окна
        public UIBase Footer { get; set; }

        public UIDialogView(UIBox border, UILine headerDividor, UILine buttonsDividor)
        {
            Border = border;
            HeaderDivider = headerDividor;
            ButtonsDivider = buttonsDividor;
            UpdateDimensions();
        }

        private void UpdateDimensions()
        {
            if (Border != null)
            {
                if (HeaderDivider == null)
                {
                    if (ButtonsDivider == null)
                    {
                        Header = null;
                        Body = new UIBase(Border.Position, Border.Size);
                        Footer = null;
                        Input = new UIBase(
                            new Coordinates(
                                Header.Position.Left,
                                Body.Position.Top + Body.Size.Height - 3
                                ),
                            new Dimensions(
                                Border.Size.Width - (2 * Border.LineWidth) - 1,
                                3
                                )
                            );
                    }
                    else
                    {
                        Header = null;
                        Body = new UIBase(Border.Position, new Dimensions(Border.Size.Width, Border.Position.Top - ButtonsDivider.Position.Top));
                        Footer = new UIBase(ButtonsDivider.Position, new Dimensions(Border.Size.Width, Border.Size.Height - Body.Size.Height));
                        Input = new UIBase(
                        new Coordinates(
                            Header.Position.Left,
                            Body.Position.Top + Body.Size.Height - 3
                            ),
                        new Dimensions(
                            Border.Size.Width - (2 * Border.LineWidth) - 1,
                            3
                            )
                        );
                    }
                }
                else
                {
                    if (ButtonsDivider == null)
                    {
                        Header = new UIBase(Border.Position, new Dimensions(Border.Size.Width, Border.Position.Top - ButtonsDivider.Position.Top));
                        Body = new UIBase(HeaderDivider.Position, new Dimensions(Border.Size.Width, Border.Size.Height - HeaderDivider.Size.Height));
                        Footer = null;
                        Input = new UIBase(
                                new Coordinates(
                                    Header.Position.Left,
                                    Body.Position.Top + Body.Size.Height - 3
                                    ),
                                new Dimensions(
                                    Border.Size.Width - (2 * Border.LineWidth) - 1,
                                    3
                                    )
                                );
                    }
                    else
                    {
                        Header = new UIBase(
                            new Coordinates(
                                Border.Position.Left + Border.LineWidth + 1,
                                Border.Position.Top + Border.LineWidth
                                ),
                            new Dimensions(
                                Border.Size.Width - (2 * Border.LineWidth) - 1,
                                HeaderDivider.Position.Top - Border.Position.Top - Border.LineWidth
                                )
                            );

                        Body = new UIBase(
                            new Coordinates(
                                Header.Position.Left,
                                HeaderDivider.Position.Top + Border.LineWidth
                                ),
                            new Dimensions(
                                Border.Size.Width - (2 * Border.LineWidth) - 1,
                                ButtonsDivider.Position.Top - HeaderDivider.Position.Top - Border.LineWidth
                                )
                            );

                        Footer = new UIBase(
                            new Coordinates(
                                Header.Position.Left,
                                ButtonsDivider.Position.Top + Border.LineWidth
                                ),
                            new Dimensions(
                                Border.Size.Width - (2 * Border.LineWidth) - 1,
                                Border.Position.Top + Border.Size.Height - ButtonsDivider.Position.Top - Border.LineWidth - 1
                                )
                            );

                        Input = new UIBase(
                            new Coordinates(
                                Header.Position.Left,
                                Body.Position.Top + Body.Size.Height - 3
                                ),
                            new Dimensions(
                                Border.Size.Width - (2 * Border.LineWidth) - 1,
                                3
                                )
                            );
                    }
                }
            }

        }

        public bool Draw()
        {
            if (Border != null && Data != null)
            {
                return DrawDialog();
            }

            return false;
        }

        public bool Draw(DialogData dialogData)
        {
            if (dialogData != null)
            {
                // Пересчитываем размер окна в зависимости от размера сообщения
                Dimensions size = dialogData.Message == null ? Border.Size : new Dimensions(Border.Size.Width, dialogData.Message.Count + 8);

                // Если поле ввода пользователи не пустое, то увеличиваем размер окна на высоту поля ввода
                if (string.IsNullOrEmpty(dialogData.InputData) == false)
                {
                    size.Height += 4;
                }

                // Задаем стиль линии
                UILineStyle dialogLineStyle = new UILineStyle();

                // Задаем размер линии
                Dimensions dialogLineDimensions = new Dimensions(size.Width, Border.LineWidth);

                // Задаем координаты для линии отделяющей заголовок, от области сообщений
                Coordinates headerDividerPosition = new Coordinates(Border.Position.Left, Border.Position.Top + 2);

                // Задаем координаты для линии отжедляющей область кнопок, от области сообщений
                Coordinates buttonsDividerPosition = new Coordinates(Border.Position.Left, Border.Position.Top + size.Height - 5);

                // Создаем соответсвующие линии
                HeaderDivider = new UILine(headerDividerPosition, dialogLineDimensions, dialogLineStyle);
                ButtonsDivider = new UILine(buttonsDividerPosition, dialogLineDimensions, dialogLineStyle);

                // Задаем стиль, размер и координаты рамки диалогового окна
                UIBoxStyle borderStyle = new UIBoxStyle();
                Dimensions borderSize = size;
                Coordinates borderPosition = Border.Position;

                // Создаем рамку
                Border = new UIBox(borderPosition, borderSize, Border.LineWidth, borderStyle);

                // Создаем диалоговое окно
                Data = dialogData;

                UpdateDimensions();

                return Draw();
            }

            return false;
        }

        public void RefreshContent()
        {
            DrawHeader();
            DrawMessage();
            DrawBtns();
        }

        public void UpdateContent()
        {
            RefreshContent();
        }

        /// <summary>
        /// Добавляет Data.Header в заголовок диалогового окна
        /// </summary>
        private void DrawHeader()
        {
            ClearContent(Header);

            if (HeaderDivider != null)
            {
                if (Data != null && (string.IsNullOrEmpty(Data.Header) != true))
                {
                    Console.SetCursorPosition(Header.Position.Left, Header.Position.Top);
                    Console.Write(StringHelper.AlignString(Data.Header, Header.Size.Width, AlignType.Center));
                }
            }
        }

        /// <summary>
        /// Добавляет Data.Message в поле сообщений диалогового окна
        /// </summary>
        private void DrawMessage()
        {
            ClearContent(Body);

            if (Body != null && Data != null && Data.Message != null)
            {
                int axisXBase = Body.Position.Left;
                int axisYBase = Body.Position.Top;
                int height = Body.Size.Height;
                int width = Body.Size.Width;

                Console.CursorVisible = false;

                // Рассчитываем смещение для вывода первой строки сообщения и кол-во сообщений для вывода

                // Задаем начальные значения по размеру окна сообщений
                int offsetY = axisYBase;
                int numOfMessagesToShow = height;

                // Если сообщение меньше окна, то изменяем параметры
                if (Data.Message.Count < height)
                {
                    //offsetY = axisYBase + (height - Data.Message.Count) / 2;
                    numOfMessagesToShow = Data.Message.Count;
                }

                // Очищаем
                //for (int i = 0; i < height; i++)
                //{
                //    Console.SetCursorPosition(axisXBase, axisYBase + i);
                //    Console.Write(new String('\x20', width+1));
                //}

                // Выводим сообщение
                for (int i = 0; i < numOfMessagesToShow; i++)
                {
                    Console.SetCursorPosition(axisXBase, offsetY + i);
                    Console.Write(StringHelper.ShrinkStringMiddle(Data.Message[i], width));
                }
            }
        }

        /// <summary>
        /// Рисует кнопки Data.Butttons в поле для кнопок диалогового окна
        /// </summary>
        private void DrawBtns()
        {
            if (ButtonsDivider != null)
            {
                /// TODO Create Button Area Clear

                ClearContent(Footer);

                if (Footer != null && Data != null && Data.Buttons != null)
                {
                    // Если массив кнопок не нулевой, то рассчитываем положение курсора для их вывода

                    int numOfBtns = Data.Buttons.Count;
                    int axisXBase = Footer.Position.Left;
                    int axisYBase = Footer.Position.Top;
                    int height = Footer.Size.Height;
                    int width = Footer.Size.Width;

                    // Запоминаме текущий цвет фона, т.к. фон у активной кнопки будет другой
                    ConsoleColor defaultBGColour = Console.BackgroundColor;

                    // Переводим курсор в нужную позицию и очищаем поле для кнопок
                    Console.SetCursorPosition(axisXBase-1, axisYBase);
                    Console.Write(new string('\x20', width+2));

                    // Опять переводим курсор в нужную позицию и выводим кнопки
                    Console.SetCursorPosition(axisXBase, axisYBase);

                    for (int i = 0; i < Data.Buttons.Count; i++)
                    {
                        // Если установлен параметр Отображать, то рисуем кнопку
                        if (Data.Buttons[i].isVisible)
                        {
                            // Если установлены параметр Активна, то меняем цвет фона
                            if (Data.Buttons[i].isActive)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }

                            // Рассчитываем положение курсора в зависимости от количества кнопок
                            int x = axisXBase + ((i * (width / numOfBtns)) + (((width / numOfBtns) - Data.Buttons[i].Name.Length) / 2));

                            // Переводим курсор в нужную позицию и рисуем кнопку
                            Console.SetCursorPosition(x, axisYBase);
                            Console.Write($" {new string('\x20', Data.Buttons[i].Name.Length)} ");
                            Console.SetCursorPosition(x, axisYBase + 1);
                            Console.Write($" {Data.Buttons[i].Name} ");
                            Console.SetCursorPosition(x, axisYBase + 2);
                            Console.Write($" {new string('\x20', Data.Buttons[i].Name.Length)} ");
                            Console.BackgroundColor = defaultBGColour;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Очищает внутреннее пространство элемента в консоли
        /// </summary>
        /// <param name="item">графифеский элемент</param>
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
        /// Вызов всплывающего окна и обработка ответа пользователя. Если пользователь согласился, метод вернет true, иначе false
        /// </summary>
        /// <returns></returns>
        private bool DrawDialog()
        {
            bool result = false;

            if (Data != null)
            {
                // Рисуем элементы диалогового окна
                Border.Draw();
                HeaderDivider.Draw();
                ButtonsDivider.Draw();

                // Выводим сообщения в соответсвующие разделы и рисуем кнопки
                DrawHeader();
                DrawBtns();
                DrawMessage();

                // Проверяем, что нужно показывать кнопки (если список не пустой, то показываем)
                if (Data.Buttons != null)
                {
                    // Создаем флаг "Показывать всплывающее окно" 
                    bool showPopUp = true;

                    // Создаем флаг "Диалоговое окно с полем для ввода данных пользователя"
                    bool isInputDialog = !string.IsNullOrEmpty(Data.InputData);

                    // Если диалоговое окно с полем ввода, то переключаемся на него
                    if (isInputDialog)
                    {
                        SwitchToInputArea();
                    }

                    // Запоминаем индекс активной кнопки
                    int activeIndex = Data.Buttons.FindIndex(x => x.isActive);

                    // Показываем всплывающее окно до тех пор, пока активен флаг "Показывать всплывающее окно"
                    while (showPopUp)
                    {
                        // Ожидаем выбора пользователя
                        switch (Console.ReadKey().Key)
                        {
                            // Пользователь подтвердил выбор
                            case (ConsoleKey.Enter):
                                {
                                    if (Data.Buttons[activeIndex].ButtonBehavior == ButtonBehavior.Confirm)
                                    {
                                        // Если активна кнопка c поведением "Подтвердить",
                                        result = true;
                                    }
                                    // Сбрасываем флаг "Показывать всплывающее окно"
                                    showPopUp = false;
                                }
                                break;

                            // Перемещение между кнопками
                            case (ConsoleKey.Tab):
                            case (ConsoleKey.LeftArrow):
                            case (ConsoleKey.RightArrow):
                                {
                                    // Изменяем активность кнопок и перерисовываем их
                                    if (Data.Buttons.Count > 1)
                                    {
                                        Data.Buttons.ForEach(x => x.isActive ^= true);
                                    }

                                    // Перерисоввываем кнопки всплывающего окна
                                    DrawBtns();
                                }
                                break;

                            // Переключение между полем для ввода и кнопками
                            case (ConsoleKey.UpArrow):
                            case (ConsoleKey.DownArrow):
                                {
                                    // Если диалоговое окно с полем для ввода, то переключаемся на это поле
                                    if (isInputDialog)
                                    {
                                        SwitchToInputArea();
                                    }
                                }
                                break;

                            // При нажатии других клавиш закрываем окно
                            default:
                                {
                                    showPopUp = false;
                                }
                                break;
                        }

                        activeIndex = Data.Buttons.FindIndex(x => x.isActive);
                    }
                }
                else
                {
                    // Если список кнопок пустой,
                    // то проверяем, что поле ввода пользовательских анных не пустое
                    if (string.IsNullOrEmpty(Data.InputData) == false)
                    {
                        // Обрабатываем пользовательский ввод
                        Data.InputData = ProcessUserInput(Data.InputData, Body.Size.Width);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Перключение на поле ввода информации от пользователя и обработка ввода
        /// </summary>
        private void SwitchToInputArea()
        {
            // Сохраняем индекс активной кнопки
            int saveActiveButton = Data.Buttons.FindIndex(x => x.isActive);

            // сбрасываем флаг активности и перерисовываем их
            Data.Buttons.ForEach(x => x.isActive = false);
            DrawBtns();

            // Обрабатываем ввод от пользователя
            if (string.IsNullOrEmpty(Data.InputData) == false)
            {
                Data.InputData = ProcessUserInput(Data.InputData, Body.Size.Width);
            }

            // Восстанавливаем флаг активности у кнопок
            Data.Buttons[saveActiveButton].isActive = true;

            // Перерисоввываем кнопки всплывающего окна
            DrawBtns();
        }

        /// <summary>
        /// Обработка ввода данных от пользователя
        /// </summary>
        /// <param name="initString">начальная строка, выводимая в поле ввода</param>
        /// <param name="maxWidth">максимальная длина поля ввода</param>
        /// <returns></returns>
        private string ProcessUserInput(string initString, int maxWidth)
        {
            int axisX = Input.Position.Left;
            int axisY = Input.Position.Top;

            string result = initString;
            string emptyString = new string('\x20', maxWidth);

            // Запоминаем цвета консоли по умолчанию
            ConsoleColor defaultBGColor = Console.BackgroundColor;
            ConsoleColor defaultTextColour = Console.ForegroundColor;

            // показывем кусор и устанавливаем новые цвета
            Console.CursorVisible = true;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            // Очищаем поле ввода информации
            Console.SetCursorPosition(axisX, axisY++);
            Console.Write(emptyString);
            Console.SetCursorPosition(axisX, axisY++);
            Console.Write(emptyString);
            Console.SetCursorPosition(axisX, axisY);
            Console.Write(emptyString);
            Console.SetCursorPosition(axisX, --axisY);
            Console.Write(result.TrimStart());


            // Обрабатываем ввод от пользователя
            ConsoleKeyInfo key = Console.ReadKey();

            while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.DownArrow && key.Key != ConsoleKey.Escape)
            {
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        {
                            if (string.IsNullOrEmpty(result) == false)
                            {
                                result = result.Substring(0, result.Length - 1);
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Delete:
                        {
                        }
                        break;
                    default:
                        {
                            if (result.Length < maxWidth)
                            {
                                result += key.KeyChar;
                            }
                        }
                        break; 
                }

                Console.SetCursorPosition(axisX, axisY);
                Console.Write(emptyString);
                Console.SetCursorPosition(axisX, axisY);
                Console.Write(result.TrimStart());
                
                key = Console.ReadKey();
            }

            // Восстанавливаем цвет фона консоли
            Console.BackgroundColor = defaultBGColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            // оцищаем поле ввода
            Console.SetCursorPosition(axisX, --axisY);
            Console.Write(emptyString);
            Console.SetCursorPosition(axisX, ++axisY);
            Console.Write(emptyString);
            Console.SetCursorPosition(axisX, ++axisY);
            Console.Write(emptyString);

            // выводим введенный данные пользователя (надпись с цветом фона по умолчанию)
            Console.SetCursorPosition(axisX, --axisY);           
            result = result.Trim();
            Console.Write(result);
      
            // восстанавливаем цвет текста и прячем курсор
            Console.ForegroundColor = defaultTextColour;
            Console.CursorVisible = false;

            return result;
        }

    }
}
