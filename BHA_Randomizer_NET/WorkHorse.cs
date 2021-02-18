using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHA_Randomizer_NET
{
    public class WorkHorse
    {
        public static bool MainLogicPath(List<ProviderModel> providerAssignedList, List<ProviderModel> providerList, int lookBackDays, int day, Random rnd)
        {
            /*recieves providerAssignedList (current list of of assigned providers in order)
            recieves providerList (current list of providers)
            equitabilityDict (maintians providers and times assigned)
            lookBackDays: the days to look back when deciding when to assign, 
            if lookback days = 5 and a provider is within 4 of the last spot in provider asigned List, will not assign that provider but will assign if the -6 index
            */
            if (providerAssignedList.Count() < lookBackDays) //reassigns lookbackdays to avoid out of range exception when in early assignments of providerAssignedList
            {
                lookBackDays = providerAssignedList.Count();
            }

            //Creates a sacrificialList, providers will be removed from this list until a suitable provider is found or until the sacrificialList is empty
            //If the sacrificialList is empty, break and return false
            //if a suitiable provider is found, add to the providerAssignedList, account for this in equitability and return true
            List<ProviderModel> sacrificialList = new List<ProviderModel>(providerList);
            while (true)
            {
                if (sacrificialList.Count() == 0)
                    break;
                int choice = rnd.Next(0, sacrificialList.Count()); //Grab a random index from 0-the count of sacrificialList
                ProviderModel personChoice = sacrificialList[choice];
                sacrificialList.RemoveRange(choice, 1);
                if (CheckDay(personChoice, day, providerAssignedList, lookBackDays))
                {
                    providerAssignedList.Add(personChoice);

                    //Add 1 to the num assigned for the provider chosen
                    personChoice.assignedValue++;


                    return false;
                }

            }
            return true;
        }

        private static bool CheckDay(ProviderModel person, int day, List<ProviderModel> providerAssignedList, int lookBackDays)
        {
            int targetIndex;
            ///
            /// checks 2 scenarios, the day of the week and the current list of providers
            /// If at anytime one of these fails, it will return false and MainLogicPath will continue
            ///

            switch (day) //check to see if the person.Day returns true, break out and continue if true, return false if not
            {
                case 1:
                    if (person.Monday)
                        break;
                    return false;
                case 2:
                    if (person.Tuesday)
                        break;
                    return false;
                case 3:
                    if (person.Wednesday)
                        break;
                    return false;
                case 4:
                    if (person.Thursday)
                        break;
                    return false;
                case 5:
                    if (person.Friday)
                        break;
                    return false;
            }
            lookBackDays *= -1;
            for (int i = -1; i >= lookBackDays; i--)
            {
                targetIndex = providerAssignedList.Count() + i;
                if (Equals(providerAssignedList[targetIndex].Name, person.Name))
                {
                    return false;
                }

            }
            return true; //everything passed, okay to add this person to the list
        }

    }
}