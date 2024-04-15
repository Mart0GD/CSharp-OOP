using System;
using System.ComponentModel.DataAnnotations;
using ValidationAttributes.Models;
using ValidationAttributes.Validators;

namespace ValidationAttributes
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var person = new Person
             (
                 "p",
                 -1
             );
 
            bool isValidEntity = Validators.Validator.IsValid(person);
            

            Console.WriteLine(isValidEntity);
        }
    }
}
