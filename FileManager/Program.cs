using System;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем логгер
            IErrorLog errorLogger = new ErrorLogToFile();

            // Считываем настройки приложения
            AppSettings appSettings = new AppSettings(errorLogger, new AppData(), new ReadFromAppSettings(), new SaveToAppSettings());
            appSettings.ReadSettings();

            // Устанавливаем размер окна
            AppHelper.SetApplicationWindowSize(appSettings, errorLogger);

            // Создаем файловый менеджер
            App fileManager = new App(appSettings, errorLogger);

            // Рисуем графический интерфейс         
            fileManager.Draw();

            // Считываем команды от пользователя и обрабатывем их
            ConsoleKey key = Console.ReadKey().Key;

            while (key != ConsoleKey.F10)
            {
                fileManager.CommandExecute(key);
                key = Console.ReadKey().Key;
            }

            // Сохраняем настройки при выходе из приложения
            appSettings.SaveSettings();

        }
    }
}
