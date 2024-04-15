using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;

namespace Skeleton.Tests
{
    [TestFixture]
    public class AxeTests
    {
        private const int BASE_AXE_ATTCK = 1;
        private const int BASE_AXE_DURABILITY = 1;

        private const int BASE_DUMMY_HEALTH = 10;
        private const int BASE_DUMMY_XP = 0;

        private Axe axe;
        private Dummy dummy;

        [SetUp]
        public void Start()
        {
            axe = new Axe(BASE_AXE_ATTCK, BASE_AXE_DURABILITY);
            dummy = new Dummy(BASE_DUMMY_HEALTH, BASE_DUMMY_XP);
        }

        [Test]
        public void WeponShouldLoseDurabilityAfterEachAttack()
        {
            const int EXPECTED_DURABILITY = 0;


            axe.Attack(dummy);

            Assert.AreEqual(EXPECTED_DURABILITY, axe.DurabilityPoints, "Wepon doesn't lose durability");
        }

        [Test]
        public void WeponShouldNotAttckIfBroken()
        {
            const string EXPECTED_MESSAGE = "Axe is broken.";

            axe.Attack(dummy);

            InvalidOperationException ex =  Assert.Throws<InvalidOperationException>(() => axe.Attack(dummy));

            Assert.AreEqual(EXPECTED_MESSAGE, ex.Message, "Wepon attacks when broken");
        }
    }
}