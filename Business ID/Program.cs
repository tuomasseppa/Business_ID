using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Business_ID
{
    class Program
    {
        static void Main(string[] args)
        {
            bool programRunning = true;

            BusinessidSpecification<string> businessIdCheck = new BusinessidSpecification<string>();

            while (programRunning)
            {
                Console.WriteLine("\nWrite 'test' to run the test set. Write 'exit' to exit.\nEnter Business ID:");

                string businessID = Console.ReadLine();

                if (businessID.ToLower() == "test")
                {
                    // Runs pre-set tests.
                    checkTests(businessIdCheck);
                }
                else if (businessID.ToLower() == "exit")
                {
                    programRunning = false;
                }
                else if (businessID.Length != 0)
                {
                    // Checks given input.
                    checkGivenID(businessID, businessIdCheck);
                }
                
            }
        }

        // Checks if the given Business ID is valid or invalid.
        private static bool checkGivenID(string businessID, BusinessidSpecification<string> checkID)
        {
            if (checkID.IsSatisfiedBy(businessID))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The Business ID is valid.");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The Business ID is invalid!\nThe reasons for invalidity: ");
                Console.ResetColor();

                // Loops the reason for Business ID to be invalid.
                foreach (string reason in checkID.ReasonsForDissatisfaction)
                {
                    Console.WriteLine(reason.ToString());
                }
                return false;
            }
        }

        // Set of different kind of tests.
        private static void checkTests(BusinessidSpecification<string> checker)
        {
            Dictionary<string, bool> businessIdTests = new Dictionary<string, bool>();

            // Business ID is too short.
            businessIdTests.Add("12", false);

            // Business ID is too long.
            businessIdTests.Add("3232324713849XX", false);

            // Business ID is too short and missing separator (-).
            businessIdTests.Add("24713849", false);

            // Business ID check number does not match.
            businessIdTests.Add("2471385-9", false);

            // Business ID check number does not match.
            businessIdTests.Add("2471384-8", false);

            // Business ID has characters.
            businessIdTests.Add("2471A84-9", false);

            // Business ID has characters.
            businessIdTests.Add("a4b1C8D-E", false);

            // Business ID with control mark one.
            businessIdTests.Add("2471384-1", false);

            // Business ID with control number result one.
            businessIdTests.Add("1001000-9", false);

            // Proper Business ID.
            businessIdTests.Add("2471384-9", true);

            // Proper Business ID with control mark zero.
            businessIdTests.Add("1572860-0", true);

            // Print out all the tests.
            foreach (var item in businessIdTests)
            {
                Console.Write("\n" + item.Key + ": ");

                if (checkGivenID(item.Key, checker) == item.Value)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Test passed.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Test failed.");
                    Console.ResetColor();

                }
            }
        }
    }
}

