using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FootballTeam.Tests
{
    public class FootballPlayerTests
    {

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void ConstructorShouldWork()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 16, "Forward");

            Assert.IsNotNull(player);
            Assert.AreEqual("Pesho", player.Name);
            Assert.AreEqual(16, player.PlayerNumber);
            Assert.AreEqual("Forward", player.Position);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Name_SetterException(string name)
        {
            ArgumentException ex = Assert.Catch <ArgumentException>(() => new FootballPlayer(name, 16, "Forward"));

            Assert.AreEqual("Name cannot be null or empty!", ex.Message);
        }

        [TestCase(0)]
        [TestCase(22)]
        public void PlayerNumber_SetterException(int number)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new FootballPlayer("Pesho", number, "Forward"));

            Assert.AreEqual("Player number must be in range [1,21]", ex.Message);
        }

        [TestCase("St")]
        [TestCase("GK")]
        public void position_SetterException(string position)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => new FootballPlayer("Pesho", 16, position));

            Assert.AreEqual("Invalid Position", ex.Message);
        }

        [Test]
        public void Score_IncreaseGoals()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 16, "Forward");

            player.Score();

            int expected = 1;
            int actual = player.ScoredGoals;

            Assert.AreEqual(expected,actual);
        }


    }
}