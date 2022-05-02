
using System;

namespace FileManager
{
    public static class StringHelper
    {
        /// <summary>
        /// Возвращает строку с размером файла, например "562 B" или "12.83 Gb"
        /// </summary>
        /// <param name="fileSize">Размер файла</param>
        /// <returns></returns>
        public static string FileSizeToString(long fileSize)
        {
            if (fileSize < Math.Pow(10, 3))
            {
                return $"{(double)fileSize}  B";
            }
            else if (fileSize < Math.Pow(10, 6))
            {
                return $"{((double)fileSize / Math.Pow(2,10)): ###.00} Kb";
            }
            else if (fileSize < Math.Pow(10, 9))
            {
                return $"{((double)fileSize / Math.Pow(2, 20)): ###.00} Mb";
            }
            else if (fileSize < Math.Pow(10, 12))
            {
                return $"{((double)fileSize / Math.Pow(2, 30)): ###.00} Gb";
            } 
            else
            {
                return $"{((double)fileSize / Math.Pow(2, 40)): ###.00} Tb";
            }
        }

        /// <summary>
        /// Возвращает строку заданной ширины, в которой сообщение выравнено в соответствии с заданным параметром alignType
        /// Пустое место заполняется пробелами
        /// </summary>
        /// <param name="data">Входная строка</param>
        /// <param name="width">Заданная ширина</param>
        /// <param name="alignType">Параметр выравнивания (по левому краю, по центру, по правому краю)</param>
        /// <returns></returns>
        public static string AlignString(string data, int width, AlignType alignType)
        {

            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            else
            {
                if (data.Length > width)
                {
                    // Если исходная строка больше заданной ширины, то обрезаем ее до заданной ширины
                    return ShrinkStringEnd(data, width);
                }
                else if (data.Length == width)
                {
                    // Если исходная строка равна заданной ширине, то возвращаем исходную строку
                    return data;
                }
                else
                {
                    // Во всех остальных случаях выравниваем строку с соответствии с заданным параметром выравнивания, заполняя свободное место пробелами
                    switch (alignType)
                    {
                        case AlignType.Center:
                            {
                                // Выравнивание по центру
                                return $"{new string('\x20', (width - data.Length) / 2)}{data}{new string('\x20', (width - data.Length) - ((width - data.Length) / 2))}";
                            }
                        case AlignType.Right:
                            {
                                // Выравнивание по правому краю
                                return $"{new string('\x20', (width - data.Length))}{data}";
                            }
                        default:
                            {
                                // Во всех остальных случаях выравниваем по левому краю
                                return $"{data}{new string('\x20', width - data.Length)}";
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает строку обрезанную до заданной ширины. 
        /// Берутся первые 3 символа строки, добавляются точки (от 1 до 3), остальное место заполняется последними символами переданной строки 
        /// </summary>
        /// <param name="data">Первоначальная строка</param>
        /// <param name="width">Заданная ширина итоговой строки</param>
        /// <returns></returns>
        public static string ShrinkStringMiddle(string data, int width)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            else
            {
                // Если длина строки больше ширины поля вывода
                if (data.Length > width)
                {
                    string delimetr;

                    if (data.Length - width > 2)
                    {
                        // Если строка больше ширины поля больше чем на 2 символа
                        // то обрезаем начало (там должно быть название диска), добавляем 3 точки
                        // а далее заполнеяем оставшееся место последними символами строки

                        return $"{data.Substring(0, 3)}...{data.Remove(0, data.Length - width + 6)}";
                    }
                    else
                    {
                        // Если строка больше ширины поля больше меньше чем на 2 символа
                        // то обрезаем начало (там должно быть название диска), добавляем нужное количество точек
                        // а далее заполнеяем оставшееся место последними символами строки

                        delimetr = new string('.', data.Length - width);
                        return $"{data.Substring(0, 3)}{delimetr}{data.Remove(0, data.Length - width + delimetr.Length + 3)}";
                    }
                }
                else
                {
                    return data;
                }
            }
        }

        /// <summary>
        /// Возвращает строку обрезанную до заданной ширины. 
        /// Берутся первые символа строки а в конце дорбавляются точки вместо тех символов, что не помещаются в заданную ширину 
        /// </summary>
        /// <param name="data">Первоначальная строка</param>
        /// <param name="width">Заданная ширина итоговой строки</param>
        /// <returns></returns>
        public static string ShrinkStringEnd(string data, int width)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            else
            {
                // Если длина строки больше ширины поля вывода
                if (data.Length > width)
                {
                    return $"{data.Substring(0, width - 3)}...";
                }
                else
                {
                    return data;
                }
            }
        }

    }
}
