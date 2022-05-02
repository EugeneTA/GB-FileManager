using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Абстрактный класс создания элемента графического интерфейса
    /// </summary>
    /// <typeparam name="T">тип создаваемого элемента</typeparam>
    public abstract class UIFactory<T>
    {
         public abstract T CreateView();
    }
}
