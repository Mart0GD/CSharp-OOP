using CommandPattern.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommandPattern.Models
{
    public class ExitCommand : Core.Contracts.ICommand
    {
        private const int EXIT_CODE = 0;

        public string Execute(string[] args)
        {
            Environment.Exit(EXIT_CODE);

            return null;
        }
    }
}
