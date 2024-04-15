using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDriveRent.Repositories
{
    public class VehicleRepository : IRepository<IVehicle>
    {
        List<IVehicle> _vehicles;

        public VehicleRepository()
        {
            _vehicles = new List<IVehicle>();
        }

        public void AddModel(IVehicle model) => _vehicles.Add(model);

        public IVehicle FindById(string identifier) => _vehicles.FirstOrDefault(x => x.LicensePlateNumber == identifier);

        public IReadOnlyCollection<IVehicle> GetAll() => _vehicles.AsReadOnly();

        public bool RemoveById(string identifier)
        {
            IVehicle vehicle = _vehicles.FirstOrDefault(x => x.LicensePlateNumber == identifier);

            return _vehicles.Remove(vehicle);
        }
    }
}
