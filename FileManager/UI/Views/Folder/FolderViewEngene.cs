using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Класс работы с деревом каталогов
    /// </summary>
    public class FolderViewEngene : IDraw, IContent, IConsoleCommand
    {
        // UI элемент показа дерева каталогов в консоли 
        public UIFolderView FolderView { get; private set; }

        // Список элементов папки
        public List<FolderItem> FolderContent { get; set; }

        // Навигационный индекс по данным дерева каталогов
        public FolderNavigation NavIndex { get; private set; } = new FolderNavigation();

        // Путь к отображаемой директории
        public string DirectoryPath { get; private set; }

        // Показывает, активна ли в данный момент панель в консоли
        public bool IsActive 
        { 
            get { return FolderView?.IsActive ?? false; }
            set
            {
                if (FolderView != null)
                {
                    FolderView.IsActive = value;
                }
            } 
        }

        /// <summary>
        /// Создает панель каталога
        /// </summary>
        /// <param name="factory">Фабрика для создания рамки панели дерева каталогов</param>
        /// <param name="path">первоначальный путь к каталогу, отображаемому в панели</param>
        /// <param name="isActive">флаг активности панели</param>
        public FolderViewEngene(UIFactory<UIFolderView> factory, string path, bool isActive)
        {
            DirectoryPath = path;
            FolderView = factory.CreateView();
            IsActive = isActive;
            Draw();
            UpdateContent();
        }

        /// <summary>
        /// Рисует рамку панели дерева катлогов
        /// </summary>
        public bool Draw()
        {
            return FolderView?.Draw() ?? false;
        }

        /// <summary>
        /// Обновление содержимого в панели каталога
        /// </summary>
        public void RefreshContent()
        {
            FolderView.BodyContent = UpdateNavigationIndex();
            FolderView.RefreshContent();
            SetCursorToDefaultPosition();
        }

        /// <summary>
        /// Запрос содержимого каталога и обновление содержимого панели в консоли
        /// В качестве пути к каталогу использует DirectoryPath
        /// </summary>
        public void UpdateContent()
        {
            UpdateContent(DirectoryPath);
        }

        /// <summary>
        /// Запрос содержимого каталога и обновление содержимого панели в консоли
        /// </summary>
        /// <param name="path">путь к каталогу</param>
        public void UpdateContent(string path)
        {
            // получаем элементы выбранной папки, если файл, то выполняем его
            List<FolderItem> _contentNew = FolderHelper.GetDirectory(path);

            // Если получили обновленные данные, сохраняем их для данной панели и перерисовываем.
            if (_contentNew != null)
            {
                string lastFolderPath = DirectoryPath;
                DirectoryPath = path;
                FolderContent = _contentNew;

                NavIndex.FirstItem = 0;
                NavIndex.LastItem = 0;

                NavIndex.SelectedItem = FolderContent.FindIndex(x => x.Path == lastFolderPath);
                NavIndex.SelectedItem = NavIndex.SelectedItem == -1 ? 0 : NavIndex.SelectedItem;

                FolderView.HeaderContent = DirectoryPath;
                FolderView.BodyContent = UpdateNavigationIndex();

                ClearContent();
                RefreshContent();
            }
        }

        /// <summary>
        /// Поиск названия файлов и папок по заданным параметрам
        /// </summary>
        /// <param name="searchOption">параметр поиска</param>
        public void FindContent(string searchOption)
        {
            string root = Directory.GetDirectoryRoot(DirectoryPath);
            FindContent(root,searchOption);
        }

        public void FindContent(string path, string searchOption)
        {
            // получаем элементы выбранной папки, если файл, то выполняем его
            List<FolderItem> _contentNew = FolderHelper.Search(path, DirectoryPath, searchOption);

            // Если получили обновленные данные, сохраняем их для данной панели и перерисовываем.
            if (_contentNew != null)
            {
                string lastFolderPath = DirectoryPath;
                DirectoryPath = path;
                FolderContent = _contentNew;

                NavIndex.FirstItem = 0;
                NavIndex.LastItem = 0;

                NavIndex.SelectedItem = FolderContent.FindIndex(x => x.Path == lastFolderPath);
                NavIndex.SelectedItem = NavIndex.SelectedItem == -1 ? 0 : NavIndex.SelectedItem;

                FolderView.HeaderContent = DirectoryPath;
                FolderView.BodyContent = UpdateNavigationIndex();

                //ClearContent();
                Draw();
                RefreshContent();
            }
        }

        /// <summary>
        /// Очищает сожедржимое в панелях
        /// </summary>
        public void ClearContent()
        {
            FolderView?.ClearContent();
            SetCursorToDefaultPosition();
        }

        /// <summary>
        /// Заполнение панели дерева каталогов данными текущей папки (папки, файлы и информация о них)
        /// </summary>
        public List<FolderItem> UpdateNavigationIndex()
        {
            // Проверяем, что массив элементов не пустой
            if (FolderContent != null)
            {
                 FolderContent.Where(x => x.IsActive == true).Select(x => x.IsActive = false).ToList();

                // Рассчитываем, какие элементы массива _content выводить в консоль
                if ((NavIndex.FirstItem == 0 && NavIndex.LastItem == 0) || (NavIndex.FirstItem > NavIndex.LastItem))
                {
                    // Если индексы первого и последнего отображаемого элемента равны 0 или индекс первого отображаемого элемента больше последнего, то
                    // проверяем, что индекс выбранного элемента меньше или равен максимальному количеству строк в панели дерева каталогов
                    if (NavIndex.SelectedItem <= FolderView.Body.Size.Height)
                    {
                        // Если это так, то индекс первого отображаемого элемента равен 0
                        NavIndex.FirstItem = 0;

                        // Индекс последнего элемента либо равен количеству строк в панели, либо количеству элементов в папке, в зависимости от того, что меньше
                        NavIndex.LastItem = FolderContent.Count > FolderView.Body.Size.Height ? FolderView.Body.Size.Height : FolderContent.Count;
                    }
                    else
                    {
                        // Если индекс выбранного элемент больше, то
                        // индекс первого отображаемого элемента равен разнице между индексом выбранного элемента и количеством строк в панели плюс 1     
                        NavIndex.FirstItem = NavIndex.SelectedItem - FolderView.Body.Size.Height + 1;
                        // Индекс последнего элемента равен индексу выбранного элемента плюс 1
                        NavIndex.LastItem = NavIndex.SelectedItem + 1;
                    }
                }
                else
                {
                    if (NavIndex.SelectedItem < NavIndex.FirstItem)
                    {
                        // Если индекс выбранного элемента меньше индекса первого элемента отображения, то уменьшаем индекс первого и последнего элемента отображение на 1
                        NavIndex.FirstItem--;
                        NavIndex.LastItem--;
                    }
                    else if (NavIndex.LastItem <= NavIndex.SelectedItem)
                    {
                        // Если индекс выбранного элемента больше или равен индексу последнего элемента отображения, то увеличиваем индекс первого и последнего элемента отображение на 1
                        NavIndex.FirstItem++;
                        NavIndex.LastItem++;
                    }
                }

                // Если индекс выбранного элемента оказался больше индекса последнего отображаемого элемента, то исправляем данную ситуацию
                if (NavIndex.SelectedItem > NavIndex.LastItem)
                {
                    NavIndex.SelectedItem = NavIndex.LastItem > 0 ? NavIndex.LastItem - 1 : 0;
                }

                FolderContent[NavIndex.SelectedItem].IsActive = true;

                return FolderContent.GetRange(NavIndex.FirstItem, NavIndex.LastItem- NavIndex.FirstItem);
            }
            else
            {
                return null;
            }
        }

        public void SetCursorToDefaultPosition()
        {
            try
            {
                App.globalMessages["CursorDefault"].Processing();
            }
            catch
            {

            }

        }

        public void CommandExecute(ConsoleKey key)
        {
            switch (key)
            {
                // Переход по дереву каталогов (файлов) вверх 
                case (ConsoleKey.UpArrow):
                    {

                        // Если активна левая панель, то проверяем, что индекс выбранного элемента > 0
                        if (NavIndex.SelectedItem > 0)
                        {
                            // Если индекс выбранного элемента > 0, то уменьшаем его и перерисовываем левую панель
                            --NavIndex.SelectedItem;
                            RefreshContent();
                        }
                        else
                        {
                            NavIndex.SelectedItem = 0;
                        }

                    }
                    break;

                // Переход по дереву каталогов (файлов) вниз
                case (ConsoleKey.DownArrow):
                    {

                        // Если активна левая панель, то проверяем, что индекс выбранного элемента > индекса последнего элемента
                        if (NavIndex.SelectedItem < FolderContent.Count - 1)
                        {
                            // Если индекс меньше, то увеличиваем его и перерисовываем левую панель
                            ++NavIndex.SelectedItem;
                            RefreshContent();
                        }

                    }
                    break;

                // Переход вначало дерева каталогов
                case (ConsoleKey.LeftArrow):
                    {

                        // Сбрасываем индексы и перерисовываем паель
                        NavIndex.SelectedItem = 0;
                        NavIndex.FirstItem = 0;
                        NavIndex.LastItem = 0;
                        RefreshContent();


                    }
                    break;

                // Переход в конец дерева каталогов
                case (ConsoleKey.RightArrow):
                    {

                        // Сбрасываем индексы первого и последнего элементы для вывода
                        // Индеску выбранного элемента присваиваем индекс последнего элемента и перерисоваваем панель
                        NavIndex.SelectedItem = FolderContent.Count - 1;
                        NavIndex.FirstItem = 0;
                        NavIndex.LastItem = 0;
                        RefreshContent();

                    }
                    break;

                // Открытие папки или файла
                case (ConsoleKey.Enter):
                    {
                        if (FolderContent[NavIndex.SelectedItem].Type == FolderItemType.File)
                        {
                            Process process = new Process();

                            process.StartInfo.FileName = FolderContent[NavIndex.SelectedItem].Path;
                            //process.StartInfo.UseShellExecute = false;
                            process.StartInfo.CreateNoWindow = false;
                            process.Start();
                        }
                        else
                        {

                            UpdateContent(FolderContent[NavIndex.SelectedItem].Path);
                        }
                    }
                    break;
                default:
                    {
                        RefreshContent();
                    }
                    break;
            }
        }

    }
}
