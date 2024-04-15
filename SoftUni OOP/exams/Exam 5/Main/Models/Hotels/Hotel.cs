using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Models.Hotels
{
    public class Hotel : IHotel
    {
        public Hotel(string fullName, int category)
        {
            FullName = fullName;
            Category = category;

            Rooms = new RoomRepository();
            Bookings = new BookingRepository();
        }

        public string FullName { get; private set; }

        public int Category { get; private set; }

        public double Turnover => Bookings.All().Sum(x => x.ResidenceDuration * x.Room.PricePerNight);

        public IRepository<IRoom> Rooms { get;  set; }

        public IRepository<IBooking> Bookings { get; set; }
    }
}
