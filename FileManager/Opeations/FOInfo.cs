using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileManager
{
    /// <summary>
    /// Получение информации о папке (список папок, файлов и общий размер папки )
    /// </summary>
    public class FOInfo : FOBase
    {
        public FolderInfo FolderInfo { get;  private set; } = new FolderInfo();

        public FOInfo(FOData data)
        {
            if (data != null)
            {
                base.Data = data;
            }
        }

        public override bool Execute()
        {
            return GetFolderInfo(base.Data);
        }

        /// <summary>
        /// Получение информации о папке
        /// </summary>
        /// <param name="path">путь к папке</param>
        /// <returns></returns>
        private bool GetFolderInfo(string path)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(path) == false)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        FolderInfo.Space += new FileInfo(path).Length;
                        DisplayInfoMessage();
                    }
                    catch (Exception e)
                    {
                        result = false;
                        ErrorHandler(new List<string>() { " ", "Ошибка получения информации о файле ", $"{path} ", $"Ошибка: {e.Message}", " " });
                    }
                }
                else if (Directory.Exists(path))
                {
                    try
                    {
                        string[] dirs = Directory.GetDirectories(path);

                        FolderInfo.Dirs += dirs.Length;
                        DisplayInfoMessage();

                        foreach (var dir in dirs)
                        {
                            result = GetFolderInfo(dir);
                            DisplayInfoMessage();
                        }
                    }
                    catch (Exception e)
                    {
                        result = false;
                        ErrorHandler(new List<string>() { " ", "Ошибка обработки пути ", $"{path} ", $"Ошибка: {e.Message}", " " });
                    }

                    try
                    {
                        string[] files = Directory.GetFiles(path);

                        FolderInfo.Files += files.Length;
                        DisplayInfoMessage();

                        foreach (var file in files)
                        {
                            FolderInfo.Space += new FileInfo(file).Length;
                            DisplayInfoMessage();
                        }
                    }
                    catch (Exception e)
                    {
                        result = false;
                        ErrorHandler(new List<string>() { " ", "Ошибка получения информации о файле ", $"{path} ", $"Ошибка: {e.Message}", " " });
                    }
                }

            }

            return result;
        }

        private bool GetFolderInfo(FOData data)
        {
            bool result = false;

            if (data != null)
            {
                result = GetFolderInfo(data.SourcePath);
                DisplayInfoQuestion();
            }
            return result;
        }

        /// <summary>
        /// Вывод информации о папки в диалоговое окно
        /// </summary>
        private void DisplayInfoMessage()
        {
            Data.Dialog.Data = new DialogInfoTemplate(FolderInfo, false);
            Data.Dialog.RefreshContent();
        }

        /// <summary>
        /// Вывод информации о папки в диалоговое окно с подтверждением от пользователя
        /// </summary>
        private void DisplayInfoQuestion()
        {            
            Data.Dialog.Data = new DialogInfoTemplate(FolderInfo, true);
            Data.Dialog.Draw(Data.Dialog.Data);
        }

    }
}
