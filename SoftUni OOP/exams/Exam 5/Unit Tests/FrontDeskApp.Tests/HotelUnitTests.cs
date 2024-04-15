using FrontDeskApp;
using NUnit.Framework;
using System;

namespace BookigApp.Tests
{
    public class RoomTests
    {
        Hotel hotel;

        [SetUp]
        public void Setup()
        {
            hotel = new Hotel("Perl Beach", 3);
        }

        
        [TestCase(0)]
        [TestCase(-1)]
        public void BedCapacity_SetterException(int capacity)
        {
            Assert.Catch<ArgumentException>(() => new Room(capacity,10));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void PricePerNight_SetterException(int value)
        {
            Assert.Catch<ArgumentException>(() => new Room(2, value));
        }


    }
}