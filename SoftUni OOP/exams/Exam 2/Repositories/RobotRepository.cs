using RobotService.Models.Contracts;
using RobotService.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Repositories
{
    public class RobotRepository : IRepository<IRobot>
    {
        private readonly List<IRobot> robots;

        public RobotRepository()
        {
            robots = new List<IRobot>();
        }

        public void AddNew(IRobot model)
        {
            robots.Add(model);
        }

        public IReadOnlyCollection<IRobot> Models() => robots.AsReadOnly();

        public bool RemoveByName(string robotModel)
        {
            IRobot supplement = robots.FirstOrDefault(x => x.Model == robotModel);

            return robots.Remove(supplement);
        }

        IRobot IRepository<IRobot>.FindByStandard(int interfaceStandard) => robots.FirstOrDefault(x => x.InterfaceStandards.Any(x => x == interfaceStandard));
    }
}
