using CommandPattern.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern.Models
{
    public class Engine : IEngine
    {
        private readonly ICommandInterpreter commandInterpreter;

        public Engine(ICommandInterpreter interpreter)
        {
            commandInterpreter = interpreter;
        }
        public void Run()
        {
            string command = Console.ReadLine();
            while (true)
            {
                Console.WriteLine(commandInterpreter.Read(command));

                command = Console.ReadLine();
            }
        }
    }
}
