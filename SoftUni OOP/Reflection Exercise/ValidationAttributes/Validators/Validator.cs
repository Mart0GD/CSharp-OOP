using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ValidationAttributes.Attributes;

namespace ValidationAttributes.Validators
{
    public static class Validator
    {
        public static bool IsValid(object obj)
        {
            Type type = obj.GetType();

            var members = type.GetMembers();

            List<MyValidationAttribute> attributes = new();
            foreach (var member in members)
            {
                var customAttributes = member.GetCustomAttributes(false).Where(x => (x as MyValidationAttribute) is not null).ToArray();
                if (customAttributes.Any())
                {
                    attributes.AddRange(customAttributes.Cast<MyValidationAttribute>().ToArray());
                }
            }
            

            foreach (MyValidationAttribute item in attributes)
            {
                if (!item.IsValid(obj))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
