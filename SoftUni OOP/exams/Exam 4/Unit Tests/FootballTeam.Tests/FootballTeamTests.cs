using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FootballTeam.Tests
{
    public class FootballTeamTests
    {
        FootballTeam team;

        [SetUp]
        public void Setup()
        {
            team = new FootballTeam("FK Levski", 16);
        }

        [Test]
        public void ConstructorShouldWord()
        {
            FootballTeam team = new FootballTeam("FK Levski", 16);

            Assert.IsNotNull(team);

            Assert.AreEqual("FK Levski", team.Name);
            Assert.AreEqual(16, team.Capacity);
            Assert.IsNotNull(team.Players);

        }

        [TestCase("")]
        [TestCase(null)]
        public void Name_SetterException(string name)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => team.Name = name);

            Assert.AreEqual("Name cannot be null or empty!", ex.Message);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Capacity_SetterException(string name)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => team.Capacity = 11);

            Assert.AreEqual("Capacity min value = 15", ex.Message);
        }

        [Test]
        public void AddNewPlayer_Succeed()
        {
            string expected = "Added player Pesho in position Forward with number 11";
            string actual = team.AddNewPlayer(new FootballPlayer("Pesho", 11, "Forward"));

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, team.Players.Count);
        }

        [Test]
        public void AddNewPlayer_Fail()
        {
            team.Capacity = 15;

            Random random = new Random();
            for (int i = 0; i < 15; i++)
            {
                team.AddNewPlayer(new FootballPlayer(random.Next().ToString(), random.Next(11, 15), "Forward"));
            }

            string expected = "No more positions available!";
            string actual = team.AddNewPlayer(new FootballPlayer("Pesho", 11, "Forward"));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PickPlayer_Succeed()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 11, "Forward");

            team.AddNewPlayer(player);

            Assert.AreEqual(player, team.PickPlayer("Pesho"));
        }

        [Test]
        public void PickPlayer_SucceedNull()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 11, "Forward");

            team.AddNewPlayer(player);

            Assert.AreEqual(null, team.PickPlayer("Gosho"));
        }

        [Test]
        public void PlayerScore_Succeed()
        {
            FootballPlayer player = new FootballPlayer("Pesho", 11, "Forward");

            team.AddNewPlayer(player);

            string expected = "Pesho scored and now has 1 for this season!";
            string actual = team.PlayerScore(11);

            Assert.AreEqual(expected, actual);
        }

       
    }
}