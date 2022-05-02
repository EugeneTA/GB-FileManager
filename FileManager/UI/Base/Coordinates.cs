using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Базовый класс координат в консоли
    /// </summary>
    public class Coordinates
    {
        // Смещение по вертикали
        public int Top { get; set; }
        // Смещение по горищонтали
        public int Left { get; set; }

        public Coordinates() : this (0,0)
        {
        }

        public Coordinates(int left, int top)
        {
            Top = top;
            Left = left;
        }

        public static Coordinates operator +(Coordinates op1, Coordinates op2)
        {
            if (op1 == null)
            {
                return op2;
            }

            if (op2 == null)
            {
                return op1;
            }

            return new Coordinates(op1.Left + op2.Left, op1.Top + op2.Top);
        }

        public static Coordinates operator -(Coordinates op1, Coordinates op2)
        {
            if (op1 == null)
            {
                return op2;
            }

            if (op2 == null)
            {
                return op1;
            }

            return new Coordinates(op1.Left - op2.Left, op1.Top - op2.Top);
        }

    }
}
