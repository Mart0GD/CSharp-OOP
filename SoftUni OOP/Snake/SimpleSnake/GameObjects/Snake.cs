using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimpleSnake.GameObjects
{
    public class Snake
    {
        private const char snakeSymbol = '\u25CF';
        private const char emptySpace = ' ';

        Queue<Point> body;
        Food[] foods;
        Border borders;

        private int nextX;
        private int nextY;

        private int foodIndex;
        
        public Snake(Food[] foods, Border borders)
        {
            this.foods = foods;
            this.borders = borders;
            this.foodIndex = GetRandomIndex;

            body = new Queue<Point>();

            SpawnSnake();
            foods[foodIndex].Spawn(body);
        }

        public int GetRandomIndex => new Random().Next(0, foods.Length);

        public bool IsMoving(Point direction)
        {
            Point snakeHead = body.Last();

            GetNextDirection(snakeHead, direction);

            Point nextPoint = new Point(nextX, nextY);

            bool isBitingSelf = body.Any(body => body.x == nextPoint.x && body.y == nextPoint.y);

            if (isBitingSelf)
            {
                return false;
            }

            bool isPartOfBorder = this.borders.IsBorder(nextPoint);

            if (isPartOfBorder)
            {
                return false;
            }

            bool isFood = this.foods[foodIndex].IsFoodPoint(nextPoint);

            if (isFood)
            {
                this.Eat(direction, snakeHead);
            }

            Point tail = body.Dequeue();
            tail.Draw(emptySpace);

            body.Enqueue(nextPoint);
            nextPoint.Draw(snakeSymbol);

            return true;
        }

        private void Eat(Point direction, Point snakeHead)
        {
            int points = foods[foodIndex].FoodPoints;

            for (int i = 0; i < points; i++)
            {
                body.Enqueue(new Point(nextX, nextY));
                GetNextDirection(direction, snakeHead);
            }

            foodIndex = GetRandomIndex;
            foods[foodIndex].Spawn(body);
        }

        public void GetNextDirection(Point direction, Point snakeHead)
        {
            nextX = snakeHead.x + direction.x;
            nextY = snakeHead.y + direction.y;
        }
        private void SpawnSnake()
        {
            for (int x = borders.x + 2; x < borders.x + 5; x++)
            {
                body.Enqueue(new Point(x, borders.y + 2));
            }

            foreach (var point in body)
            {
                point.Draw(snakeSymbol);
            }
        }
    }
}
