using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationAttributes.Attributes;

namespace ValidationAttributes.Getter
{
    public static class PropertyGetter
    {
        public static T GetPropertyValue<T>(object obj, Type type, MyValidationAttribute att)
        {
            T value = default(T);

            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                if (prop.CustomAttributes.Any(x => x.AttributeType == att.GetType()))
                {
                    value = (T)prop?.GetValue(obj);
                    break;
                }
            }

            return value;
        }
    }
}
