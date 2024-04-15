namespace FightingArena.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class WarriorTests
    {
        public const int MIN_ATTACK_HP = 30;

        Warrior warrior;
        Warrior weakerWarrior;

        [SetUp]
        public void SetUp()
        {
            warrior = new Warrior("Peshko", 50, 60);
            weakerWarrior = new Warrior("Gosho", 10, MIN_ATTACK_HP);
        }

        [Test]
        public void WarriorConstructorShouldWork()
        {
            Assert.IsNotNull(warrior);
        }

        [Test]
        public void NameSetterShouldSetName()
        {
            Assert.AreEqual("Peshko", warrior.Name);
        }

        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void NameSetterShouldThrowExceptionIfNameIsNullOrWhiteSpace(string name)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new Warrior(name, 60, 25));

            Assert.AreEqual("Name should not be empty or whitespace!", ex.Message);
        }

        [Test]
        public void DamageSetterShouldSetDamage()
        {
            int expectedDamage = 50;

            Assert.AreEqual(expectedDamage, warrior.Damage);
        }

        [TestCase(-2902)]
        [TestCase(0)]
        [TestCase(-1)]
        public void DamageSetterShouldThrowExceptionIfDamageIsZeroOrNegative(int damage)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new Warrior("Pesho",damage, 25));

            Assert.AreEqual("Damage value should be positive!", ex.Message);
        }

        [Test]
        public void HPSetterShouldSetHP()
        {
            Assert.AreEqual(60, warrior.HP);
        }

        [TestCase(-29002)]
        [TestCase(-17711)]
        [TestCase(-1)]
        public void HPSetterShouldThrowExceptionIfHPIsNegative(int hp)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new Warrior("Pesho", 60, hp));

            Assert.AreEqual("HP should not be negative!", ex.Message);
        }

        [Test]
        public void AttackShouldThrowExceptionIfAttackerHPIsLessThanMinAttackHp()
        {
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => weakerWarrior.Attack(warrior));

            Assert.AreEqual("Your HP is too low in order to attack other warriors!", ex.Message);
        }

        [Test]
        public void AttackShouldThrowExceptionIfEnemyHasLessThanMinAttackHp()
        {
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => warrior.Attack(weakerWarrior));

            Assert.AreEqual($"Enemy HP must be greater than {MIN_ATTACK_HP} in order to attack him!", ex.Message);
        }

        [Test]
        public void AttackShouldThrowExceptionWarriorAttacksEnemyWithMoreDamage()
        {
            Warrior veryWeak = new Warrior(weakerWarrior.Name, weakerWarrior.Damage, weakerWarrior.HP + 2);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => veryWeak.Attack(warrior));

            Assert.AreEqual($"You are trying to attack too strong enemy", ex.Message);
        }


        [Test]
        public void AttackShouldThrowZeroOutEnemyHealthIsDamageIsMoreTanItsHealth()
        {
            Warrior warrior = new Warrior("gggg", 34, 35);

            warrior.Attack(warrior);

            Assert.AreEqual(0, warrior.HP);
        }

        [Test]
        public void AttackShouldThrowLowerEnemyHealth()
        {
            Warrior veryWeak = new Warrior("gggg", 31, 55);

            warrior.Attack(veryWeak);

            Assert.AreEqual(5, veryWeak.HP);
        }
    }
}
