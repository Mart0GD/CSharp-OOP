namespace Database.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class DatabaseTests
    {
        Database db;

        [SetUp]
        public void SetUp()
        {
            db = new Database();
        }

        [Test]
        public void ConstructorShouldWork()
        {
            Assert.AreEqual(db.Count, 0);
            Assert.IsTrue(db is not null);
        }

        [Test]
        public void AddMethodShouldIncreaseCount()
        {
            db.Add(1);

            Assert.AreEqual(1, db.Count);
        }

        [Test]
        public void AddMethodShouldThrowExceptionWhenDatabaseIsFull()
        {
            for (int i = 0; i < 16; i++)
            {
                db.Add((int)i);
            }

            InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => db.Add(1));
            Assert.AreEqual(ex.Message, "Array's capacity must be exactly 16 integers!");
        }

        [Test]
        public void RemoveMethodDecreasesCount()
        {
            db.Add(1);
            db.Remove();

            Assert.AreEqual(0, db.Count);
        }

        [Test]
        public void RemoveMethodShouldThrowExceptionWhenDatabaseIsEmpty()
        {
            InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => db.Remove());
            Assert.AreEqual(ex.Message, "The collection is empty!");
        }

        [Test]
        public void FetchShouldReturnArray()
        {
            db.Add(1);

            Assert.AreEqual(db.Fetch(), new int[] { 1 });
        }
    }
}
