using SimpleSnake.Enums;
using SimpleSnake.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Point = SimpleSnake.GameObjects.Point;

namespace SimpleSnake.Core
{
    public class Engine
    {
        Snake snake;
        Point[] directionsCordinates;
        Direction direction;

        private float sleepTime;


        public Engine(Snake snake) 
        {
           

            directionsCordinates = new Point[4];

            directionsCordinates[0] = new Point(1, 0);
            directionsCordinates[1] = new Point(-1, 0);

            directionsCordinates[2] = new Point(0, 1);
            directionsCordinates[3] = new Point(0, -1);

            this.snake = snake;
            this.sleepTime = 100f;

            Run();
        }

        public void Run()
        {

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    this.GetDirection();
                }

                bool isMoving = snake.IsMoving(directionsCordinates[(int)direction]);

                if (!isMoving)
                {
                    AskForRestart();
                }

                sleepTime -= 0.01f;

                Thread.Sleep((int)sleepTime);
            }
        }

        private void AskForRestart()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();

            Console.WriteLine("Restart");
            Console.WriteLine("Y/N");


            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    StartUp.Restart();
                }
                else if (key.Key == ConsoleKey.N)
                {
                    Console.Clear();

                    Console.SetCursorPosition(Console.WindowWidth / 2 / 2, Console.WindowHeight / 2);
                    Console.WriteLine("Thank you for playing!");

                    Environment.Exit(0);
                }
                else
                {
                    Console.Clear();

                    Console.WriteLine("Restart");
                    Console.WriteLine("Y/N");
                }
            }
        }

        private void GetDirection()
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.RightArrow)
            {
                if (direction != Direction.Left)
                {
                    direction = Direction.Right;
                }
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (direction != Direction.Right)
                {
                    direction = Direction.Left;
                }
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                if (direction != Direction.Down)
                {
                    direction = Direction.Up;
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (direction != Direction.Up)
                {
                    direction = Direction.Down;
                }
            }
        }
    }
}
