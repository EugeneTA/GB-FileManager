using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Перемещает папку/файл из одной директории в другую
    /// </summary>
    class FOMove : FOBase
    {
        public FOMove(FOData data)
        {
            if (data != null)
            {
                base.Data = data;
            }
        }

        public override bool Execute()
        {
            return Move(base.Data);
        }

        /// <summary>
        /// Перемещает папку/файл в новую директорию
        /// </summary>
        /// <param name="sourcePath">Путь к папке/файлу, которую пермещаем</param>
        /// <param name="destinationPath">Путь, куда перемещаем</param>
        /// <returns></returns>
        private bool Move(string sourcePath, string destinationPath)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(sourcePath) == false)
            {
                if ((Directory.Exists(sourcePath) || File.Exists(sourcePath)) && Directory.Exists(destinationPath))
                {
                    try
                    {
                        FOData data = new FOData
                        {
                            SourcePath = sourcePath,
                            DestinationPath = destinationPath,
                            DoSilent = false,
                            Dialog = Data.Dialog,
                            ErrorLogger = Data.ErrorLogger
                        };

                        FOBase operation = new FOCopy (data);
                        result = operation.Execute();

                        if (result == true)
                        {
                            data.DoSilent = true;
                            operation = new FODelete(data);
                            result = operation.Execute();
                        }
                    }
                    catch (Exception e)
                    {
                        result = false;
                        ErrorHandler(new List<string>() { " ", "Ошибка перемещения каталога", $"{sourcePath}", "в", $"{destinationPath}", $"Ошибка: {e.Message}", " " });
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }


        private bool Move(FOData data)
        {
            if (data != null)
            {
                DisplayMoveMessage(data.SourcePath, data.DestinationPath);
                return Move(data.SourcePath, data.DestinationPath);
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
        /// Вывод сообщения в диалоговое окно о перемещении объекта
        /// </summary>
        /// <param name="source">откуда перемещаем</param>
        /// <param name="destination">куда перемещаем</param>
        private void DisplayMoveMessage(string source, string destination)
        {
            Data.Dialog.Data = new DialogMoveTemplate(source, destination, false);
            Data.Dialog.RefreshContent();
        }
    }
}
