using NUnit.Framework;
using System.Runtime.InteropServices;

namespace VehicleGarage.Tests
{
    public class GarageTests
    {
        Garage garage;
        [SetUp]
        public void Setup()
        {
            garage = new Garage(5);
        }

        [Test]
        public void ConstructorShouldWorkProperly()
        {
            Assert.NotNull(garage);
            Assert.AreEqual(garage.Capacity, 5);
            Assert.NotNull(garage.Vehicles);
        }

        [Test]
        public void AddShouldIncreaseVehiclesCountAndReturnTrue()
        {
            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            Assert.IsTrue(garage.AddVehicle(car));
            Assert.AreEqual(garage.Vehicles.Count, 1);
        }

        [Test]
        public void AddShouldReturnFalseIfSameLicensePlateIsAdded()
        {
            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            Assert.IsTrue(garage.AddVehicle(car));
            Assert.IsFalse(garage.AddVehicle(car));
            Assert.AreEqual(garage.Vehicles.Count, 1);
        }

        [Test]
        public void AddShouldReturnFalseCapacityReached()
        {
            Garage gar = new Garage(0);

            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            Assert.IsFalse(gar.AddVehicle(car));
            Assert.AreEqual(gar.Vehicles.Count, 0);
        }

        [Test]
        public void ChargeShouldSetVehicleBatteryToFull()
        {
            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            garage.AddVehicle(car);

            car.BatteryLevel = 10;

            Assert.AreEqual(garage.ChargeVehicles(50), 1);
            Assert.AreEqual(100, car.BatteryLevel);
        }

        [Test]
        public void ChargeShouldNotSetVehiclesBatteryIfTheAreAboveTreshold()
        {
            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            garage.AddVehicle(car);

            car.BatteryLevel = 70;

            Assert.AreEqual(garage.ChargeVehicles(50), 0);
            Assert.AreEqual(70, car.BatteryLevel);
        }

        [Test]
        public void DriveVehicleShouldDrainBattery()
        {
            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            garage.AddVehicle(car);

            garage.DriveVehicle("PK2329KL", 10, false);

            Assert.AreEqual(90, car.BatteryLevel);
            Assert.IsFalse(car.IsDamaged);
        }

        [Test]
        public void DriveVehicleShouldChangeDamagedVariableIfAccidentOcurred()
        {
            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            garage.AddVehicle(car);

            garage.DriveVehicle("PK2329KL", 10, true);

            Assert.AreEqual(90, car.BatteryLevel);
            Assert.IsTrue(car.IsDamaged);
        }

        [Test]
        public void DriveVehicleShouldNotdrainBattery()
        {
            Vehicle car = new Vehicle("BMW", "325", "PK2329KL");

            garage.AddVehicle(car);


            //Test 1
            garage.DriveVehicle("PK2329KL", 101, true);

            Assert.AreEqual(100, car.BatteryLevel);

            //Test 2
            car.IsDamaged = true;
            garage.DriveVehicle("PK2329KL", 10, true);

            Assert.AreEqual(100, car.BatteryLevel);

            //Test 3
            car.IsDamaged = false;
            car.BatteryLevel = 50;

            garage.DriveVehicle("PK2329KL", 60, true);

            Assert.AreEqual(50, car.BatteryLevel);
        }

        [Test]
        public void RepairVehiclesShouldSetIsDamagedToFalse()
        {
            Vehicle car = new Vehicle("BMW", "325d", "PK2329KL");
            Vehicle car2 = new Vehicle("BMW", "320i", "CB9872PP");

            garage.AddVehicle(car);
            garage.AddVehicle(car2);

            car.IsDamaged = true;
            car2.IsDamaged = true;

            string result = garage.RepairVehicles();

            Assert.IsFalse(car.IsDamaged);
            Assert.IsFalse(car2.IsDamaged);
            Assert.AreEqual("Vehicles repaired: 2", result);
        }

        [Test]
        public void RepairVehiclesShouldNotSetIsDamagedToFalseWhenCarIsNotDamaged()
        {
            Vehicle car = new Vehicle("BMW", "325d", "PK2329KL");
            Vehicle car2 = new Vehicle("BMW", "320i", "CB9872PP");

            garage.AddVehicle(car);
            garage.AddVehicle(car2);

            string result = garage.RepairVehicles();

            Assert.IsFalse(car.IsDamaged);
            Assert.IsFalse(car2.IsDamaged);
            Assert.AreEqual("Vehicles repaired: 0", result);
        }

        [Test]
        public void VehicleConstructorShouldWork()
        {
            Vehicle car = new Vehicle("BMW", "325d", "PK2329KL");


            Assert.NotNull(car);
            Assert.AreEqual("BMW", car.Brand);
            Assert.AreEqual("325d", car.Model);
            Assert.AreEqual("PK2329KL", car.LicensePlateNumber);
            Assert.AreEqual(100, car.BatteryLevel);
            Assert.AreEqual(false, car.IsDamaged);

        }
    }
}