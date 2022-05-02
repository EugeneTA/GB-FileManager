
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileManager
{
    /// <summary>
    /// Класс осуществления копирования папки/файла
    /// </summary>
    public class FOCopy : FOBase
    {
        public FOCopy(FOData data)
        {
            if (data != null)
            {
                base.Data = data;
            }
        }

        public override bool Execute()
        {
            return Copy(base.Data);
        }

        /// <summary>
        /// Копирование папки/файла 
        /// </summary>
        /// <param name="sourcePath">откуда копировать</param>
        /// <param name="destinationPath">куда копировать</param>
        /// <param name="doReplace">если true, то принудительно перезаписывает существующие файлы/папки</param>
        private bool Copy(string sourcePath, string destinationPath, bool doReplace)
        {
            bool result = true;

            // Проверяем, что переданные пути не являются пустыми и пути не равны друг другу (копирование папки в саму себя)
            if (string.IsNullOrWhiteSpace(sourcePath) == false && string.IsNullOrWhiteSpace(destinationPath) == false && String.Equals(sourcePath, destinationPath) == false)
            {
                // Если это так, то проверяем, что источником является файл и директория, куда его надо скопировать, тоже существует
                if (File.Exists(sourcePath) && Directory.Exists(destinationPath))
                {
                    // Добавляем к пути, куда надо скопировать, название файла.
                    string destinationFilePath = Path.Combine(destinationPath, new FileInfo(sourcePath).Name);

                    if (String.Equals(sourcePath, destinationFilePath) == false)
                    {

                        // Если файла не существует по указанному пути, то записываем во всплывающее окно информацию о копировании файла 
                        if (File.Exists(destinationFilePath) == false)
                        {
                            try
                            {
                                DisplayCopyMessage(new FileInfo(sourcePath).Name, destinationFilePath);
                                File.Copy(sourcePath, destinationFilePath);
                            }
                            catch (Exception e)
                            {
                                result = false;
                                ErrorHandler(new List<string> { " ", "Ошибка копирования файла ", $"{sourcePath} ", "в ", $"{destinationFilePath} ", $"Ошибка: {e.Message}", " " });
                            }
                        }
                        else
                        {
                            // Если файл уже существует, то выводим сообщение, что такой файл уже существует

                            DialogData savedPopUpData = Data.Dialog.Data;

                            Data.Dialog.Data = new DialogData()
                            {
                                Header = Data.Dialog.Data.Header,
                                Message = new List<string> { " ", "Файл", new FileInfo(sourcePath).Name, "в", destinationFilePath, "уже существует. Заменить?", " " },
                                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.Skip, ButtonType.Replace }, ButtonType.Skip)
                            };

                            bool replace = doReplace || Data.Dialog.Draw(Data.Dialog.Data);

                            // Если ползователь ответил Заменить, то сначала удаляем существующий файл, а затем копируем новый. 
                            if (replace == true)
                            {
                                Data.Dialog.Data = savedPopUpData;
                                Data.Dialog.Data.Buttons = null;
                                Data.Dialog.Draw();

                                // Сначала удаляем старый файл
                                try
                                {
                                    DisplayDeleteMessage(destinationFilePath);
                                    File.Delete(destinationFilePath);
                                }
                                catch (Exception e)
                                {
                                    result = false;
                                    ErrorHandler(new List<string> { " ", "Ошибка удаления файла", $"{destinationFilePath} ", $"Ошибка: {e.Message}", " " });
                                }

                                // Затем копируем новый
                                try
                                {
                                    DisplayCopyMessage(new FileInfo(sourcePath).Name, destinationFilePath);
                                    File.Copy(sourcePath, destinationFilePath);
                                }
                                catch (Exception e)
                                {
                                    result = false;
                                    ErrorHandler(new List<string> { " ", "Ошибка копирования файла", $"{sourcePath} ", "в ", $"{destinationFilePath} ", $"Ошибка: {e.Message}", " " });
                                }
                            }
                        }
                    }
                }
                else if (Directory.Exists(sourcePath))
                {
                    // Если источником является директория, то добавляем к пути, куда надо скопировать, название директории.

                    string nextPath = Path.Combine(destinationPath, new DirectoryInfo(sourcePath).Name);

                    if (String.Equals(sourcePath, nextPath) == false)
                    {
                        // Проверяем, существует ли уже такая директория в той  папке куда копируем
                        if (Directory.Exists(nextPath) == false)
                        {
                            // Если нет, то создаем ее
                            try
                            {
                                DisplayCopyMessage(new DirectoryInfo(sourcePath).Name, destinationPath);

                                Directory.CreateDirectory(nextPath);

                                string[] dirs = null;
                                string[] files = null;

                                // Получаем список поддиректорий
                                try
                                {
                                    dirs = Directory.GetDirectories(sourcePath);
                                }
                                catch (Exception e)
                                {
                                    result = false;
                                    ErrorHandler(new List<string> { " ", "Ошибка доступа к директории", $"{sourcePath} ", $"Ошибка: {e.Message}", " " });
                                }

                                // Получаем список файлов в директории
                                try
                                {
                                    files = Directory.GetFiles(sourcePath);
                                }
                                catch (Exception e)
                                {
                                    result = false;
                                    ErrorHandler(new List<string> { " ", "Ошибка доступа к файлу", $"{sourcePath} ", $"Ошибка: {e.Message}", " " });
                                }

                                // Если есть поддиректории, то запускаем каопирование для каждой из них
                                if (dirs != null)
                                {
                                    foreach (string dir in dirs)
                                    {
                                        Copy(dir, nextPath, false);
                                    }
                                }

                                // Если есть файлы, то запускаем копирование каждого файла
                                if (files != null)
                                {
                                    foreach (string file in files)
                                    {
                                        Copy(file, nextPath, false);
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                result = false;
                                ErrorHandler(new List<string> { " ", "Ошибка создания директории", $"{nextPath} ", $"Ошибка: {e.Message}", " " });
                            }

                        }
                        else
                        {
                            // Если такая директория уже существует, то выводим сообщение о том, что такая директория уже существует

                            Data.Dialog.Data = new DialogData()
                            {
                                Header = Data.Dialog.Data.Header,
                                Message = new List<string> { " ", "Директория", new DirectoryInfo(sourcePath).Name, "в", destinationPath, "уже существует. Заменить?", " " },
                                Buttons = ButtonFactory.GetButtons(new List<ButtonType>() { ButtonType.Skip, ButtonType.Replace }, ButtonType.Skip)
                            };

                            bool replace = doReplace || Data.Dialog.Draw(Data.Dialog.Data);


                            // Если ползователь ответил Заменить, то сначала удаляем существующую папку, а затем запускаем капирование этой папки еще раз
                            if (replace == true)
                            {
                                try
                                {
                                    DisplayDeleteMessage(nextPath);
                                    Directory.Delete(nextPath, true);
                                }
                                catch (Exception e)
                                {
                                    result = false;
                                    ErrorHandler(new List<string> { " ", "Ошибка удаления директории ", $"{nextPath} ", $"Ошибка: {e.Message}", " " });
                                }

                                try
                                {
                                    DisplayCopyMessage(sourcePath, destinationPath);
                                    Copy(sourcePath, destinationPath, true);
                                }
                                catch (Exception e)
                                {
                                    result = false;
                                    ErrorHandler(new List<string> { " ", "Ошибка копирования директории ", $"{nextPath} ", $"Ошибка: {e.Message}", " " });
                                }
                            }
                            else
                            {
                                result = false;
                            }
                        }
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        private bool Copy(FOData data)
        {
            if (data != null)
            {
                DisplayCopyMessage(data.SourcePath, data.DestinationPath);
                return Copy(data.SourcePath, data.DestinationPath, data.DoSilent);
            }
            return false;
        }

        /// <summary>
        /// Вывод сообщения в диалоговое окно о удалении файла/папки
        /// </summary>
        /// <param name="source">путь к удаляемемому элементу</param>
        private void DisplayDeleteMessage(string source)
        {
            Data.Dialog.Data = new DialogDeleteTemplate(source, false);
            Data.Dialog.RefreshContent();
        }

        /// <summary>
        /// Вывод сообщения в диалоговое окно о копировании файла/папки
        /// </summary>
        /// <param name="source">откуда копируется</param>
        /// <param name="destination">куда копируется</param>
        private void DisplayCopyMessage(string source, string destination)
        {
            Data.Dialog.Data = new DialogCopyTemplate(source, destination, false);
            Data.Dialog.RefreshContent();
        }
    }
}
