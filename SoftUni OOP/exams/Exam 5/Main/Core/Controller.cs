using BookingApp.Core.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using BookingApp.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BookingApp.Core
{
    public class Controller : IController
    {
        private IRepository<IHotel> hotelRepository;

        public Controller()
        {
            hotelRepository = new HotelRepository();
        }


        public string AddHotel(string hotelName, int category)
        {
            string result = "";

            if (hotelRepository.Select(hotelName) is not null)
            {
                result = string.Format(OutputMessages.HotelAlreadyRegistered, hotelName);
            }
            else
            {
                IHotel hotel = new Hotel(hotelName, category);

                hotelRepository.AddNew(hotel);

                result = string.Format(OutputMessages.HotelSuccessfullyRegistered, category, hotelName);
            }

            return result.TrimEnd();
        }

        public string BookAvailableRoom(int adults, int children, int duration, int category)
        {
            string result = "";

            List<IHotel> hotels = hotelRepository.All().OrderBy(x => x.FullName).Where(x => x.Category == category).ToList();

            if (!hotels.Any())
            {
                result = string.Format(OutputMessages.CategoryInvalid, category);
            }
            else
            {
                List<IRoom> rooms = new();
                foreach (var hotel in hotels)
                {
                    List<IRoom> rooms1 = hotel.Rooms.All()
                                              .Where(x => x.PricePerNight > 0)
                                              .ToList();

                    rooms.AddRange(rooms1);
                }

                int neededCapacity = adults + children;

                IRoom bestRoom = rooms.OrderBy(x => x.BedCapacity).FirstOrDefault(x => x.BedCapacity >= neededCapacity);

                if (bestRoom == null)
                {
                    result = string.Format(OutputMessages.RoomNotAppropriate);
                }
                else
                {
                    IHotel hotel = hotelRepository.All().FirstOrDefault(x => x.Rooms.All().Any(x => x == bestRoom));
                    IBooking booking = new Booking(bestRoom, duration, adults, children, hotel.Bookings.All().Count + 1);

                    hotel.Bookings.AddNew(booking);

                    result = string.Format(OutputMessages.BookingSuccessful, booking.BookingNumber, hotel.FullName);
                }
            }

            return result.TrimEnd();
        }

        public string HotelReport(string hotelName)
        {
            StringBuilder sb = new StringBuilder();

            IHotel hotel = hotelRepository.Select(hotelName);

            if (hotel is null)
            {
                sb.AppendLine(String.Format(OutputMessages.HotelNameInvalid, hotelName));
            }
            else 
            {
                sb.AppendLine($"Hotel name: {hotelName}");
                sb.AppendLine($"--{hotel.Category} star hotel");
                sb.AppendLine($"--Turnover: {hotel.Turnover:F2} $");
                sb.AppendLine("--Bookings:");
                sb.AppendLine();

                if (hotel.Bookings.All().Any())
                {
                    foreach (var booking in hotel.Bookings.All())
                    {
                        sb.AppendLine(booking.BookingSummary());
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine("none");
                }

            }

           return sb.ToString().TrimEnd();
        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            string result = "";

            IHotel hotel = hotelRepository.Select(hotelName);

            if (hotel is null)
            {
                result = string.Format(OutputMessages.HotelNameInvalid, hotelName);
            }
            else if (roomTypeName != nameof(DoubleBed) && roomTypeName != nameof(Apartment) && roomTypeName != nameof(Studio))
            {
                throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);
            }
            else if (!hotel.Rooms.All().Any(x => x.GetType().Name == roomTypeName))
            {
                result = string.Format(OutputMessages.RoomTypeNotCreated);
            }
            else if (hotel.Rooms.All().FirstOrDefault(x => x.GetType().Name == roomTypeName).PricePerNight != 0)
            {
                throw new InvalidOperationException(ExceptionMessages.PriceAlreadySet);
            }
            else
            {
                hotel.Rooms.All().FirstOrDefault(x => x.GetType().Name == roomTypeName).SetPrice(price);

                result = string.Format(OutputMessages.PriceSetSuccessfully, roomTypeName, hotelName);
            }

            return result.TrimEnd();

        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            string result = "";

            IHotel hotel = hotelRepository.Select(hotelName);

            if ( hotel is null)
            {
                result = string.Format(OutputMessages.HotelNameInvalid, hotelName);
            }
            else if (roomTypeName != nameof(DoubleBed) && roomTypeName != nameof(Apartment) && roomTypeName != nameof(Studio))
            {
                throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);
            }
            else if (hotel.Rooms.All().Any(x => x.GetType().Name == roomTypeName))
            {
                result = string.Format(OutputMessages.RoomTypeAlreadyCreated);
            }
            else
            {
                IRoom room;

                if (roomTypeName == nameof(DoubleBed))
                {
                    room = new DoubleBed();
                }
                else if (roomTypeName == nameof(Studio))
                {
                    room = new Studio();
                }
                else
                {
                    room = new Apartment();
                }

                hotel.Rooms.AddNew(room);

                result = string.Format(OutputMessages.RoomTypeAdded, roomTypeName, hotelName);
            }


            return result.TrimEnd();
        }
    }
}