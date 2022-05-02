using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Базовый класс размера элемента
    /// </summary>
    public class Dimensions
    {
        // Ширина
        private int _width;
        // Высота
        private int _height;

        public Dimensions() : this(0, 0)
        {

        }

        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width 
        {
            get { return _width; }
            set { _width = Math.Abs(value); } 
        }

        public int Height 
        {
            get { return _height; }
            set { _height = Math.Abs(value); } 
        }

        public static Dimensions operator +(Dimensions op1, Dimensions op2)
        {
            if (op1 == null)
            {
                return op2;
            }

            if (op2 == null)
            {
                return op1;
            }

            return new Dimensions(op1.Width + op2.Width, op1.Height + op2.Height);
        }

        public static Dimensions operator -(Dimensions op1, Dimensions op2)
        {
            if (op1 == null)
            {
                return op2;
            }

            if (op2 == null)
            {
                return op1;
            }

            return new Dimensions(op1.Width - op2.Width, op1.Height - op2.Height);
        }
    }
}
