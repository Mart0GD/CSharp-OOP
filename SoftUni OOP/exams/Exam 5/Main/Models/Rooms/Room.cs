using BookingApp.Models.Rooms.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Models.Rooms
{
    public abstract class Room : IRoom
    {
        private readonly int bedCapacity;
        public Room(int bedCapacity)
        {
            this.bedCapacity = bedCapacity;
        }

        public int BedCapacity => bedCapacity;

        public double PricePerNight { get; private set; }

        public void SetPrice(double price) => PricePerNight = price;
        
    }
}
