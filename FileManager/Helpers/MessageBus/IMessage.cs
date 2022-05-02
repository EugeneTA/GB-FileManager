using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Information message interface to communicate between objects
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Execute message
        /// </summary>
        void Processing();
    }
}
