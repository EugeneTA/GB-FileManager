using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FileManager
{
    public static class FolderHelper
    {
        /// <summary>
        /// Получение структуры выбранной папки.
        /// Если заданный путь директория - то получаем структуру директории и возвращаем ее
        /// Если заданный путь файл - то возвращаем null
        /// Во всех других случаях возаращаем массив доступных дисков
        /// Если не удалось получить структуру директории, то возвращает null 
        /// </summary>
        /// <param name="path">Путь к папке</param>
        /// <returns></returns>
        public static List<FolderItem> GetDirectory(string path)
        {
            List<FolderItem> dirList = new List<FolderItem>();
            FolderItem dirItems = new FolderItem();


            // Проверяем, что такая директория существует
            if (Directory.Exists(path))
            {
                try
                {
                    // Если директория существует, то получаем информацию о ней
                    DirectoryInfo root = new DirectoryInfo(path);

                    // Если null, то значит корневая директория и надо показывать диски.
                    if (root == null)
                    {
                        DriveInfo[] drives = DriveInfo.GetDrives();

                        if (drives != null)
                        {
                            for (int i = 0; i < drives.Length; i++)
                            {
                                dirList.Add(new FolderItem()
                                {
                                    Name = drives[i].Name,
                                    Path = drives[i].RootDirectory.FullName,
                                    Size = StringHelper.FileSizeToString(drives[i].AvailableFreeSpace),
                                    Date = StringHelper.FileSizeToString(drives[i].TotalSize),
                                    Attr = "",
                                    IsActive = false,
                                    Type = FolderItemType.Drive
                                });
                            }
                        }
                        //rootPath = "";
                    }
                    else
                    {
                        // иначе показываем структуру папки

                        string[] dir = null;    // массив имен директорий
                        string[] files = null;  // массив имен файлов

                        // Получаем названия папок и файлов в директории
                        dir = Directory.GetDirectories(path);
                        files = Directory.GetFiles(path);

                        // Если текущяя директория не является корневой, то в первом элементе запоминаем путь к папке для выхода выше
                        if (root.Parent != null)
                        {
                            dirList.Add(new FolderItem()
                            {
                                Name = "\\..",
                                Path = root.Parent.FullName,
                                Size = "",
                                Date = "",
                                Attr = "",
                                IsActive = false,
                                Type = FolderItemType.Return
                            });
                        }
                        else
                        {
                            // Если текущий каталог является корневым, то путь возврата будет пустым. Тип элемента - Диск
                            dirList.Add(new FolderItem()
                            {
                                Name = "\\..",
                                Path = "",
                                Size = "",
                                Date = "",
                                Attr = "",
                                IsActive = false,
                                Type = FolderItemType.Return
                            });
                        }

                        // Если в директории найдены папки, то заполняем информационный массив
                        if (dir != null)
                        {
                            for (int i = 0; i < dir.Length; i++)
                            {
                                DirectoryInfo dirInfo = new DirectoryInfo(dir[i]);

                                dirList.Add(new FolderItem()
                                {
                                    Name = dirInfo.Name,
                                    Path = dirInfo.FullName,
                                    Size = "",
                                    Date = dirInfo.CreationTime.ToString("d"),
                                    Attr = "",
                                    IsActive = false,
                                    Type = FolderItemType.Folder
                                });
                            }

                        }

                        // Если в директории найдены файлы, то заполняем информационный массив
                        if (files != null)
                        {
                            for (int i = 0; i < files.Length; i++)
                            {
                                FileInfo fileInfo = new FileInfo(files[i]);

                                dirList.Add(new FolderItem()
                                {
                                    Name = fileInfo.Name,
                                    Path = fileInfo.FullName,
                                    Size = StringHelper.FileSizeToString(fileInfo.Length),
                                    Date = fileInfo.CreationTime.ToString("d"),
                                    Attr = GetAttributes(fileInfo),
                                    IsActive = false,
                                    Type = FolderItemType.File

                                }); ;
                            }
                        }

                    }

                    return dirList;
                }

                catch (Exception e)
                {
                    //ErrorLog(errorLogFilename, $"Ошибка доступа к директории. {e.Message}");
                    return null;
                }
            }
            else if (File.Exists(path))
            {
                return null;
            }
            else
            {
                if (String.IsNullOrEmpty(path))
                {
                    DriveInfo[] drives = DriveInfo.GetDrives();

                    if (drives != null)
                    {
                        for (int i = 0; i < drives.Length; i++)
                        {
                            dirList.Add(new FolderItem()
                            {
                                Name = drives[i].Name,
                                Path = drives[i].RootDirectory.FullName,
                                Size = StringHelper.FileSizeToString(drives[i].AvailableFreeSpace),
                                Date = StringHelper.FileSizeToString(drives[i].TotalSize),
                                Attr = "",
                                IsActive = false,
                                Type = FolderItemType.Drive
                            });
                        }

                        return dirList;
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// Поиск файла или папки по названию
        /// </summary>
        /// <param name="searchPath">Путь к папка, в которой искать</param>
        /// <param name="returnPath">Путь к папке, в которую вернуться из резульататов поиска</param>
        /// <param name="searchOption">Параметры поиска. Звездочка заменяет любой количество символов</param>
        /// <returns>Возвращает список найденных объектов. Первым элементом списка будет возврат в папку, указанную в параметре returnPath</returns>
        public static List<FolderItem> Search(string root, string returnPath, string searchOption)
        {
            Stack<string> dirs = new Stack<string>(20);
            List<FolderItem> dirList = new List<FolderItem>();
            List<FolderItem> filesList = new List<FolderItem>();


            // Добавляем путь возврата из резульататов поиска
            dirList.Add(new FolderItem()
            {
                Name = "\\..",
                Path = returnPath,
                Size = "",
                Date = "",
                Attr = "",
                IsActive = false,
                Type = FolderItemType.Return
            });


            if (Directory.Exists(root))
            {
                dirs.Push(root);

                while (dirs.Count > 0)
                {
                    string currentDir = dirs.Pop();
                    
                    string[] subDirs;
                    string[] searchDirs;

                    try
                    {
                        // Получаем массов поддиректорий
                        subDirs = System.IO.Directory.GetDirectories(currentDir);
                        // Получаем массив поддиректорий, чьи названия удовлетворяют параметрам поиска 
                        searchDirs = Directory.GetDirectories(currentDir, searchOption);
                    }
                    catch
                    {
                        // В случае ошибки продолжаем дальше
                        continue;
                    }

                    string[] files = null;
                    string[] searchFiles = null;

                    try
                    {
                        // Получаем массив файлов в данной директории
                        files = System.IO.Directory.GetFiles(currentDir);
                        // Получаем массив файлов в данной директории чьи названия удоавлетворяют параметрам поиска
                        searchFiles = Directory.GetFiles(currentDir, searchOption);
                    }

                    catch
                    {
                        continue;
                    }

                    // Найденные файлы, удовлетворяющие параметрам поиска, добавляем в список найденных файлов 
                    foreach (string file in searchFiles)
                    {
                        try
                        {
                            FileInfo fileInfo = new FileInfo(file);

                            filesList.Add(new FolderItem()
                            {
                                Name = fileInfo.Name,
                                Path = fileInfo.FullName,
                                Size = StringHelper.FileSizeToString(fileInfo.Length),
                                Date = fileInfo.CreationTime.ToString("d"),
                                Attr = GetAttributes(fileInfo),
                                IsActive = false,
                                Type = FolderItemType.File

                            });
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    // Найденные папки, удовлетворяющие параметрам поиска, добавляем в список найденных папок 
                    foreach (string dir in searchDirs)
                    {
                        try
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(dir);

                            dirList.Add(new FolderItem()
                            {
                                Name = dirInfo.Name,
                                Path = dirInfo.FullName,
                                Size = "",
                                Date = dirInfo.CreationTime.ToString("d"),
                                Attr = "",
                                IsActive = false,
                                Type = FolderItemType.Folder
                            });
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    // Все найденые поддиректории добавляем в стек для прохода по ним 
                    foreach (string str in subDirs)
                        dirs.Push(str);
                }
            }

            // Объединяем списки найденных директорий и файлов и возвращаем результат
            dirList.AddRange(filesList);

            return dirList;
        }

        /// <summary>
        /// Возвращает список аттрибутов файла в виде строки
        /// </summary>
        /// <param name="fileInfo">элемент каталога</param>
        /// <returns></returns>
        private static string GetAttributes(FileInfo fileInfo)
        {
            if (fileInfo != null)
            {
                // Получаем атрибуты элемента
                FileAttributes fileAttributes = fileInfo.Attributes;
                StringBuilder result = new StringBuilder();

                result.Append(fileAttributes.HasFlag(FileAttributes.Archive) ? " A" : "  ");
                result.Append(fileAttributes.HasFlag(FileAttributes.System) ? " S" : "  ");
                result.Append(fileAttributes.HasFlag(FileAttributes.Hidden) ? " H" : "  ");
                result.Append(fileAttributes.HasFlag(FileAttributes.ReadOnly) ? " R" : "  ");
                result.Append(fileAttributes.HasFlag(FileAttributes.Encrypted) ? " E" : "  ");

                return result.ToString();
            }

            return "";
        }

    }
}
