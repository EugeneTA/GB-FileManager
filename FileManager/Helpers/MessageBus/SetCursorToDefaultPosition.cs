using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Set cursor to default position message.
    /// Move console cursor to defined default position
    /// </summary>
    public class SetCursorToDefaultPosition : IMessage
    {
        public void Processing()
        {
            Console.SetCursorPosition(2, Console.WindowHeight-2);
            Console.CursorVisible = true;
        }
    }
}
