using ChristmasPastryShop.Core.Contracts;
using ChristmasPastryShop.Models.Booths;
using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

namespace ChristmasPastryShop.Core
{
    public class Controller : IController
    {
        private IRepository<IBooth> booths;

        public Controller()
        {
            booths = new BoothRepository();
        }

        public string AddBooth(int capacity)
        {
            int id = booths.Models.Count + 1;

            IBooth booth = new Booth(id, capacity);

            booths.AddModel(booth);

            return String.Format(OutputMessages.NewBoothAdded, id, capacity);
        }

        public string AddCocktail(int boothId, string cocktailTypeName, string cocktailName, string size)
        {
            string result = "";

            if (cocktailTypeName != nameof(Hibernation) && cocktailTypeName != nameof(MulledWine))
            {
                result = String.Format(OutputMessages.InvalidCocktailType, cocktailTypeName);
            }
            else if (size != "Large" && size != "Middle" && size != "Small")
            {
                result = String.Format(OutputMessages.InvalidCocktailSize, size);
            }
            else if (booths.Models.Any(booth => booth.CocktailMenu
                                                .Models
                                                .Any(cocktail => cocktail.Name == cocktailName
                                                     && cocktail.Size == size)))
            {
                result = String.Format(OutputMessages.CocktailAlreadyAdded, size, cocktailName);
            }
            else
            {
                ICocktail cocktail = null;

                if (cocktailTypeName == nameof(Hibernation))
                {
                    cocktail = new Hibernation(cocktailName, size);
                }
                else
                {
                    cocktail = new MulledWine(cocktailName, size);
                }

                booths.Models.FirstOrDefault(x => x.BoothId == boothId).CocktailMenu.AddModel(cocktail);

                result = String.Format(OutputMessages.NewCocktailAdded, size, cocktailName, cocktailTypeName);
            }


            return result.TrimEnd();
        }

        public string AddDelicacy(int boothId, string delicacyTypeName, string delicacyName)
        {
            string result = "";

            if (delicacyTypeName != nameof(Gingerbread) && delicacyTypeName != nameof(Stolen))
            {
                result = String.Format(OutputMessages.InvalidDelicacyType, delicacyTypeName);
            }
            else if (booths.Models.Any(
                     menues => menues.DelicacyMenu.Models.
                     Any(delicacy => delicacy.Name == delicacyName)))
            {
                result = String.Format(OutputMessages.DelicacyAlreadyAdded, delicacyName);
            }
            else
            {
                IDelicacy delicacy = null;

                if (delicacyTypeName == nameof(Gingerbread))
                {
                    delicacy = new Gingerbread(delicacyName);
                }
                else
                {
                    delicacy = new Stolen(delicacyName);
                }

                booths.Models.FirstOrDefault(x => x.BoothId == boothId).DelicacyMenu.AddModel(delicacy);
                result = String.Format(OutputMessages.NewDelicacyAdded, delicacyTypeName, delicacyName);
            }


            return result.TrimEnd();
        }

        public string BoothReport(int boothId)
        {
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);

            return booth.ToString();
        }

        public string LeaveBooth(int boothId)
        {
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format(OutputMessages.GetBill, $"{booth.CurrentBill:f2}"));
            sb.AppendLine(String.Format(OutputMessages.BoothIsAvailable, boothId));

            booth.Charge();
            booth.ChangeStatus();

            return sb.ToString().TrimEnd();
        }

        public string ReserveBooth(int countOfPeople)
        {
            string result = "";

            IBooth compatableBooth = booths.Models
                                                 .Where(x => !x.IsReserved && x.Capacity >= countOfPeople)
                                                 .OrderBy(x => x.Capacity)
                                                 .ThenByDescending(x => x.BoothId)
                                                 .FirstOrDefault();

            if (compatableBooth is null)
            {
                result = String.Format(OutputMessages.NoAvailableBooth, countOfPeople);
            }
            else
            {
                compatableBooth.ChangeStatus();

                result = String.Format(OutputMessages.BoothReservedSuccessfully, compatableBooth.BoothId, countOfPeople);
            }


            return result.TrimEnd();
        }

        public string TryOrder(int boothId, string order)
        {
            string result = "";
            bool isCocktail = false;

            string[] tokens = order.Split("/", StringSplitOptions.RemoveEmptyEntries);

            string itemTypeName = tokens[0];
            string itemName = tokens[1];
            int orderedCount = int.Parse(tokens[2]);
            string cocktailSize = "";

            if (itemTypeName == nameof(MulledWine) || itemTypeName == nameof(Hibernation))
            {
                cocktailSize = tokens[3];
                isCocktail = true;
            }

            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);

            if (itemTypeName != nameof(MulledWine)
                && itemTypeName != nameof(Hibernation)
                && itemTypeName != nameof(Gingerbread)
                && itemTypeName != nameof(Stolen))
            {
                result = String.Format(OutputMessages.NotRecognizedType, itemTypeName);

            }
            else if (isCocktail && !booth.CocktailMenu.Models.Any(x => x.Name == itemName))
            {
                result = String.Format(OutputMessages.CocktailStillNotAdded, itemTypeName, itemName);
            }
            else if (!isCocktail && !booth.DelicacyMenu.Models.Any(x => x.Name == itemName))
            {
                result = String.Format(OutputMessages.DelicacyStillNotAdded, itemTypeName, itemName);
            }
            else
            {
                Cocktail cocktail = null;
                Delicacy delicacy = null;

                CreateOrders(itemTypeName, itemName, cocktailSize, ref cocktail, ref delicacy);

                if (isCocktail)
                {
                    if (!booth.CocktailMenu.Models.Any(x => x.Name == itemName && x.GetType().Name == itemTypeName && x.Size == cocktailSize))
                    {
                        result = String.Format(OutputMessages.CocktailStillNotAdded, cocktailSize, itemName);
                    }
                    else
                    {
                        booth.UpdateCurrentBill(cocktail.Price * orderedCount);

                        result = String.Format(OutputMessages.SuccessfullyOrdered, boothId, orderedCount, itemName);
                    }
                }
                else
                {
                    if (!booth.DelicacyMenu.Models.Any(x => x.GetType().Name == itemTypeName && x.Name == itemName))
                    {
                        result = String.Format(OutputMessages.DelicacyStillNotAdded, itemTypeName, itemName);
                    }
                    else
                    {
                        booth.UpdateCurrentBill(delicacy.Price * orderedCount);

                        result = String.Format(OutputMessages.SuccessfullyOrdered, boothId, orderedCount, itemName);
                    }
                }


            }

            return result.TrimEnd();
        }

        private static void CreateOrders(string itemTypeName, string itemName, string cocktailSize, ref Cocktail cocktail, ref Delicacy delicacy)
        {
            if (itemTypeName == nameof(Hibernation) || itemTypeName == nameof(MulledWine))
            {
                if (itemTypeName == nameof(Hibernation))
                {
                    cocktail = new Hibernation(itemName, cocktailSize);
                }
                else
                {
                    cocktail = new MulledWine(itemName, cocktailSize);
                }

            }
            else if (itemTypeName == nameof(Gingerbread) || itemTypeName == nameof(Stolen))
            {
                if (itemTypeName == nameof(Gingerbread))
                {
                    delicacy = new Gingerbread(itemName);
                }
                else
                {
                    delicacy = new Stolen(itemName);
                }

            }
        }
    }
}
