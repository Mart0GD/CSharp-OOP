using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSnake.GameObjects
{
    public class Border : Point
    {
        public Border(int startX, int startY) : base(startX, startY)
        {
        }

        public int HorizontalSize { get; private set; }
        public int VerticalSize { get; private set; }

        public bool IsBorder(Point snakeHead)
            => snakeHead.x == x || snakeHead.y == y
               || snakeHead.x == x + HorizontalSize -1 || snakeHead.y == y + VerticalSize - 1;

        public void InitializeBorders(int horizontalSize, int verticalSize)
        {
            HorizontalSize = horizontalSize;
            VerticalSize = verticalSize;

            SetHorizontal(horizontalSize, x, y);

            SetVertical(verticalSize, x, y);
            SetVertical(verticalSize, x + horizontalSize - 1, y);

            SetHorizontal(horizontalSize, x, y + verticalSize - 1);
        }

        private void SetHorizontal(int horizontalSize, int x, int y)
        {
            Console.SetCursorPosition(x, y);

            for (int i = 0; i < horizontalSize; i++)
            {
                Console.Write("-");
            }
        }

        private void SetVertical(int verticalSize, int x, int y)
        {
            for (int i = 1; i < verticalSize; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write('-');
            }
        }

    }
}
