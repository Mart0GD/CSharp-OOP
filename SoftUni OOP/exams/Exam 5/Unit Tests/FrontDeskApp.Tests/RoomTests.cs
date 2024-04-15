using FrontDeskApp;
using NUnit.Framework;
using System;

namespace BookigApp.Tests
{
    public class HotelUnitTests
    {
        Hotel hotel;

        [SetUp]
        public void Setup()
        {
            hotel = new Hotel("Perl Beach", 3);
        }

        [Test]
        public void ConstructorShouldWork()
        {
            Assert.NotNull(hotel);
            Assert.AreEqual("Perl Beach", hotel.FullName);
            Assert.AreEqual(3, hotel.Category);
            Assert.AreEqual(0, hotel.Turnover);
            Assert.NotNull(hotel.Rooms);
            Assert.NotNull(hotel.Bookings);
        }

        [TestCase(null)]
        [TestCase("     ")]
        public void FullName_Setter_Exception(string name)
        {
            Assert.Throws<ArgumentNullException>(() => new Hotel(name, 3));
        }

        [TestCase(0)]
        [TestCase(7)]
        public void Category_Setter_Exception(int category)
        {
            Assert.Throws<ArgumentException>(() => new Hotel("Pearl Beach", category));
        }

        [Test]
        public void AddRoom_IncreasesCount()
        {
            hotel.AddRoom(new Room(2, 10));

            int expected = 1;
            int actual = hotel.Rooms.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BookRoom_Succees()
        {
            hotel.AddRoom(new Room(2, 10));

            hotel.BookRoom(1, 0, 2, 100);

            int expectedBookingCount = 1;
            int actualBookingsCount = hotel.Bookings.Count;

            double expectedTurnover = 20;
            double actualTurnover = hotel.Turnover;

            Assert.AreEqual(expectedBookingCount, actualBookingsCount);
            Assert.AreEqual(expectedTurnover, actualTurnover);
        }

        [Test]
        public void BookRoom_NothingHappens()
        {
            hotel.AddRoom(new Room(2, 10));

            hotel.BookRoom(3, 0, 2, 100);

            int expectedBookingCount = 0;
            int actualBookingsCount = hotel.Bookings.Count;

            double expectedTurnover = 0;
            double actualTurnover = hotel.Turnover;

            Assert.AreEqual(expectedBookingCount, actualBookingsCount);
            Assert.AreEqual(expectedTurnover, actualTurnover);
        }

        [Test]
        public void BookRoom_BudgetNotEnough()
        {
            hotel.AddRoom(new Room(2, 100));

            hotel.BookRoom(3, 0, 2, 100);

            int expectedBookingCount = 0;
            int actualBookingsCount = hotel.Bookings.Count;

            double expectedTurnover = 0;
            double actualTurnover = hotel.Turnover;

            Assert.AreEqual(expectedBookingCount, actualBookingsCount);
            Assert.AreEqual(expectedTurnover, actualTurnover);
        }

        [Test]
        public void BookRoom_ExceptionAdults()
        {
            Assert.Catch<ArgumentException>(() => hotel.BookRoom(0, 1, 2, 100));
        }

        [Test]
        public void BookRoom_ExceptionChildren()
        {
            Assert.Catch<ArgumentException>(() => hotel.BookRoom(1, -1, 2, 100));
        }

        [Test]
        public void BookRoom_ExceptionDuratiopn()
        {
            Assert.Catch<ArgumentException>(() => hotel.BookRoom(1, 2, 0, 100));
        }


    }
}