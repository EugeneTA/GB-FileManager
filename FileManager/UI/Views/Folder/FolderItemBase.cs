using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Базовый тип элемента файловой структуры 
    /// </summary>
    public class FolderItemBase
    {
        public FolderItemType Type { get; set; }
    }
}
