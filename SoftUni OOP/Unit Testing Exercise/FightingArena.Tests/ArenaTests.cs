namespace FightingArena.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class ArenaTests
    {
        Arena arena;

        [SetUp]
        public void SetUp()
        {
            arena = new Arena();
        }

        [Test]
        public void ConstructShouldWork()
        {
            Assert.IsNotNull(arena);
        }

        [Test]
        public void WarriorsShouldReturnACollectionOfTheCurrentWarrior()
        {
            IReadOnlyCollection<Warrior> warriors = new List<Warrior>();

            CollectionAssert.AreEqual(warriors, arena.Warriors);
        }

        [TestCase(3)]
        public void CountShouldReturnTheCurrentWarriorsCount(int expectedWarriors)
        {
            Random random = new Random();

            for (int i = 0; i < expectedWarriors; i++)
            {
                arena.Enroll(new Warrior(random.Next().ToString(), random.Next(), random.Next()));
            }

            Assert.AreEqual(expectedWarriors, arena.Count);
        }

        [Test]
        public void EnrollShouldAddAWarriorToTheCollection()
        {
            int expectedWarriors = 1;

            arena.Enroll(new Warrior("Pesho", 10000000, 1));

            Assert.AreEqual(expectedWarriors, arena.Count);
        }

        [Test]
        public void EnrollShouldThrowExceptionWhenWarriorWithSameNameIsAdded()
        {
            string expectedMessage = "Warrior is already enrolled for the fights!";

            arena.Enroll(new Warrior("Pesho", 10000000, 1));

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => arena.Enroll(new Warrior("Pesho", 10000000, 1)));

            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void AttackShouldMakeWarriorsAttack()
        {
            int expectedHealth = 15;

            Warrior attacker = new Warrior("Pesho", 30, 50);
            Warrior defender = new Warrior("Gocaka", 50, 45);

            arena.Enroll(attacker);
            arena.Enroll(defender);

            arena.Fight(attacker.Name, defender.Name);

            Assert.AreEqual(15, defender.HP);
        }

        [TestCase("Petranko")]
        public void FightShouldThrowExcpetionIdAttackerNameDoesNotExist(string name)
        {
            string expectedMessage = $"There is no fighter with name {name} enrolled for the fights!";

            Warrior attacker = new Warrior("Pesho", 30, 50);
            Warrior defender = new Warrior("Gocaka", 50, 45);

            arena.Enroll(attacker);
            arena.Enroll(defender);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => arena.Fight(name, defender.Name));

            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestCase("Petranko")]
        public void FightShouldThrowExcpetionIdDefenderNameDoesNotExist(string name)
        {
            string expectedMessage = $"There is no fighter with name {name} enrolled for the fights!";

            Warrior attacker = new Warrior("Pesho", 30, 50);
            Warrior defender = new Warrior("Gocaka", 50, 45);

            arena.Enroll(attacker);
            arena.Enroll(defender);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => arena.Fight(attacker.Name, name));

            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}
