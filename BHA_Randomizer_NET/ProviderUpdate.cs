using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace BHA_Randomizer_NET
{
    public class ProviderUpdate
    {
        public static string updateMenu(List<ProviderModel> providerList, List<string> listOfClinics, string clinicsString)
        {
            Console.Clear();


            string userInput = " ";
            string postCode = "";

            while (!userInput.Equals("q") && !userInput.Equals("quit"))
            {
                userInput = " ";
                Console.WriteLine(postCode);
                postCode = "";
                Console.WriteLine("------------------- Update Menu -------------------");
                Console.WriteLine("Choose a clinic to view providers and update");
                Console.WriteLine("Clinics: " + clinicsString);
                Console.Write(">>>");

                userInput = Console.ReadLine();

                if (listOfClinics.Contains(userInput))
                {
                    List<ProviderModel> clinicProviders = printClinicInfo(providerList, userInput);

                    // TODO: option to update specific ones such as 
                    // del
                    // mod num (fte, days, name, clinic)
                    Console.WriteLine("<command> <num> <type> <mew value> ex: mod 1 fte .8 will change the fte of provider number 1 to .8");
                    Console.WriteLine("<-h> for help and list of commands");
                    Console.Write(">>>");
                    //Console.WriteLine("ERROR: Provider update has not been implented yet, please modify provider_info.json file");
                    List<string> provUpdateInput = Console.ReadLine().Split(' ').ToList();

                    if (provUpdateInput[0].Equals("-h")) 
                    {
                        postCode = help();
                    } else if (provUpdateInput[0].Equals("mod"))
                    {

                    } else if (provUpdateInput[0].Equals("del"))
                    {
                        postCode = delProv(provUpdateInput, clinicProviders);
                    } else if (provUpdateInput[0].Equals("add"))
                    {

                    } else if (provUpdateInput[0].Equals("quit") || provUpdateInput[0].Equals("q")) 
                    {
                        return "Exited provider update early";
                    }
                    else 
                    {
                        postCode = "Error: did not understand input " + provUpdateInput.ToString();
                    }
                } else
                {
                    postCode = userInput + " is an unrecognized command, please select a clinic or q to quit\n";
                }
            }


            Console.Clear();
            return "Provider update complete";
        }

        private static string delProv(List<string> provUpdateInput, List<ProviderModel> clinicList)
        {
            try
            {
                ProviderModel providerDel = clinicList[int.Parse(provUpdateInput[1])];
                Console.WriteLine($"Deleting {providerDel.Name}? Please verify y/n");
                if (Console.ReadLine().Equals("y"))
                {
                    // TODO: Delete provider
                }
            } catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }

            return "";
        }

        private static string help()
        {
            Console.WriteLine("\nCommands:\ndel <num>\t(ex: 'del 1' will delete provider with num of 1");
            Console.WriteLine("mod <num> fte/days/name <new value>\t(ex: 'mod 1 fte .8' will change the provider with num 1 to .8 fte)");
            Console.WriteLine("'mod <num> days' will lead to a series of questions for each day\n");

            return "";
        }

        private static List<ProviderModel> printClinicInfo(List<ProviderModel> providerList, string clinic)
        {
            List<ProviderModel> clinicList = new List<ProviderModel>();

            Console.WriteLine(String.Format("| {0,-5} {1, -25} {2,7} {3,5} {4,5} {5,5} {6,5} {7,5} {8,5} |", "num", "provider name", "clinic", "fte", "mon", "tue", "wed", "thur", "fri"));
            Console.WriteLine("_____________________________________________________________________________");
            for (int i = 0; i < providerList.Count; i++)
            {
                if (providerList[i].clinic.Equals(clinic))
                {
                    clinicList.Add(providerList[i]);
                    //Create a workingDays List and assign the 0-4 values as 
                    List<string> workingDays = new List<string>();
                    if (providerList[i].Monday)
                    {
                        workingDays.Add(" \u221A ");
                    } else
                    {
                        workingDays.Add("   ");
                    }
                    if (providerList[i].Tuesday)
                    {
                        workingDays.Add(" \u221A ");
                    }
                    else
                    {
                        workingDays.Add("   ");
                    }
                    if (providerList[i].Wednesday)
                    {
                        workingDays.Add(" \u221A ");
                    }
                    else
                    {
                        workingDays.Add("   ");
                    }
                    if (providerList[i].Thursday)
                    {
                        workingDays.Add(" \u221A ");
                    }
                    else
                    {
                        workingDays.Add("   ");
                    }
                    if (providerList[i].Friday)
                    {
                        workingDays.Add(" \u221A ");
                    }
                    else
                    {
                        workingDays.Add("   ");
                    }

                    Console.WriteLine(String.Format("| {0,-5} {1, -25} {2,7} {3,5} {4,5} {5,5} {6,5} {7,5} {8,5} |", i, providerList[i].Name, providerList[i].clinic, providerList[i].Fte, workingDays[0], workingDays[1], workingDays[2], workingDays[3], workingDays[4]));
                    Console.WriteLine("_____________________________________________________________________________");

                }

            }
            return clinicList;

        }
    }

}
