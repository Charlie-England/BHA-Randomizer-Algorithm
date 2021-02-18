using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BHA_Randomizer_NET
{
    class Program
    {
        static List<ProviderModel> providerList;
        static void Main(string[] args)
        {
            Console.WriteLine("Loading Providers From Excel...");

            providerList = ExcelProviders.getExcelData();
            //List<ProviderModel> providerList = getJsonData(); //JSON datastuff

            string postCode = "";
            string userInput = " ";
            string clinicList = getClinicList("string");

            while (!userInput.Equals("") && !userInput.Equals("q") && !userInput.Equals("quit"))
            {
                printIntro(postCode, clinicList);
                postCode = "";
                userInput = Console.ReadLine();

                if (userInput == "rand")
                {
                    postCode = getRandomized();
                } else if (userInput == "update")
                {
                    Console.WriteLine("Please use Excel file providers.xlsx to modify providers");
                    //ProviderUpdate.updateMenu(providerList, getClinicList(), getClinicList("string"));
                } else
                {
                    postCode = userInput + " is not recognized, please see below for menu";
                    Console.Clear();
                }
            }
            
            Console.WriteLine("Press 'enter' to exit");
            Console.ReadLine();
        }

/*        private static List<ProviderModel> getJsonData()
        {
            *//*
             * Returns a list of provider model from the current provider_info.json file
             *//*
            string providerJson = File.ReadAllText(@"provider_info.json");
            List<ProviderModel> providerList = JsonConvert.DeserializeObject<List<ProviderModel>>(providerJson);

            return providerList;
        }*/
        
        private static List<ProviderModel> getClinicProviders(string clinic)
        {
            //returns a list with only the clinic chosen, if clinic does not match any, returns an empty list
            List<ProviderModel> parsedList = new List<ProviderModel>();
            //List<ProviderModel> allProviderList = ExcelProviders.getExcelData();

            for (int i = 0; i < providerList.Count; i++)
            {
                if (providerList[i].clinic.ToLower().Equals(clinic.ToLower()))
                {
                    parsedList.Add(providerList[i]);
                }
            }

            return parsedList;
        }

        private static List<string> getClinicList()
        {
            /*
             * Iterates through the providerList and adds each unique clinic to a string build and returns this tring
             */
            List<string> clinics = new List<string>();

            //Fencepost
            clinics.Add(providerList[0].clinic);

            for (int i = 1; i < providerList.Count(); i++)
            {
                if (!clinics.Contains(providerList[i].clinic))
                {
                    clinics.Add(providerList[i].clinic);
                }
            }

            return clinics;
        }

        private static string getClinicList(string type)
        {
            List<string> clinicList = getClinicList();

            StringBuilder clinicsString = new StringBuilder();
            clinicsString.Append(clinicList[0]);
            for (int i = 1; i < clinicList.Count(); i++)
            { 
                clinicsString.Append(", " + clinicList[i]);
            }
          
            return clinicsString.ToString();

        }

        private static string getRandomized()
        {
            Console.WriteLine("Current Live Clinics: " + getClinicList("string"));
            string clinic = getUserInput("Choose a clinic (see current live clinics)\n>>>");

            List<ProviderModel> ProviderList = getClinicProviders(clinic);
            if (ProviderList.Count == 0)
            {
                return "Error with clinic choice: " + clinic;
            }

            string targetDaysOutInput = getUserInput("Rotation for how many days? ('enter' for default 100) \nWARNING!!! choosing more than 200 could run into problems >>>");
            int targetDaysOut = 100; //90 days out, looking at 3 months as the default
            if (!targetDaysOutInput.Equals(""))
            {
                targetDaysOut = int.Parse(targetDaysOutInput);
            }

            Console.WriteLine(clinic);
            Console.WriteLine(targetDaysOut);



            Random rnd = new Random();
            bool breakOutOfLoop = false;
            int difficultyLoopCounter = 0;
            double equitabilityDifficultyModifier = 2; //will check to make sure providers are assigned within +- this value


            int lookBackDays = ProviderList.Count() - 2;


            // badRandMoveOn is a bool and decides whether or not to break out of the internal do loop
            // if badRandMoveOn is true it means that the targetDaysOut has been reached or the equitibility is bad (via the CheckEquitibility Fn)
            Console.WriteLine($"Target days out:{targetDaysOut}, equitability difficulty - must be +- {equitabilityDifficultyModifier} of desired value\nLook Back Days:{lookBackDays}\nWorking...");
            while (breakOutOfLoop == false)
            {
                //Difficulty adjustment, if the loopcounter is greater than a set point, it removes 1 from the lookBackDays variable.
                //Removing 1 from the lookbackdays makes the calculation easier
                difficultyLoopCounter++;

                if (difficultyLoopCounter % (100000 / 5) == 0)
                {
                    Console.WriteLine("Working...");
                }

                if (difficultyLoopCounter > 100000)
                {
                    difficultyLoopCounter = 0;
                    if (lookBackDays > 1)
                    {
                        lookBackDays -= 1;
                        Console.WriteLine($"Too difficult, lowering back check days to {lookBackDays}\n");
                        Console.WriteLine($"Working...\ntarget days out:{targetDaysOut}, equitability difficulty - must be +- {equitabilityDifficultyModifier} of desired value\nLook Back Days:{lookBackDays}");
                    }
                }

                int count = 0;
                int day = 1;
                List<ProviderModel> ProviderAssignedList = new List<ProviderModel>();

                //Initiate dictionary with key=name of provider and value = 0
                foreach (ProviderModel provider in ProviderList)
                {
                    provider.resetAssignedValue();
                }

                int numOfProviders = ProviderList.Count();

                //Main do loop
                do
                {
                    breakOutOfLoop = WorkHorse.MainLogicPath(ProviderAssignedList, ProviderList, lookBackDays, day, rnd);
                    count++;


                    day++;
                    if (day > 5) //reset day past 6 (aka reset to Monday)
                        day = 1;

                    breakOutOfLoop = EquitabilityChecker.CheckEquitability(ProviderList, count, numOfProviders, targetDaysOut, equitabilityDifficultyModifier, ProviderAssignedList.Count());
                    if (!breakOutOfLoop && ProviderAssignedList.Count() == targetDaysOut)
                    {
                        break;
                    }
                    if (breakOutOfLoop) //if true, break out of 
                        break;
                } while (count < targetDaysOut);

                if (ProviderAssignedList.Count() < targetDaysOut) //check if the loop as been done enough, if so, go to done, write to CSV
                {
                    breakOutOfLoop = false;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(">>>>>>>>>>>> DONE <<<<<<<<<<<<<");
                    Console.WriteLine();
                    Console.WriteLine("Equitability Check");
                    //write to csv
                    foreach (ProviderModel person in ProviderList)
                    {
                        Console.WriteLine($"{person.Name}-({person.Fte} fte) : {person.assignedValue}");
                    }
                    breakOutOfLoop = true;

                    //write to CSV
                    string filePath = writeToCSV(ProviderAssignedList, clinic, targetDaysOut);

                    Console.WriteLine();
                    Console.WriteLine($"CSV file created and stored in {filePath}");
                }
            }
            return "";
        }

        private static string writeToCSV(List<ProviderModel> providerAssignedList, string clinic, int targetDaysOut)
        {
            var csv = new StringBuilder();
            var today = DateTime.Today.ToString("MM-dd-yy");

            //Create directory 
            var directoryName = Directory.GetCurrentDirectory();
            var directoryPath = $"{directoryName}\\csv";

            try
            {
                if (Directory.Exists(directoryPath))
                {
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(directoryPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"The process failed: {e}");
            }



            var fileName = $"{clinic}-{today}-{targetDaysOut}.csv";

            var filePath = $"{directoryPath}\\{fileName}";

            int day = 0;

            foreach (ProviderModel person in providerAssignedList)
            {
                day++; //iterate day day1=mon, day2=tues, day3=wed, day4=thur, day5=fri
                string newLine = person.Name;

                csv.AppendLine(newLine);

                if (day == 5)
                {
                    csv.AppendLine();
                    csv.AppendLine();

                    day = 0;
                }
            }

            File.WriteAllText(filePath, csv.ToString());

            return filePath;
        }

        public static string getUserInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public static void printIntro(string postCode, string clinicList)
        {
            //prints the intro for the BHA randomizer
            Console.WriteLine(postCode);
            Console.WriteLine();
            Console.WriteLine("---   BHA Hold Randomizer   ---");
            Console.Write("Clinics current live: ");
            Console.WriteLine(clinicList);
            Console.WriteLine("------------------------------");
            Console.WriteLine("Menu: ");
            Console.WriteLine("--- rand (BHA Randomizer Algorithm run report menu)");
            Console.WriteLine("--- update (update/view providerlist)");
            Console.Write(">>>");
        }
    }
}