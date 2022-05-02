
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileManager
{
    /// <summary>
    /// Осуществляет удаление папки/файла
    /// </summary>
    public class FODelete : FOBase
    {
        public FODelete(FOData data)
        {
            if (data != null)
            {
                base.Data = data;
            }
        }

        public override bool Execute()
        {
            return Delete(base.Data);
        }

        /// <summary>
        /// Удаление файла или папки
        /// </summary>
        /// <param name="deletePath">Путь к папке (файлу)</param>
        /// <param name="doSilent">если true, то не выводит сообщение об удалении</param>
        private bool Delete(string deletePath, bool doSilent)
        {
            bool result = true;
            
            if (string.IsNullOrWhiteSpace(deletePath) == false)
            {
                // Если указанный путь является директорией
                if (Directory.Exists(deletePath))
                {
                    string[] dirs = null;
                    string[] files = null;

                    // Получаем список поддиректорий
                    try
                    {
                        dirs = Directory.GetDirectories(deletePath);
                    }
                    catch (Exception e)
                    {
                        result = false;
                        ErrorHandler(new List<string> { " ", "Ошибка доступа к директории", $"{deletePath} ", $"Ошибка: {e.Message}", " " });
                    }

                    // Получаем список файлов
                    try
                    {
                        files = Directory.GetFiles(deletePath);
                    }
                    catch (Exception e)
                    {
                        result = false;
                        ErrorHandler(new List<string> { " ", "Ошибка доступа к файлу", $"{deletePath} ", $"Ошибка: {e.Message}", " " });
                    }

                    // Если в папке есть директории, то запускаем их удаление
                    if (dirs != null)
                    {
                        foreach (string dir in dirs)
                        {
                            result = Delete(dir, doSilent);
                        }
                    }

                    // Если в папке есть файлы, то запускаем их удаление
                    if (files != null)
                    {
                        foreach (string file in files)
                        {
                            result = Delete(file, doSilent);
                        }
                    }

                    // Удаляем пустую директорию только если в ней удалены все файлы и подкаталоги
                    if (result)
                    {
                        try
                        {
                            Directory.Delete(deletePath);
                        }
                        catch (Exception e)
                        {
                            result = false;
                            ErrorHandler(new List<string> { " ", "Ошибка удаления директории", $"{deletePath} ", $"Ошибка: {e.Message}", " " });
                        }
                }
            }
                else if (File.Exists(deletePath))
                {
                    bool doDelete = doSilent || DisplayDeleteQuestion(deletePath);

                    if (doDelete)
                    {
                        try
                        {
                            DisplayDeleteMessage(deletePath);
                            File.Delete(deletePath);
                        }
                        catch (Exception e)
                        {
                            result = false;
                            ErrorHandler(new List<string> { " ", "Ошибка удаления файла", $"{deletePath} ", $"Ошибка: {e.Message}", " " });
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Удаление файла или папки
        /// </summary>
        /// <param name="data">Объект с информацией необходиомй для выполнения операции</param>
        /// <returns></returns>
        private bool Delete(FOData data)
        {
            if (data != null)
            {
                Data.Dialog.Data = Data.Dialog.Data = new DialogDeleteTemplate(data.SourcePath, false);
                Data.Dialog.Draw();
                return Delete(data.SourcePath, data.DoSilent);
            }
            return false;
        }

        /// <summary>
        /// Вывод сообщения в диалоговое окно информацию о удалении файла/папки
        /// </summary>
        /// <param name="source">путь к удаляемемому элементу</param>
        private void DisplayDeleteMessage(string source)
        {
            Data.Dialog.Data = new DialogDeleteTemplate(source, false);
            Data.Dialog.RefreshContent();
        }

        /// <summary>
        /// Вывод сообщения в диалоговое окно запрос на удаление файла/папки
        /// </summary>
        /// <param name="source">путь к удаляемемому элементу</param>
        private bool DisplayDeleteQuestion(string source)
        {
            return Data.Dialog.Draw(new DialogDeleteTemplate(source, true));
        }
    }
}
