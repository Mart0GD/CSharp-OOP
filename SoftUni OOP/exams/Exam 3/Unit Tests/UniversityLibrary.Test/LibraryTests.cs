namespace UniversityLibrary.Test
{
    using NUnit.Framework;
    using System.Text;

    public class LibraryTests
    {
        UniversityLibrary library;

        [SetUp]
        public void Setup()
        {
            library = new UniversityLibrary();
        }

        [Test]
        public void ConstructorShouldWork()
        {
            Assert.IsNotNull(library);
            Assert.IsNotNull(library.Catalogue);
        }

        [Test]
        public void AddTextBookToLibrary_IncreasesCount()
        {
            library.AddTextBookToLibrary(new TextBook("Neshto", "Pesho", "Drama"));

            int expected = 1;
            int actual = library.Catalogue.Count;

            int expectedInventoryNumber = 1;
            int actualInventoryNumber = library.Catalogue[0].InventoryNumber;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddTextBookToLibrary_ReturnsString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Book: Neshto - 1");
            sb.AppendLine($"Category: Drama");
            sb.AppendLine($"Author: Pesho");

            string expected = sb.ToString().TrimEnd();
            string actual = library.AddTextBookToLibrary(new TextBook("Neshto", "Pesho", "Drama"));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoanTextBook_LoanerSucceed()
        {
            library.AddTextBookToLibrary(new TextBook("Neshto", "Pesho", "Drama"));

            string expected = "Neshto loaned to Gosho.";
            string actual = library.LoanTextBook(1, "Gosho");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoanTextBook_LoanerNotReturned()
        {
            library.AddTextBookToLibrary(new TextBook("Neshto", "Pesho", "Drama"));
            library.LoanTextBook(1, "Gosho");

            string expected = "Gosho still hasn't returned Neshto!";
            string actual = library.LoanTextBook(1, "Gosho");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnTextBook_ClearsHolderAndReturnsString()
        {
            library.AddTextBookToLibrary(new TextBook("Neshto", "Pesho", "Drama"));
            library.LoanTextBook(1, "Gosho");

            string expected = "Neshto is returned to the library.";
            string actual = library.ReturnTextBook(1);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(string.Empty, library.Catalogue[0].Holder);
        }
    }
}