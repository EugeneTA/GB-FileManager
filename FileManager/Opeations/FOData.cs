
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Набор данных, необходимый для выполнения файловой операции
    /// </summary>
    public class FOData
    {
        // Путь источника
        public string SourcePath { get; set; } = "";
        
        // Путь к папке/файлу назначения
        public string DestinationPath { get; set; } = "";

        // Флаг запроса подтверждения операции от пользователя
        public bool DoSilent { get; set; } = false;
        
        // Диалоговое окно для вывода информации
        public UIDialogView Dialog { get; set; }

        // Логгер ошибок
        public IErrorLog ErrorLogger { get; set; }

    }
}
