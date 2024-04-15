using NUnit.Framework;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace RobotFactory.Tests
{
    public class FactoryTests
    {
        Factory factory;

        [SetUp]
        public void Setup()
        {
            factory = new Factory("PPP", 1);
        }

        [Test]
        public void ProduceRobot_Succeed()
        {
            int expectedCountBeforeAdd = 0;
            int resultBeforeAdd = factory.Robots.Count;

            string message = factory.ProduceRobot("Pesho", 10, 1000);

            int expectedCountAfterAdd = 1;
            int resultAfterAdd = factory.Robots.Count;

            Assert.AreEqual(expectedCountBeforeAdd, resultBeforeAdd);
            Assert.AreEqual(expectedCountAfterAdd, resultAfterAdd);

            string expectedMessage = "Produced --> Robot model: Pesho IS: 1000, Price: 10.00";

            Assert.AreEqual(expectedMessage, message);
        }

        [Test]
        public void ProduceRobot_Fail()
        {
            int expectedCountBeforeAdd = 0;
            int resultBeforeAdd = factory.Robots.Count;

            factory.ProduceRobot("Pesho", 10, 1000);

            string message = factory.ProduceRobot("Pesho", 10, 1000);
            string expectedMessage = "The factory is unable to produce more robots for this production day!";

            Assert.AreEqual(expectedMessage, message);
        }

        [Test]
        public void ProduceSupplement_Succeed()
        {
            int expectedCountBeforeAdd = 0;
            int resultBeforeAdd = factory.Supplements.Count;

            string message = factory.ProduceSupplement("arm", 1000);

            int expectedCountAfterAdd = 1;
            int resultAfterAdd = factory.Supplements.Count;

            Assert.AreEqual(expectedCountBeforeAdd, resultBeforeAdd);
            Assert.AreEqual(expectedCountAfterAdd, resultAfterAdd);

            string expectedMessage = "Supplement: arm IS: 1000";

            Assert.AreEqual(expectedMessage, message);
        }

        [Test]
        public void UpgradeRobot_Succeed()
        {
            factory.ProduceRobot("MK-1", 10, 1000);
            factory.ProduceSupplement("arm", 1000);

            bool expected = true;
            bool actual = factory.UpgradeRobot(factory.Robots.First(), factory.Supplements.First());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpgradeRobot_Fail_RobotcontainsSupplement()
        {
            factory.ProduceRobot("MK-1", 10, 1000);
            factory.ProduceSupplement("arm", 1000);

            factory.UpgradeRobot(factory.Robots.First(), factory.Supplements.First());

            bool expected = false;
            bool actual = factory.UpgradeRobot(factory.Robots.First(), factory.Supplements.First());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpgradeRobot_Fail_DifferentInterfaceStandart()
        {
            factory.ProduceRobot("MK-1", 10, 1001);
            factory.ProduceSupplement("arm", 1000);

            
            bool expected = false;
            bool actual = factory.UpgradeRobot(factory.Robots.First(), factory.Supplements.First());

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SellRobot_Succeed()
        {
            factory.ProduceRobot("MK-1", 15, 1000);
            factory.ProduceRobot("MK-2", 12, 1002);
            factory.ProduceRobot("MK-3", 17, 1003);

            Robot result = factory.SellRobot(20);

            Assert.AreSame(factory.Robots.OrderByDescending(x => x.Price).First(), result);
        }

        [Test]
        public void SellRobot_Fail()
        {
            factory.ProduceRobot("MK-1", 15, 1000);
            factory.ProduceRobot("MK-2", 12, 1002);
            factory.ProduceRobot("MK-3", 17, 1003);

            Robot result = factory.SellRobot(9);

            Assert.IsNull(result);
        }

    }
}