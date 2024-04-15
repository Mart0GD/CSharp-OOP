using RobotService.Core.Contracts;
using RobotService.Models;
using RobotService.Models.Contracts;
using RobotService.Models.Robots;
using RobotService.Models.Supplements;
using RobotService.Repositories;
using RobotService.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Core
{
    public class Controller : IController
    {
        private readonly SupplementRepository supplementRepository;
        private readonly RobotRepository robotRepository;

        public Controller()
        {
            supplementRepository = new SupplementRepository();
            robotRepository = new RobotRepository();
        }


        public string CreateRobot(string model, string typeName)
        {
            if (typeName != typeof(DomesticAssistant).Name && typeName != typeof(IndustrialAssistant).Name)
            {
                return String.Format(OutputMessages.RobotCannotBeCreated, typeName);
            }

            IRobot robotToAdd = typeof(DomesticAssistant).Name == typeName
                                ? new DomesticAssistant(model)
                                : new IndustrialAssistant(model);

            robotRepository.AddNew(robotToAdd);

            return String.Format(OutputMessages.RobotCreatedSuccessfully, typeName, model);
        }

        public string CreateSupplement(string typeName)
        {
            if (typeName != typeof(LaserRadar).Name && typeName != typeof(SpecializedArm).Name)
            {
                return String.Format(OutputMessages.SupplementCannotBeCreated, typeName);
            }

            ISupplement supplementToAdd = typeof(LaserRadar).Name == typeName
                                ? new LaserRadar()
                                : new SpecializedArm();

            supplementRepository.AddNew(supplementToAdd);

            return String.Format(OutputMessages.SupplementCreatedSuccessfully, typeName);
        }

        public string PerformService(string serviceName, int intefaceStandard, int totalPowerNeeded)
        {
            IRobot[] robots = robotRepository.Models()
                                        .Where(x => x.InterfaceStandards.Contains(intefaceStandard))
                                        .OrderByDescending(x => x.BatteryLevel).ToArray();

            if (robots is null || robots.Length == 0)
            {
                return String.Format(OutputMessages.UnableToPerform, intefaceStandard);
            }

            int robotsBatterySum = robots.Sum(x => x.BatteryLevel);

            if (robotsBatterySum < totalPowerNeeded)
            {
                return String.Format(OutputMessages.MorePowerNeeded, serviceName, totalPowerNeeded - robotsBatterySum);
            }

            int robotsCount = 0;
            foreach (var robot in robots)
            {
                robotsCount++;
                if (robot.BatteryLevel > totalPowerNeeded)
                {
                    robot.ExecuteService(totalPowerNeeded);
                    break;
                }

                totalPowerNeeded -= robot.BatteryLevel;

                robot.ExecuteService(robot.BatteryLevel);
            }

            return String.Format(OutputMessages.PerformedSuccessfully, serviceName, robotsCount);
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var robot in robotRepository.Models().OrderByDescending(x => x.BatteryLevel).ThenBy(x => x.BatteryCapacity))
            {
                sb.AppendLine(robot.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public string RobotRecovery(string model, int minutes)
        {
            int robotsFed = 0;
            foreach (var robot in robotRepository.Models().Where(x => x.Model == model && x.BatteryLevel < x.BatteryCapacity / 2))
            {
                robot.Eating(minutes);
                robotsFed++;
            }

            return String.Format(OutputMessages.RobotsFed, robotsFed);
        }

        public string UpgradeRobot(string model, string supplementTypeName)
        {
            int interfaceValue = supplementRepository.Models().FirstOrDefault(x => x.GetType().Name == supplementTypeName).InterfaceStandard;

            var robotToUpgarde = robotRepository.Models().Where(x => !x.InterfaceStandards.Contains(interfaceValue) && x.Model == model).FirstOrDefault();

            if (robotToUpgarde is null)
            {
                return String.Format(OutputMessages.AllModelsUpgraded, model);
            }

            robotToUpgarde.InstallSupplement(supplementTypeName == typeof(SpecializedArm).Name ? new SpecializedArm() : new LaserRadar());

            return String.Format(OutputMessages.UpgradeSuccessful, model, supplementTypeName);
        }


    }
}
