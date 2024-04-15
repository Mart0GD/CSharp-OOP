using RobotService.Models.Contracts;
using RobotService.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Models.Robots
{
    public abstract class Robot : IRobot
    {
        private string model;
        private int batteryCapacity;
        private int batteryLevel;
        private int convertionCapacityIndex;
        private List<int> interfaceStandards;

        protected Robot(string model, int batteryCapacity, int convertionCapacityIndex)
        {
            Model = model;
            BatteryCapacity = batteryCapacity;
            ConvertionCapacityIndex = convertionCapacityIndex;

            BatteryLevel = batteryCapacity;
            interfaceStandards = new List<int>();
        }

        public string Model
        {
            get => model;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.ModelNullOrWhitespace);
                }

                model = value;
            }
        }

        public int BatteryCapacity
        {
            get => batteryCapacity;

            private set 
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.BatteryCapacityBelowZero);
                }

                batteryCapacity = value;
            }
        }

        public int BatteryLevel
        {
            get => batteryLevel;

            private set => batteryLevel = value;
        }

        public int ConvertionCapacityIndex
        {
            get => convertionCapacityIndex;

            private set => convertionCapacityIndex = value;
        }

        public IReadOnlyCollection<int> InterfaceStandards
        {
            get => interfaceStandards.AsReadOnly();
        }

        public void Eating(int minutes)
        {
            for (int i = 0; i < minutes; i++)
            {
                if (batteryLevel == batteryCapacity) break;


                BatteryLevel += ConvertionCapacityIndex;
            }
        }
        public void InstallSupplement(ISupplement supplement)
        {
            Add(supplement.InterfaceStandard);

            BatteryCapacity -= supplement.BatteryUsage;
            BatteryLevel -= supplement.BatteryUsage;
        }

        public bool ExecuteService(int consumedEnergy)
        {
            if (batteryLevel >= consumedEnergy)
            {
                batteryLevel -= consumedEnergy;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine($"{this.GetType().Name} {Model}:");
            sb.AppendLine($"--Maximum battery capacity: {BatteryCapacity}");
            sb.AppendLine($"--Current battery level: {BatteryLevel}");
            sb.AppendLine($"--Supplements installed: {(interfaceStandards.Any() ? string.Join(" ", interfaceStandards) : "none")}");

            return sb.ToString();
        }



        private void Add(int supplement)
        {
            interfaceStandards.Add(supplement);
        }
    }
}
