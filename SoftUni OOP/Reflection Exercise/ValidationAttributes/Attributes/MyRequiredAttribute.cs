using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ValidationAttributes.Getter;
using ValidationAttributes.Models;

namespace ValidationAttributes.Attributes
{
    public class MyRequiredAttribute : MyValidationAttribute
    {
        Assembly assembly;

        public MyRequiredAttribute()
        {
            assembly = Assembly.GetEntryAssembly();
        }

        public override bool IsValid(object obj)
        {
            Type type = obj.GetType();
            object value = null;

            value = PropertyGetter.GetPropertyValue<object>(obj, type,this);

            if (String.IsNullOrWhiteSpace(value?.ToString()))
                return false;


            return true;
        }

       
    }
}
