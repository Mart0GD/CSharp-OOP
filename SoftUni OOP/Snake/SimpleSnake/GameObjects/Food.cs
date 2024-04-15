using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSnake.GameObjects
{
    public abstract class Food : Point
    {
        private readonly char foodSymbol;
        private readonly Random random;
        private readonly Border borders;

        public Food(char symbol, int points, Border borders) : base(0,0)
        {
            random = new Random();
            this.borders = borders;
            foodSymbol = symbol;

            SetPoints(points);
        }


        public int FoodPoints { get; private set; }

        public bool IsFoodPoint(Point snakeHead) => x == snakeHead.x && y == snakeHead.y;

        public void Spawn(Queue<Point> snake)
        {
            x = random.Next(borders.x + 2, borders.HorizontalSize - 2);
            y = random.Next(borders.y + 2, borders.VerticalSize - 2);

            bool hittingSnake = snake.Any(snake => snake.x == this.x && snake.y == this.y);

            while (hittingSnake)
            {
                x = random.Next(borders.x + 2, borders.HorizontalSize - 2);
                y = random.Next(borders.y + 2, borders.VerticalSize - 2);

                hittingSnake = snake.Any(snake => snake.x == this.x && snake.y == this.y);
            }

            base.Draw(foodSymbol);
        }

        public void SetPoints(int points) => this.FoodPoints = points;

        
    }
}
