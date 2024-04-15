using EDriveRent.Core.Contracts;
using EDriveRent.Models;
using EDriveRent.Models.Contracts;
using EDriveRent.Repositories;
using EDriveRent.Repositories.Contracts;
using EDriveRent.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EDriveRent.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IUser> users;
        private readonly IRepository<IVehicle> vehicles;
        private readonly IRepository<IRoute> routes;

        public Controller()
        {
            users = new UserRepository();
            vehicles = new VehicleRepository();
            routes = new RouteRepository();
        }

        public string AllowRoute(string startPoint, string endPoint, double length)
        {
            int id = routes.GetAll().Count + 1;

            if (routes.GetAll().Any(x => x.StartPoint == startPoint && x.EndPoint == endPoint && x.Length == length))
            {
                return String.Format(OutputMessages.RouteExisting, startPoint, endPoint, length);
            }
            if (routes.GetAll().Any(x => x.StartPoint == startPoint && x.EndPoint == endPoint && x.Length < length))
            {
                return String.Format(OutputMessages.RouteIsTooLong, startPoint, endPoint);
            }

            IRoute longerRoute = routes.GetAll().FirstOrDefault(x => x.StartPoint == startPoint
                                                                  && x.EndPoint == endPoint
                                                                  && x.Length > length);
            if (longerRoute is not null)
            {
                longerRoute.LockRoute();
            }

            IRoute routeToAdd = new Route(startPoint, endPoint, length, id);

            routes.AddModel(routeToAdd);

            return String.Format(OutputMessages.NewRouteAdded, startPoint, endPoint, length);
        }

        public string MakeTrip(string drivingLicenseNumber, string licensePlateNumber, string routeId, bool isAccidentHappened)
        {
            IUser user = users.FindById(drivingLicenseNumber);
            IVehicle vehicle = vehicles.FindById(licensePlateNumber);
            IRoute route = routes.FindById(routeId);

            if (user.IsBlocked)
            {
                return String.Format(OutputMessages.UserBlocked, drivingLicenseNumber);
            }
            if (vehicle.IsDamaged)
            {
                return String.Format(OutputMessages.VehicleDamaged, licensePlateNumber);
            }
            if (route.IsLocked)
            {
                return String.Format(OutputMessages.RouteLocked, routeId);
            }

            vehicle.Drive(route.Length);

            if (isAccidentHappened)
            {
                vehicle.ChangeStatus();
                user.DecreaseRating();
            }
            else
            {
                user.IncreaseRating();
            }

            return vehicle.ToString();
        }

        public string RegisterUser(string firstName, string lastName, string drivingLicenseNumber)
        {
            IUser userToAdd = new User(firstName, lastName, drivingLicenseNumber);

            if (users.FindById(userToAdd.DrivingLicenseNumber) is not null)
            {
                return String.Format(OutputMessages.UserWithSameLicenseAlreadyAdded, drivingLicenseNumber);
            }

            users.AddModel(userToAdd);

            return String.Format(OutputMessages.UserSuccessfullyAdded, firstName, lastName, drivingLicenseNumber);
        }

        public string RepairVehicles(int count)
        {
            int repairedCount = 0;
            foreach (var vehicle in vehicles.GetAll().Where(x => x.IsDamaged).OrderBy(x => x.Brand).ThenBy(x => x.Model).Take(count))
            {
                vehicle.ChangeStatus();
                vehicle.Recharge();

                repairedCount++;
            }

            return $"{repairedCount} vehicles are successfully repaired!";

        }

        public string UploadVehicle(string vehicleType, string brand, string model, string licensePlateNumber)
        {
            if (vehicleType != typeof(PassengerCar).Name && vehicleType != typeof(CargoVan).Name)
            {
                return String.Format(OutputMessages.VehicleTypeNotAccessible, vehicleType);
            }
            else if (vehicles.FindById(licensePlateNumber) is not null)
            {
                return String.Format(OutputMessages.LicensePlateExists, licensePlateNumber);
            }

            IVehicle vehiclToAdd = vehicleType == typeof(PassengerCar).Name
                                    ? new PassengerCar(brand, model, licensePlateNumber)
                                    : new CargoVan(brand, model, licensePlateNumber);

            vehicles.AddModel(vehiclToAdd);

            return String.Format(OutputMessages.VehicleAddedSuccessfully, brand, model, licensePlateNumber);
        }

        public string UsersReport()
        {
            StringBuilder sb = new();

            sb.AppendLine("*** E-Drive-Rent ***");

            foreach (var user in users.GetAll().OrderByDescending(x => x.Rating).ThenBy(x => x.LastName).ThenBy(x => x.FirstName))
            {
                sb.AppendLine(user.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
