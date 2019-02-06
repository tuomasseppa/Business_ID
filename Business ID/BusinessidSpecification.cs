using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Business_ID
{
    // BusinessidSpecification implements interface ISpecification.
    // Business ID consists of seven digits, a dash and a control mark, for example 1234567-8.
    // Class checks if Business ID is valid and reports reasons if it is invalid.
    public class BusinessidSpecification<TEntity> : ISpecification<TEntity>
    {
       
        private List<string> reasons = new List<string>();

        // Reads the reasons that cause the Business ID dissatisfactions.
        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get
            {
                return reasons;
            }
        }

        // Checks the conditions for satisfaction. Returns a boolean.
        public bool IsSatisfiedBy(TEntity entity)
        {
            string entityString = null;

            // Delete items from the list.
            reasons.Clear();

            if (checkType(entity))
            {
                // Change input to string.
                entityString = entity.ToString();

                // Checks if string meets the requirements for valid Business ID.
                checkMinLength(entityString);
                checkMaxLength(entityString);
                checkSeparator(entityString);
                checkCharacters(entityString);

                // No reason to run checkControlMark if requirements above aren't met.
                if (reasons.Count == 0)
                {
                    checkControlMark(entityString);
                }
            }

            // If Business ID is valid, return true.
            return (reasons.Count == 0);
        }
        private bool checkType(TEntity entity)
        {
            if (entity.GetType() != typeof(string))
            {
                reasons.Add("Invalid format - input isn't a string.");
                return false;
            }
            else
            {
                return true;
            }
        }

        // Check string length is minimum of 9 characters.
        private bool checkMinLength(string businessID)
        {
            bool retVal = true;

            if (businessID.Length < 9)
            {
                reasons.Add("Given string is too short.");
                retVal = false;
            }

            return retVal;
        }

        // Check string length is maximum of 9 characters.
        private bool checkMaxLength(string businessID)
        {
            bool retVal = true;

            if (businessID.Length > 9)
            {
                reasons.Add("Given string is too long.");
                retVal = false;
            }

            return retVal;
        }

        // Check that character 7 is a separator line (-).
        private bool checkSeparator(string businessID)
        {
            bool retVal = true;

            if (businessID.Length < 8 || businessID[7] != '-')
            {
                reasons.Add("Invalid separator character (-).");
                retVal = false;
            }

            return retVal;
        }

        // Check that characters 0-6 and 8 are numbers.
        private bool checkCharacters(string businessID)
        {
            bool retVal = true;

            for (int character = 0; character < businessID.Length; character++)
            {
                if (character != 7)
                {
                    try
                    {
                        int.Parse(businessID.Substring(character, 1));
                    }
                    catch (FormatException)
                    {
                        reasons.Add("Invalid character: " + businessID.Substring(character, 1));
                        retVal = false;
                    }
                }
            }

            return retVal;
        }

        // Checking Business ID control mark:
        // Business ID numbers are each multiplied by the number in the same column: 7, 9, 10, 5, 8, 4 and 2. (Tunnuksen numeroita painotetaan vasemmalta lähtien kertoimilla).
        // Add products together. (Tulot lasketaan yhteen).
        // The sum is divided by 11. (Summa jaetaan 11:llä). 
        // If the remainder is zero, control number is zero. (Jos jakojäännös on 0, tarkistusnumero on 0).
        // If remainder is one, it's invalid Business ID. (Ei anneta tunnuksia, jotka tuottaisivat jakojäännöksen 1).
        // If remainder is 2..10, check number is 11 minus remainder. (Jos jakojäännös on 2..10, tarkistusnumero on 11 miinus jakojäännös).

        // Example 1: 1572860-0
        // 1  5  7  2  8  6  0  - 0
        // 7  9 10  5  8  4  2
        // 7 45 70 10 64 24  0  = 220 ≡ 0 (mod 11) → 0

        //Example 2: 0737546-2
        // 0  7  3  7  5  4  6  - 2
        // 7  9 10  5  8  4  2
        // 0 63 30 35 40 16 12  = 196 ≡ 9 (mod 11) → 2
        private bool checkControlMark(string businessID)
        {
            bool retVal = true;
            int totalAmount = 0;

            // Loops throught all the numbers and multiply the paired columns.
            for (int column = 0; column < 7; column++)
            {
                int columnNumber = int.Parse(businessID.Substring(column, 1));

                switch (column)
                {
                    case 0:
                        totalAmount += 7 * columnNumber;
                        break;
                    case 1:
                        totalAmount += 9 * columnNumber;
                        break;
                    case 2:
                        totalAmount += 10 * columnNumber;
                        break;
                    case 3:
                        totalAmount += 5 * columnNumber;
                        break;
                    case 4:
                        totalAmount += 8 * columnNumber;
                        break;
                    case 5:
                        totalAmount += 4 * columnNumber;
                        break;
                    case 6:
                        totalAmount += 2 * columnNumber;
                        break;
                }
            }

            // Control number is mod 11 from totalAmount.
            int controlNumber = totalAmount % 11;

            // Reminder one is invalid.
            if (controlNumber == 1)
            {
                reasons.Add("Error in control mark - check number result is one.");
                retVal = false;
            }
            // Control number is 11 minus the remainder.
            else if (controlNumber > 1)
            {
                controlNumber = 11 - controlNumber;
            }
            // If control number isn't the same as Business ID last number(check mark) it's mismatch.
            if (controlNumber != int.Parse(businessID.Substring(8, 1)))
            {
                reasons.Add("Check number does not match with control mark (mismatch).");
                retVal = false;
            }

            return retVal;
        }

    }
}
