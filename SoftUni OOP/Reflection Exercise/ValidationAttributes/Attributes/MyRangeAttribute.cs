using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ValidationAttributes.Getter;

namespace ValidationAttributes.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class MyRangeAttribute : MyValidationAttribute
    {
        int minValue;
        int maxValue;

        public MyRangeAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public int MinValue { get => minValue; set => minValue = value; }
        public int MaxValue { get => maxValue; set => maxValue = value; }

        public override bool IsValid(object obj)
        {
            Type type = obj.GetType();

            int objectNumber = 0;

            objectNumber = PropertyGetter.GetPropertyValue<int>(obj, type, this);

            return objectNumber >= minValue && objectNumber <= maxValue;
        }
    }
}
