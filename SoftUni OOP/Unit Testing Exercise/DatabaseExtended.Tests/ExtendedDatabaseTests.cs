namespace DatabaseExtended.Tests
{
    using ExtendedDatabase;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ExtendedDatabaseTests
    {
        Database db;
        Person person;

        [SetUp] 
        public void SetUp() 
        {
            db = new();
            person = new(9998, "Pesho");
        }

        [Test]
        public void DatabaseConstructorShouldWork()
        {
            Assert.IsNotNull(db);
            Assert.AreEqual(0, db.Count);
        }

        [Test]
        public void AddRangeShouldIncreaseCount()
        {
            db = new Database(new Person[] { person, new Person(9999, "Gosho") });

            Assert.AreEqual(2, db.Count);
        }

        [Test]
        public void AddrangeShouldThrowExceptionWhenTresholdIsReached()
        {
           ArgumentException ex = Assert.Catch<ArgumentException>( () => new Database(new Person[] { person, person, person, person, person, person, person, person, person, person, person, person, person, person, person, person, person }));

            Assert.AreEqual("Provided data length should be in range [0..16]!", ex.Message);
        }

        [Test]
        public void AddShouldIncreaseCount()
        {
            db.Add(person);

            Assert.AreEqual(1, db.Count);
        }

        [Test]
        public void AddShouldThrowExcpetionIfArrayLengthIsReached()
        {
            Random rnd = new Random();
            for (int i = 0; i < 17; i++)
            {
                if (i == 16)
                {
                    InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => db.Add(new Person(rnd.Next(), rnd.Next().ToString())));

                    Assert.AreEqual("Array's capacity must be exactly 16 integers!", ex.Message);
                    break;
                }
                db.Add(new Person(rnd.Next(), rnd.Next().ToString()));
            }
        }

        [Test]
        public void AddShouldThrowExcpetionIfSamePersonIsAdded()
        {
            db.Add(person);

            InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => db.Add(person));

            Assert.AreEqual("There is already user with this username!", ex.Message);
        }

        [Test]
        public void AddShouldThrowExcpetionIfSameIdIsAdded()
        {
            db.Add(person);

            InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => db.Add(new Person(person.Id, "Peshko")));

            Assert.AreEqual("There is already user with this Id!", ex.Message);
        }

        [Test]
        public void RemoveShouldDecreaseCount()
        {
            db.Add(person);
            db.Remove();

           
            Assert.AreEqual(0,db.Count);
        }

        [Test]
        public void RemoveShouldThrowExceptionIsDatabaseIsEmpty()
        {
             Assert.Catch<InvalidOperationException>(() => db.Remove());
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void FindByUsernameShouldThrowExceptionIfNameIsEmpty(string name)
        {
            ArgumentException ex = Assert.Catch<ArgumentException>(() => db.FindByUsername(name));

            Assert.AreEqual("Username parameter is null!", ex.ParamName);
        }

        [Test]
        [TestCase(" ")]
        [TestCase("Kiro")]
        public void FindByUsernameShouldThrowExceptionIfNameIsNotPresent(string name)
        {
            db.Add(person);

            InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => db.FindByUsername(name));

            Assert.AreEqual("No user is present by this username!", ex.Message);
        }

        [Test]
        public void FindByUsernameShouldReturnPerson()
        {
            db.Add(person);

            Assert.AreEqual(person, db.FindByUsername(person.UserName));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-2889291)]
        public void FindByIdShouldThrowExceptionIfIdIsNegative(int id)
        {
            ArgumentOutOfRangeException ex = Assert.Catch<ArgumentOutOfRangeException>(() => db.FindById(id));

            Assert.AreEqual("Id should be a positive number!", ex.ParamName);
        }

        [Test]
        [TestCase(12121)]
        [TestCase(2)]
        public void FindByIdShouldThrowExceptionIfThereIsNoSameId(int id)
        {
            InvalidOperationException ex = Assert.Catch<InvalidOperationException>(() => db.FindById(id));

            Assert.AreEqual("No user is present by this ID!", ex.Message);
        }

        [Test]
        public void FindByIdShouldReturnPerosn()
        {
            db.Add(person);

            Assert.AreEqual(person, db.FindById(person.Id));
        }

       


    }
}