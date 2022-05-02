using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    public class App : IConsoleCommand
    {
        public static Dictionary<string, IMessage> globalMessages = new Dictionary<string, IMessage>();

        // Настройки приложения
        AppSettings _appSettings;

        // Настройки графического инрефейса
        UISettings _uiSettings;

        // объект с данными, передаваемый в диалоговое окно
        DialogData _dialogData;

        // ДИалоговое окно
        UIDialogView _defaultUIDialog;

        // Логгер ошибок
        IErrorLog _errorLogger;

        // Левая панель отображения дерева каталогов
        FolderViewEngene _leftFolder;

        // Правая панель отображения дерева каталогов
        FolderViewEngene _rightFolder;

        // Информационная панель
        UIInfoView _info;

        // Флаг активности левой панели. Если false, то активна правая панель
        bool _leftActive = true;

        public App(AppSettings appSettings, IErrorLog errorLogger)
        {
            _appSettings = appSettings;
            _errorLogger = errorLogger;
            globalMessages.Add("CursorDefault", new SetCursorToDefaultPosition());
            CalculateUI();
        }

        /// <summary>
        /// Создаем объекты графического интерфейса и выводим их в консоли
        /// </summary>
        public void Draw()
        {
            int lineWidth = 1;

            if (_errorLogger == null)
            {
                _errorLogger = new ErrorLogToFile();
            }

            // Создаем объект диалогового окна 
            DialogViewFactory dialogFabric = new DialogViewFactory(_uiSettings.DialogView.Position, _uiSettings.DialogView.Size, lineWidth);
            _defaultUIDialog = dialogFabric.CreateView();
            

            // Создаем объекты панелей просмотра дерева каталогов
            FolderViewFactory folderFabric = new FolderViewFactory(_uiSettings.FolderView.Position, _uiSettings.FolderView.Size, lineWidth);
            _leftFolder = new FolderViewEngene(folderFabric, _appSettings.Settings.leftFolderPath, _leftActive);

            folderFabric = new FolderViewFactory(_uiSettings.FolderView.Position + _uiSettings.RightFolderOffset.Position, _uiSettings.FolderView.Size, lineWidth);
            _rightFolder = new FolderViewEngene(folderFabric, _appSettings.Settings.rightFolderPath, !_leftActive);

            // Создаем информационную панель
            List<string> infoData = new List<string>()
            {
                "F1 - Помощь",
                "F2 - Диск",
                "F3 - Свойства",
                "F4 - Переименовать",
                "F5 - Копировать",
                "F6 - Переместить",
                "F7 - Создать",
                "F8 - Удалить",
                "F9 - Поиск",
                "F10 - Выход"
            };

            InfoViewFactory uiInfoFactory = new InfoViewFactory(_uiSettings.InfoView.Position, _uiSettings.InfoView.Size, lineWidth, infoData);
            _info = uiInfoFactory.CreateView();
            _info.Draw();
            _info.RefreshContent();
        }


        /// <summary>
        /// Рассичтывает размеры UI для текущего окна и сохраняет их в структуре uiDimensions для дальнейшего использования
        /// Так же рассчитывает положения курсоров для правой и левой панелей и сохраняет их в 
        /// </summary>
        private void CalculateUI()
        {
            Dimensions dimensions;
            Coordinates position;

            _uiSettings = new UISettings();

            // Сохраняем высоту и ширину окна для дальнейших вычислений
            dimensions = new Dimensions();
            dimensions.Width = Console.WindowWidth % 2 > 0 ? Console.WindowWidth - 3 : Console.WindowWidth - 4;
            dimensions.Height = Console.WindowHeight % 2 > 0 ? Console.WindowHeight - 1 : Console.WindowHeight;


            //// Устанавливаем размер окна
            //Console.SetWindowSize(dimensions.Height, dimensions.Width);

            position =  new Coordinates(1, 1);
            _uiSettings.UIDimension = new UIBase(position, dimensions);



            // Рассчитываем ширину и высоту панели отображения дерева каталогов
            dimensions = new Dimensions();

            dimensions.Width = (_uiSettings.UIDimension.Size.Width / 2) - 1;
            dimensions.Height = _uiSettings.UIDimension.Size.Height - 8;

            position = new Coordinates(1, 1);

            _uiSettings.FolderView = new UIBase(position, dimensions);

            dimensions = new Dimensions(0,0);
            position = new Coordinates(_uiSettings.FolderView.Size.Width + 1, 0);

            _uiSettings.RightFolderOffset = new UIBase(position, dimensions);


            dimensions = new Dimensions();
            dimensions.Height = 5;
            dimensions.Width = _uiSettings.RightFolderOffset.Position.Left + _uiSettings.FolderView.Size.Width;
            position = new Coordinates(_uiSettings.FolderView.Position.Left, _uiSettings.FolderView.Position.Top + _uiSettings.FolderView.Size.Height);
            _uiSettings.InfoView = new UIBase(position, dimensions);


            dimensions = new Dimensions();
            dimensions.Width = (_uiSettings.UIDimension.Size.Width / 3) > 2 ? (_uiSettings.UIDimension.Size.Width / 3) - 2 : 0;
            dimensions.Height = (_uiSettings.UIDimension.Size.Height / 3) > 2 ? (_uiSettings.UIDimension.Size.Height / 3) - 2 : 0;
            position = new Coordinates(dimensions.Width + 1, dimensions.Height + 1);
            _uiSettings.DialogView = new UIBase(position, dimensions);

        }


        /// <summary>
        /// Обработка команд, поступающих от пользователя
        /// </summary>
        /// <param name="key">Горячая клавиша</param>
        public void CommandExecute(ConsoleKey key)
        {
            switch (key)
            {
                // Help
                case ConsoleKey.F1:
                    {
                        // Создаем диалоговое окно справки
                        _defaultUIDialog.Draw(new DialogHelpTemplate());

                        // очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();
                        _leftFolder.Draw();
                        _leftFolder.RefreshContent();
                        _rightFolder.Draw();
                        _rightFolder.RefreshContent();
                        _info.Draw();
                        _info.RefreshContent();
                    }
                    break;

                // Drive select
                case ConsoleKey.F2:
                    {
                        // Вызываем список дисков в системе
                        if (_leftActive)
                        {
                            _leftFolder.UpdateContent("");
                            _appSettings.Settings.leftFolderPath = _leftFolder.DirectoryPath;
                        }
                        else
                        {
                            _rightFolder.UpdateContent("");
                            _appSettings.Settings.rightFolderPath = _rightFolder.DirectoryPath;
                        }
                    }
                    break;

                // Folder | File Info
                case ConsoleKey.Spacebar:
                case ConsoleKey.F3:
                    {
                        // Создаем диалоговое окно с информацией о содержимом папки
                        _defaultUIDialog.Draw(new DialogInfoTemplate(null, false));

                        // то создаем блок данных для операции получения информации о папке
                        FOData data = new FOData
                        {
                            SourcePath = _leftActive ? _leftFolder.FolderContent[_leftFolder.NavIndex.SelectedItem].Path : _rightFolder.FolderContent[_rightFolder.NavIndex.SelectedItem].Path,
                            DestinationPath = "",
                            DoSilent = true,
                            Dialog = _defaultUIDialog,
                            ErrorLogger = _errorLogger
                        };

                        // Создаем экземпляр класса выполнения операции
                        // и выполняем её
                        FOBase operation = new FOInfo(data);
                        operation.Execute();

                        // после выполнения операции создания, очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();

                        _leftFolder.Draw();
                        _leftFolder.RefreshContent();
                        _rightFolder.Draw();
                        _rightFolder.RefreshContent();

                        _info.Draw();
                        _info.RefreshContent();
                    }
                    break;

                // Rename
                case ConsoleKey.F4:
                    {
                        // получаем имя и путь объекта, который хотим переименовать
                        (string sourceName, string sourcePath) =
                            _leftActive ?
                            (
                            _leftFolder.FolderView.BodyContent[_leftFolder.NavIndex.SelectedItem].Name,
                            _leftFolder.FolderView.BodyContent[_leftFolder.NavIndex.SelectedItem].Path
                            )
                            :
                            (
                            _rightFolder.FolderView.BodyContent[_rightFolder.NavIndex.SelectedItem].Name,
                            _rightFolder.FolderView.BodyContent[_rightFolder.NavIndex.SelectedItem].Path
                            );

                        // Создаем блок данных для диалгового окна
                        _dialogData = new DialogRenameTemplate(sourceName, true);

                        bool result = false;

                        // Создаем диалоговое окно и ожидаем ввода нового имени от пользователя
                        if (_defaultUIDialog.Draw(_dialogData))
                        {
                            // Если пользователь подтвердил переименование,
                            // то создаем новый путь на основнии введенного имени
                            string destinationPath =
                                _leftActive ?
                                Path.Combine(_leftFolder.DirectoryPath, _dialogData.InputData)
                                :
                                Path.Combine(_rightFolder.DirectoryPath, _dialogData.InputData);

                            // создаем блок данных для операции переименования
                            FOData data = new FOData
                            {
                                SourcePath = sourcePath,
                                DestinationPath = destinationPath,
                                DoSilent = true,
                                Dialog = _defaultUIDialog,
                                ErrorLogger = _errorLogger
                            };

                            // Создаем экземпляр класса выполнения операции
                            // и выполняем её
                            FOBase operation = new FORename(data);
                            result = operation.Execute();
                        }

                        // после выполнения операции создания, очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();
                        _leftFolder.Draw();
                        _rightFolder.Draw();

                        if (result == true)
                        {
                            _leftFolder.UpdateContent();
                            _rightFolder.UpdateContent();
                        }
                        else
                        {
                            _leftFolder.RefreshContent();
                            _rightFolder.RefreshContent();
                        }

                        _info.Draw();
                        _info.RefreshContent();
                    }
                    break;

                // Copy
                case ConsoleKey.F5:
                    {
                        // получаем путь копируемого объекта, а также путь, куда его переместить
                        (string sourcePath, string destinationPath) =
                            _leftActive ?
                            (
                            _leftFolder.FolderContent[_leftFolder.NavIndex.SelectedItem].Name,
                            _rightFolder.DirectoryPath
                            ) :
                            (
                            _rightFolder.FolderContent[_rightFolder.NavIndex.SelectedItem].Name,
                            _leftFolder.DirectoryPath
                            );

                        // Создаем диалоговое окно и ожидаем подтверждение копирования от пользователя
                        if (_defaultUIDialog.Draw(new DialogCopyTemplate(sourcePath, destinationPath, true)))
                        {
                            // Если пользователь подтвердил копирование,
                            // то создаем блок данных для операции копирования
                            FOData data = new FOData
                            {
                                SourcePath = _leftActive ? _leftFolder.FolderContent[_leftFolder.NavIndex.SelectedItem].Path : _rightFolder.FolderContent[_rightFolder.NavIndex.SelectedItem].Path,
                                DestinationPath = _leftActive ? _rightFolder.DirectoryPath : _leftFolder.DirectoryPath,
                                DoSilent = false,
                                //Buttons = _dialogData.Buttons,
                                Dialog = _defaultUIDialog,
                                ErrorLogger = _errorLogger
                            };

                            // Создаем экземпляр класса выполнения операции
                            // и выполняем её
                            FOBase operation = new FOCopy(data);
                            operation.Execute();
                        }

                        // после выполнения операции создания, очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();
                        _leftFolder.Draw();
                        _rightFolder.Draw();
                        _info.Draw();
                        _info.RefreshContent();

                        if (_leftActive)
                        {
                            _rightFolder.UpdateContent();
                            _leftFolder.RefreshContent();
                        }
                        else
                        {
                            _rightFolder.RefreshContent();
                            _leftFolder.UpdateContent();
                        }

                    }
                    break;

                // Move
                case ConsoleKey.F6:
                    {
                        // получаем имя и путь перемещяемого объекта, а также путь, куда его переместить
                        (string sourceName, string sourcePath, string destinationPath) = _leftActive ?
                            (
                            _leftFolder.FolderView.BodyContent[_leftFolder.NavIndex.SelectedItem].Name,
                            _leftFolder.FolderView.BodyContent[_leftFolder.NavIndex.SelectedItem].Path,
                            _rightFolder.DirectoryPath
                            )
                            :
                            (
                            _rightFolder.FolderView.BodyContent[_rightFolder.NavIndex.SelectedItem].Name,
                            _rightFolder.FolderView.BodyContent[_rightFolder.NavIndex.SelectedItem].Path,
                            _leftFolder.DirectoryPath
                            );

                        bool result = false;

                        // Создаем диалоговое окно и ожидаем подтверждение перемещения от пользователя
                        if (_defaultUIDialog.Draw(new DialogMoveTemplate(sourceName, destinationPath, true)))
                        {
                            // Если пользователь подтвердил перемещение,
                            // то создаем блок данных для операции перемещения
                            FOData data = new FOData
                            {
                                SourcePath = sourcePath,
                                DestinationPath = destinationPath,
                                DoSilent = true,
                                Dialog = _defaultUIDialog,
                                ErrorLogger = _errorLogger
                            };

                            // Создаем экземпляр класса выполнения операции перемещения
                            // и выполняем её
                            FOBase operation = new FOMove(data);
                            result = operation.Execute();

                        }

                        // после выполнения операции создания, очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();

                        _leftFolder.Draw();
                        _rightFolder.Draw();

                        if (result)
                        {
                            _leftFolder.UpdateContent();
                            _rightFolder.UpdateContent();
                        }
                        else if (_leftActive)
                        {
                            _leftFolder.RefreshContent();
                            _rightFolder.UpdateContent();
                        }
                        else
                        {
                            _leftFolder.UpdateContent();
                            _rightFolder.RefreshContent();
                        }

                        _info.Draw();
                        _info.RefreshContent();

                    }
                    break;

                // Create
                case ConsoleKey.F7:
                    {
                        // создаем данные для диалога запроса нового имени папки (файла) у пользователя
                        _dialogData = new DialogCreateTemplate("Name", true);

                        // Показываем диалоговое окно, ожидаем ввода пользователя
                        if (_defaultUIDialog.Draw(_dialogData))
                        {
                            // Если пользователь ввел имя и нажал кнопку "Создать",
                            // то создаем блок данных для для операции Создать папку или файл если пользователь не ввел пустую строку
                            if (string.IsNullOrWhiteSpace(_dialogData.InputData) == false)
                            {
                                FOData data = new FOData
                                {
                                    SourcePath = _leftActive ? _leftFolder.DirectoryPath : _rightFolder.DirectoryPath,
                                    DestinationPath = _leftActive ? Path.Combine(_leftFolder.DirectoryPath, _dialogData.InputData) : Path.Combine(_rightFolder.DirectoryPath, _dialogData.InputData),
                                    DoSilent = true,
                                    Dialog = _defaultUIDialog,
                                    ErrorLogger = _errorLogger
                                };

                                // Создаем экземпляр класса выполнения операции создания папки или файла
                                FOBase operation = new FOCreate(data);
                                // выполняем ее
                                operation.Execute();
                            }
                        }

                        // после выполнения операции создания, очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();

                        // отрисовываем рамки панелей отображения каталогов
                        _leftFolder.Draw();
                        _rightFolder.Draw();

                        // обновляем содержимой в них
                        if (_leftActive)
                        {
                            _leftFolder.UpdateContent();
                            _rightFolder.RefreshContent();
                        }
                        else
                        {
                            _leftFolder.RefreshContent();
                            _rightFolder.UpdateContent();
                        }

                        // Перерисовываем информаационную панель
                        _info.Draw();
                        _info.RefreshContent();

                    }
                    break;

                // Delete
                case ConsoleKey.F8:
                    {
                        // получаем имя и путь выбранного элемента активной панели
                        (string sourceName, string sourcePath) =
                            _leftActive ?
                            (
                            _leftFolder.FolderContent[_leftFolder.NavIndex.SelectedItem].Name,
                            _leftFolder.FolderContent[_leftFolder.NavIndex.SelectedItem].Path
                            )
                            :
                            (
                            _rightFolder.FolderContent[_rightFolder.NavIndex.SelectedItem].Name,
                            _rightFolder.FolderContent[_rightFolder.NavIndex.SelectedItem].Path
                            );

                        bool result = false;

                        // показываем диалоговое окно подтвеждения удаления и ожидаем ответа пользователя
                        if (_defaultUIDialog.Draw(new DialogDeleteTemplate(sourceName, true)))
                        {
                            // Если пользователь подтвердил операцию удаления то,
                            // создаем блок данных для операции удаления
                            FOData data = new FOData
                            {
                                SourcePath = sourcePath,
                                DestinationPath = null,
                                DoSilent = true,
                                Dialog = _defaultUIDialog,
                                ErrorLogger = _errorLogger
                            };

                            // создаем экземпляр класса для выполнения операции удаления
                            // и запускаем её
                            FOBase operation = new FODelete(data);
                            result = operation.Execute();
                        }

                        // после выполнения операции создания, очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();
                        _leftFolder.Draw();
                        _rightFolder.Draw();
                        _info.Draw();
                        _info.RefreshContent();

                        if (result == false)
                        {
                            _leftFolder.RefreshContent();
                            _rightFolder.RefreshContent();
                        }
                        else if (_leftActive)
                        {
                            _leftFolder.UpdateContent();
                            _rightFolder.RefreshContent();
                        }
                        else
                        {
                            _leftFolder.RefreshContent();
                            _rightFolder.UpdateContent();
                        }

                    }
                    break;

                // Search
                case ConsoleKey.F9:
                    {
                        // создаем данные для диалога запроса поисковой строки у пользователя
                        _dialogData = new DialogSearchTemplate("*", true);

                        bool result;

                        // Показываем диалоговое окно, ожидаем ввода пользователя
                        result = _defaultUIDialog.Draw(_dialogData);


                        // Ищем согласно поисковому запросу на диске активной панели
                        if (result)
                        {
                            // создаем данные для диалога запроса поисковой строки у пользователя
                            DialogData dialogDataNew = new DialogSearchTemplate("", false);

                            // Показываем диалоговое окно, ожидаем ввода пользователя
                            _defaultUIDialog.Draw(dialogDataNew);

                            if (_leftActive)
                            {
                                _leftFolder.FindContent(_dialogData.InputData);
                            }
                            else
                            {
                                _rightFolder.FindContent(_dialogData.InputData);
                            }
                        }


                        // Перерисовываем панели после выполнения поиска
                        // после выполнения операции создания, очищаем консоль и отрисовываем все заново,
                        // т.к. показывалось диалоговое окно
                        Console.Clear();
                        _leftFolder.Draw();
                        _leftFolder.RefreshContent();
                        _rightFolder.Draw();
                        _rightFolder.RefreshContent();
                        _info.Draw();
                        _info.RefreshContent();
                    }
                    break;

                // Navigation
                case (ConsoleKey.UpArrow):
                case (ConsoleKey.DownArrow):
                case (ConsoleKey.LeftArrow):
                case (ConsoleKey.RightArrow):
                case (ConsoleKey.Enter):
                    {
                        if (_leftActive)
                        {
                            _leftFolder.CommandExecute(key);
                        }
                        else
                        {
                            _rightFolder.CommandExecute(key);
                        }

                        if (key == ConsoleKey.Enter)
                        {
                            if (_leftActive)
                            {
                               _appSettings.Settings.leftFolderPath =  _leftFolder.DirectoryPath;
                            }
                            else
                            {
                                _appSettings.Settings.rightFolderPath = _rightFolder.DirectoryPath;
                            }
                        }

                    }
                    break;

                // Change active panel
                case (ConsoleKey.Tab):
                    {
                        _leftActive = !_leftActive;

                        _leftFolder.IsActive = _leftActive;
                        _leftFolder.RefreshContent();

                        _rightFolder.IsActive = !_leftFolder.IsActive;
                        _rightFolder.RefreshContent();
                    }
                    break;

                default:
                    {
                        _leftFolder.RefreshContent();
                        _rightFolder.RefreshContent();
                    }
                    break;
            }

            if (AppHelper.AppSizeChanged(_appSettings))
            {
                _appSettings.Settings.AppDimensions.Height = Console.WindowHeight;
                _appSettings.Settings.AppDimensions.Width = Console.WindowWidth;
                CalculateUI();
                Console.Clear();
                Draw();
            }

        }
    }
}
