using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Переименоввывает папку/файл
    /// </summary>
    public class FORename : FOBase
    {
        public FORename(FOData data)
        {
            if (data != null)
            {
                base.Data = data;
            }
        }

        public override bool Execute()
        {
            return Rename(base.Data);
        }

        /// <summary>
        /// Переимоввываем папку/файл
        /// </summary>
        /// <param name="sourcePath">старое название</param>
        /// <param name="destinationPath">новое название</param>
        /// <returns></returns>
        private bool Rename(string sourcePath, string destinationPath)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(sourcePath) == false)
            {
                if ((Directory.Exists(sourcePath) || File.Exists(sourcePath)) && Directory.Exists(destinationPath) == false)
                {
                    try
                    {
                        Directory.Move(sourcePath, destinationPath);
                    }
                    catch (Exception e)
                    {
                        result = false;
                        ErrorHandler(new List<string>() { " ", "Ошибка переименования каталога (файла)", $"{sourcePath}", "в", $"{destinationPath}", $"Ошибка: {e.Message}", " " });
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        private bool Rename(FOData data)
        {
            if (data != null)
            {
                DisplayRenameMessage(data.SourcePath, data.DestinationPath);
                return Rename(data.SourcePath, data.DestinationPath);
            }
            return false;
        }

        /// <summary>
        /// Вывод сообщения в диалоговое окно о переименовании
        /// </summary>
        /// <param name="source">старое название</param>
        /// <param name="destination">новое название</param>
        private void DisplayRenameMessage(string source, string destination)
        {
            Data.Dialog.Data = new DialogRenameTemplate(source, false);
            Data.Dialog.RefreshContent();
        }
    }
}
