namespace SimpleSnake
{
    using SimpleSnake.Core;
    using SimpleSnake.GameObjects;
    using SimpleSnake.GameObjects.Foods;
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using Utilities;
    using Point = GameObjects.Point;

    public class StartUp
    {
        static int[] borderCordinates = new int[2];
        static int[] arenaSize = new int[2];

        public static void Main()
        {
            ConsoleWindow.CustomizeConsole();


            bool success = false;
            while (!success)
            {

                try
                {
                    Console.WriteLine("Please insert area cordinates. Example(5,5)");

                    borderCordinates = Console.ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                    success = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong Input!");
                }
            }

            success = false;
            while (!success)
            {


                try
                {

                    Console.WriteLine("Please insert area size. Example(10,15)");
                    Console.WriteLine("Cannot be vertically larger than ");

                    arenaSize = Console.ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                    success = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong Input!");
                }
            }

            Console.Clear();

            Start(borderCordinates, arenaSize);
        }

        public static void Restart()
        {
            Start(borderCordinates, arenaSize);
        }

        public static void Start(int[] borderCordinates, int[] arenaSize)
        {
            Border bordes = new(borderCordinates[0], borderCordinates[1]);
            Food[] food = new Food[] { new HashtagFood(bordes), new DollarFood(bordes), new AsteriksFood(bordes) };


            bordes.InitializeBorders(arenaSize[0], arenaSize[1]);

            Snake snake = new Snake(food, bordes);

            Engine engine = new Engine(snake);
        }
    }
}

