using System;

namespace FileManager
{
    /// <summary>
    /// Helper class to control application windows size
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// Считывает сохраненные данные из файла настроек приложения о размере окна (WindowHeight и WindowWidth) и
        /// передает их в SetConsoleWindowSize
        /// Если пользовательские данные отсутствуют, то берутся данны по-умолчанию (DefaultWindowHeight и DefaultWindowWidht)
        /// </summary>
        public static void SetApplicationWindowSize(AppSettings appSettings, IErrorLog errorLoger)
        {
            if (appSettings != null)
            {
                if (appSettings.Settings.AppDimensions.Width < 150 || appSettings.Settings.AppDimensions.Height < 80)
                {
                    SetConsoleWindowSize(
                        Properties.Settings.Default.DefaultWindowWidth, 
                        Properties.Settings.Default.DefaultWindowHeight,
                        errorLoger);
                }
                else
                {
                    SetConsoleWindowSize(
                        appSettings.Settings.AppDimensions.Width,
                        appSettings.Settings.AppDimensions.Height,
                        errorLoger);
                }
            }
        }

        /// <summary>
        /// Установка размера консольного окна по заданным размерам
        /// </summary>
        /// <param name="width">ширина в символах</param>
        /// <param name="height">высота в символах</param>
        private static void SetConsoleWindowSize(int width, int height, IErrorLog errorLoger)
        {
            try
            {
                Console.SetWindowSize(width, height);
            }
            catch (Exception e)
            {
                if (errorLoger != null)
                {
                    errorLoger.Log($"Ошибка при задании размера окна. Значения HxW: {height} x {width}. {e.Message}\n");
                }
            }

        }


        public static bool AppSizeChanged(AppSettings _appSettings)
        {
            bool result = true;

            if (Console.WindowWidth == _appSettings.Settings.AppDimensions.Width && Console.WindowHeight == _appSettings.Settings.AppDimensions.Height)
            {
                result = false;
            }

            return result;
        }


    }
}
