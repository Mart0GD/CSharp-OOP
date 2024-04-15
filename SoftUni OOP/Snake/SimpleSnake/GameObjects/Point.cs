using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSnake.GameObjects
{
    public class Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x { get; set; }
        public int y { get; set; }

        public void Draw(char symbol)
        {
            Console.SetCursorPosition(x, y);

            Console.Write(symbol);
        }

        public void ReDraw(int x, int y, char symbol)
        {
            Console.SetCursorPosition(x, y);

            Draw(symbol);
        }
    }
}
