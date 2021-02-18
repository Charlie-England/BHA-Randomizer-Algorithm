using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHA_Randomizer_NET
{
    public class EquitabilityChecker
    {
        public static bool CheckEquitability(List<ProviderModel> providerList, int count, int numOfProviders, int targetDaysOut, double equitabilityDifficultyModifier, int numProvidersAssigned)
        {
            ///
            /// modifies moveOn in the Main fxn by returning a bool
            /// return true of the loop is done or
            /// return true if the equitibility is bad (number of times a provider is chosen is too much)
            /// If moveOn is modified to true (returned value) then this causes the function above to break out of the do loop
            ///
/*            if (count == targetDaysOut)
            {
                return true;
            }
            else
            {*/
            foreach (ProviderModel provider in providerList)
            {
                var providerEquitableAssignNumHigh = ((targetDaysOut / numOfProviders) + equitabilityDifficultyModifier) * provider.Fte;
                var providerEquitableAssignNumLow = ((targetDaysOut / numOfProviders) - equitabilityDifficultyModifier) * provider.Fte;
                //Console.WriteLine($"{provider.Name} ({provider.assignedValue}) {providerEquitableAssignNumLow}-{providerEquitableAssignNumHigh}");

                if (numProvidersAssigned == targetDaysOut) //loop is done, do full equitability check
                {
                    if (provider.assignedValue > providerEquitableAssignNumHigh || provider.assignedValue < providerEquitableAssignNumLow)
                    {
                        Console.WriteLine("Final Equitability Check Failed...retrying...\n");
                        return true;
                    }
                }
                else if (provider.assignedValue > providerEquitableAssignNumHigh)
                {
                    return true;
                }
            }
            return false;

            //}
        }
    }
}