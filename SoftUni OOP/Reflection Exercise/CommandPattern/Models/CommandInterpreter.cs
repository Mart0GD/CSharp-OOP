using CommandPattern.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern.Models
{
    internal class CommandInterpreter : ICommandInterpreter
    {
        public string Read(string args)
        {
            string[] commandTokens = args.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            Assembly assembly = Assembly.GetEntryAssembly();

            Type commandType = assembly.GetTypes().FirstOrDefault(x => x.Name.StartsWith(commandTokens[0]));

            ICommand command = Activator.CreateInstance(commandType) as ICommand;

            return command.Execute(commandTokens.Skip(1).ToArray());
        }
    }
}
