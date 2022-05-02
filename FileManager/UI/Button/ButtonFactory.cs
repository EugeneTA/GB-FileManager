using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс создание кнопок в зависимости от запрошенного типа
    /// </summary>
    public static class ButtonFactory
    {
        // Словарь кнопок диалоговых окон
        static Dictionary<ButtonType, Button> Buttons = new Dictionary<ButtonType, Button>()
        {

            {
                ButtonType.Cancel,
                new Button()
                 {
                    isActive = false,
                    isVisible = false,
                    Name = "Отмена",
                    ButtonType = ButtonType.Cancel,
                    ButtonBehavior = ButtonBehavior.Reject
                }
            },
            {
                ButtonType.Confirm,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Закрыть",
                    ButtonType = ButtonType.Confirm,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
            {
                ButtonType.ConfirmCopy,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Скопировать",
                    ButtonType = ButtonType.ConfirmCopy,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
            {
                ButtonType.ConfirmDelete,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Удалить",
                    ButtonType = ButtonType.ConfirmDelete,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
             {
                ButtonType.ConfirmRename,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Переименовать",
                    ButtonType = ButtonType.ConfirmRename,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
             {
                ButtonType.ConfirmMove,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Переместить",
                    ButtonType = ButtonType.ConfirmMove,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
            {
                ButtonType.ConfirmCreate,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Создать",
                    ButtonType = ButtonType.ConfirmCreate,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
            {
                ButtonType.ConfirmSearch,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Искать",
                    ButtonType = ButtonType.ConfirmSearch,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
            {
                ButtonType.Replace,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Заменить",
                    ButtonType = ButtonType.Replace,
                    ButtonBehavior = ButtonBehavior.Confirm
                }
            },
            {
                ButtonType.Skip,
                new Button()
                {
                    isActive = false,
                    isVisible = false,
                    Name = "Пропустить",
                    ButtonType = ButtonType.Skip,
                    ButtonBehavior = ButtonBehavior.Reject
                }
            }
        };
    
        /// <summary>
        /// Возвращает список кнопок
        /// </summary>
        /// <param name="buttons">Список необходимых типов кнопок для создания</param>
        /// <param name="activeButton">Какая кнопка будет активной</param>
        /// <returns></returns>
        public static List<Button> GetButtons(List<ButtonType> buttons, ButtonType activeButton)
        {
            if (Buttons != null && buttons != null && buttons.Count > 0)
            {
                ClearStates();

                var result = Buttons?.Where(y => buttons.Contains(y.Key)).Select(x => x.Value).ToList();
                result.ForEach(x => { x.isVisible = true; if (x.ButtonType == activeButton) x.isActive = true; });

                return result;
            }

            return null;
        }

        /// <summary>
        /// Возвращает кнопку 
        /// </summary>
        /// <param name="type">Какой тип кнопки нужно создать</param>
        /// <returns></returns>
        public static List<Button> GetButtons(ButtonType type)
        {
            if (Buttons != null)
            {
                ClearStates();

                var result = Buttons.Where(y => type == y.Key).Select(x => x.Value).ToList();
                result.ForEach(x => { x.isVisible = true; x.isActive = true; });

                return result;
            }

            return null;
        }


        /// <summary>
        /// Сброс состояний isActive и isVisible для всех кнопок в словаре
        /// </summary>
        private static void ClearStates()
        {
            (
            Buttons?.
            Where(x => x.Value.isActive == true || x.Value.isVisible == true).
            Select(x => x.Value).ToList()
            ).
            ForEach(x => { x.isActive = false; x.isVisible = false; });
        }
    }
}
