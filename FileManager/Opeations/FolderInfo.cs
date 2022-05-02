using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Информация о папке. Кол-во директорий, папок и занимаемое место
    /// </summary>
    public class FolderInfo
    {
        public long Dirs { get; set; }
        public long Files { get; set; }
        public long Space { get; set; }

        static public FolderInfo operator +(FolderInfo operand1, FolderInfo operand2)
        {
            if (operand1 == null)
            {
                return operand2;
            }

            if (operand2 == null)
            {
                return operand1;
            }

            return new FolderInfo()
            {
                Dirs = operand1.Dirs + operand2.Dirs,
                Files = operand1.Files + operand2.Files,
                Space = operand1.Space + operand2.Space
            };
        }

        static public FolderInfo operator -(FolderInfo operand1, FolderInfo operand2)
        {
            if (operand1 == null)
            {
                return operand2;
            }

            if (operand2 == null)
            {
                return operand1;
            }

            return new FolderInfo()
            {
                Dirs = operand1.Dirs - operand2.Dirs,
                Files = operand1.Files - operand2.Files,
                Space = operand1.Space - operand2.Space
            };
        }
    }
}
