using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Models.Robots
{
    public class IndustrialAssistant : Robot
    {
        private const int IndustrialBatteryCapacity = 40_000;
        private const int IndustrialConversionCapacityIndex = 5_000;
        public IndustrialAssistant(string model) : base(model, IndustrialBatteryCapacity, IndustrialConversionCapacityIndex)
        {
        }
    }
}
