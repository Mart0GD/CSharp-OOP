using NUnit.Framework;
using System;

namespace Skeleton.Tests
{
    [TestFixture]
    public class DummyTests
    {
        private const int BASE_DUMMY_HEALTH = 10;
        private const int BASE_DUMMY_XP = 0;

        Dummy dummy;

        [SetUp]
        public void SetUp()
        {
            dummy = new(BASE_DUMMY_HEALTH, BASE_DUMMY_XP);
        }

        [Test]
        public void DummyShouldLoseHealthIfAttacked()
        {
            const int EXPECTED_HEALTH = 9;


            dummy.TakeAttack(1);

            Assert.That(dummy.Health, Is.EqualTo(EXPECTED_HEALTH), "Dummy Doesn't lose health");
        }

        [Test]
        public void DummyShouldThrowExceptionIfAttackedWhenDead()
        {
            const string EXPECTED_MESSAGE = "Dummy is dead.";

            dummy.TakeAttack(10);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => dummy.TakeAttack(1));

            Assert.AreEqual(EXPECTED_MESSAGE, ex.Message, "Dummy dosn't throw exception");
        }

        [Test]
        [TestCase(10)]
        [TestCase(11)]
        public void DeadDummyShouldGiveXP(int damage)
        {
            dummy.TakeAttack(damage);

            Assert.AreEqual(BASE_DUMMY_XP, dummy.GiveExperience(), "Dummy doesn't give experience");
        }

        [Test]
        [TestCase(9)]
        [TestCase(5)]
        public void AliveDummyShouldNotGiveXP(int damage)
        {
            const string EXPECTED_MESSAGE = "Target is not dead.";

            dummy.TakeAttack(damage);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => dummy.GiveExperience());

            Assert.AreEqual(EXPECTED_MESSAGE, ex.Message, "Dummy doesn't give experience");
        }
    }

}