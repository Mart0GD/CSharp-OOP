﻿using EDriveRent.Models.Contracts;
using EDriveRent.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDriveRent.Models
{
    public abstract class Vehicle : IVehicle
    {
        private string brand;
        private string model;
        private double maxMileage;
        private string licensePlateNumber;
        private int batteryLevel;
        private bool isDamaged;

        public Vehicle(string brand, string model, double maxMileage, string licensePlateNumber)
        {
            Brand = brand;
            Model = model;
            MaxMileage = maxMileage;
            LicensePlateNumber = licensePlateNumber;
            BatteryLevel = 100;
            IsDamaged = false;
        }

        public string Brand 
        { 
            get => brand;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.BrandNull);
                }

                brand = value;
            }
        }

        public string Model 
        {
            get => model;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.ModelNull);
                }
                model = value;
            }

        }

        public double MaxMileage { get => maxMileage; private set => maxMileage = value; }

        public string LicensePlateNumber 
        { 
            get => licensePlateNumber;

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.LicenceNumberRequired);
                }

                licensePlateNumber = value;
            }
        }

        public int BatteryLevel { get => batteryLevel; private set => batteryLevel = value; }

        public bool IsDamaged { get => isDamaged; private set => isDamaged = value; }

        public void Drive(double mileage)
        {
            // 180 - 90 
            int driven = this.GetType() == typeof(CargoVan)
                            ? (int)Math.Round((mileage / MaxMileage) * 100) + 5
                            : (int)Math.Round((mileage / MaxMileage) * 100);

            BatteryLevel -= driven;

        }

        public void ChangeStatus()
        {
            IsDamaged = !IsDamaged;
        }

        public void Recharge()
        {
            BatteryLevel = 100;
        }

        public override string ToString()
        {
            return $"{Brand} {Model} License plate: {LicensePlateNumber} Battery: {BatteryLevel}% Status: {(isDamaged ? "damaged" : "OK")}";

        }
    }
}
