using System;
using System.Collections.Generic;
using System.Text;

namespace WanderLust.Common
{
   public class Helper
    {

        public static string GenerateRandomPassword()
        {
            char[] _password = new char[Constants.PASSWORD_LENGTH];
            string charSet = ""; // Initialise to blank
            System.Random _random = new Random();
            int counter;
            bool useLowerCase = true, useUpperCase = true, userNumbers = true, useSpecial = true;
            // Build up the character set to choose from
            if (useLowerCase) charSet += Constants.LOWER_CASE;
            if (useUpperCase) charSet += Constants.UPPER_CAES;
            if (userNumbers) charSet += Constants.NUMBERS;
            if (useSpecial) charSet += Constants.SPECIALS;

            for (counter = 0; counter < Constants.PASSWORD_LENGTH; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);

        }

       
    }
}
