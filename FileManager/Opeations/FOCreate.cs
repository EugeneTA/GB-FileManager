using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс обеспечивает создание папки или файла
    /// </summary>
    public class FOCreate : FOBase
    {
        public FOCreate(FOData data)
        {
            if (data != null)
            {
                base.Data = data;
            }
        }

        public override bool Execute()
        {
            return Create(base.Data);
        }

        /// <summary>
        /// Создание папки или файла
        /// </summary>
        /// <param name="path">путь к создаваемому элементу</param>
        /// <returns></returns>
        private bool Create(string path)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(path) == false)
            {
                // Если в указанном пути указанно расширение, то создаем файл
                // иначе создаем папку

                if (Path.HasExtension(path))
                {
                    // Если такого пути не существует, то пытаемся создать файл
                    // иначе выводим сообщение о смене названия
                    if (File.Exists(path) == false)
                    {
                        try
                        {
                            File.Create(path).Close();
                        }
                        catch (Exception e)
                        {
                            result = false;
                            ErrorHandler(new List<string>() { " ", "Ошибка создания файла", $"{path}", $"Ошибка: {e.Message}", " " });
                        }
                    }
                    else
                    {
                        result = DisplayCreateQuestion(path);
                    }
                }
                else
                {
                    // Если такого пути не существует, то пытаемся создать папку
                    // иначе выводим сообщение о смене названия

                    if (Directory.Exists(path) == false)
                    {
                        try
                        {
                            Directory.CreateDirectory(path);
                        }
                        catch (Exception e)
                        {
                            result = false;
                            ErrorHandler(new List<string>() { " ", "Ошибка создания каталога", $"{path}", $"Ошибка: {e.Message}", " " });
                        }
                    }
                    else
                    {
                        result = DisplayCreateQuestion(path);
                    }
                }

            }

            return result;
        }


        private bool Create(FOData data)
        {
            if (data != null)
            {
                DisplayCreateMessage(data.DestinationPath);
                return Create(data.DestinationPath);
            }
            return false;
        }

        /// <summary>
        /// Вывод сообщения в диалоговое окно о создании папки/файла
        /// </summary>
        /// <param name="source">путь к создаваемому объекту</param>
        private void DisplayCreateMessage(string source)
        {
            Data.Dialog.Data = new DialogCreateTemplate(source, false);
            Data.Dialog.RefreshContent();
        }

        /// <summary>
        ///  Вывод сообщения в диалоговое окно о том, что файл или папка с таким названием уже существует
        ///  Ожидает ввода нового названия или отмены операции
        /// </summary>
        /// <param name="source">путь к создаваемому объекту</param>
        /// <returns></returns>
        private bool DisplayCreateQuestion(string source)
        {
            bool result = false;

            Data.Dialog.Data.Header = "Создание папки (файла)";
            Data.Dialog.Data.Message = new List<string> { " ", "Такая папка (файл) уже существует", source, " " };
            Data.Dialog.Data.Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.Cancel, ButtonType.Confirm }, ButtonType.Cancel);
            if (Data.Dialog.Draw(Data.Dialog.Data))
            {
                Data.DestinationPath = Path.Combine(Data.SourcePath, Data.Dialog.Data.InputData);
                result = Execute();
            }

            return result;
        }
    }
}
