namespace CarManager.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CarManagerTests
    {
        Car car;

        [SetUp]
        public void Start()
        {
            car = new Car("VW","Golf",4.5,65);
        }

        [Test] 
        public void ConstructorShouldWork() 
        { 
            Assert.IsNotNull(car);

            Assert.AreEqual(car.Make, "VW");
            Assert.AreEqual(car.Model, "Golf");
            Assert.AreEqual(car.FuelConsumption, 4.5);
            Assert.AreEqual(car.FuelCapacity, 65);
            
        }

        [Test]
        public void PrivateConstructorShouldWork()
        { 
            Assert.AreEqual(car.FuelAmount, 0);
        }


        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void MakeShouldThrowExceptionIfValueIsNull(string make)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new Car(make, "Golf", 4.5, 65));

            Assert.AreEqual("Make cannot be null or empty!", ex.Message);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ModelShouldThrowExceptionIfValueIsNull(string model)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new Car("VW", model, 4.5, 65));

            Assert.AreEqual("Model cannot be null or empty!", ex.Message);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void FuelConsumptionShouldThrowExceptionIfValueIsZeroOrNegative(double amount)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new Car("VW", "Golf", amount, 65));

            Assert.AreEqual("Fuel consumption cannot be zero or negative!", ex.Message);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void FuelCapacityShouldThrowExceptionIfValueIsZeroOrNegative(int amount)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new Car("VW", "Golf", 4.5, amount));

            Assert.AreEqual("Fuel capacity cannot be zero or negative!", ex.Message);
        }

        

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void RefuelShouldThrowExceptionIfFuelIsZeroOrNegative(double fuel)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => car.Refuel(fuel));

            Assert.AreEqual("Fuel amount cannot be zero or negative!", ex.Message);
        }

        [Test]
        public void RefuelShouldIncreaseFuelAmount()
        {
            car.Refuel(10);

            Assert.AreEqual(10, car.FuelAmount);

            car.Refuel(1000);

            Assert.AreEqual(65, car.FuelAmount);
        }

        [Test]
        [TestCase(100)]
        [TestCase(10000)]
        public void DriveShouldThrowExceptionIfDistanceIsTooMuch(double distance)
        {
            InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => car.Drive(distance));

            Assert.AreEqual("You don't have enough fuel to drive!", ex.Message);
        }

        [Test]
        [TestCase(5)]
        public void DriveShouldLowerFuelAmount(double distance)
        {
            car.Refuel(10);

            car.Drive(distance);

            Assert.AreEqual(9.775, car.FuelAmount);
        }

    }
}