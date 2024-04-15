using RobotService.Models.Robots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Models
{
    public class DomesticAssistant : Robot
    {
        private const int DomesticBatteryCapacity = 20_000;
        private const int DomesticConvertionCapacityIndex = 2_000;
        public DomesticAssistant(string model) : base(model, DomesticBatteryCapacity, DomesticConvertionCapacityIndex)
        {
        }
    }
}
