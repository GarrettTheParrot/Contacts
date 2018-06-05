using System;
using System.Text.RegularExpressions;

namespace Lists
{
    /// <summary>
    /// Methods used to validate data against
    /// </summary>
    public class Validation
    {
        #region ctors
        public Validation()
        {
        }
        #endregion ctors

        #region validation methods
        /// <summary>
        /// Returns true, if string converts to a valid integer value.
        /// </summary>
        /// <returns><c>true</c> if is numeric the specified val result; otherwise, <c>false</c>.</returns>
        /// <param name="val">Value.</param>
        /// <param name="result">Result.</param>
        public static bool IsNumeric(string val, ref int result)
        {
            int outVal = 0;

            // Try converting to an interger...
            if (int.TryParse(val, out outVal))
            {
                // Success!
                result = outVal;
                return true;
            }
            else
                //Conversion failed.
                return false;

        }

        /// <summary>
        /// Determines if the value is a valid US phone number.
        /// </summary>
        /// <param name="val">Value.</param>
        public static bool IsValidUSPhoneNumber(string val)
        {
            // Using the Regular Expression below, 
            // these are the valid phone number formats:  
            // 0123456789, 
            // 012-345-6789, 
            // (012)-345-6789, 
            // (012)3456789 
            // 012 3456789, 
            // 012 345 6789, 
            // 012 345-6789, 
            // (012) 345-6789, 
            // 012.345.6789

            // To learn more about regular expressions, 
            // go here: https://msdn.microsoft.com/en-us/library/az24scfc.aspx 

            Regex rg = new Regex(@"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
            return rg.IsMatch(val);

        }

        /// <summary>
        /// Determines if the value is a valid email address.
        /// </summary>
        /// <param name="val">Value.</param>
        public static bool IsValidEmail(string val)
        {

            // To learn more about regular expressions, 
            // and how it works for email specifically, go here: 
            //     https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx

            // Return true if val is in valid e-mail format.
            try {
                return Regex.IsMatch(val,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }

        }

        /// <summary>
        /// Determines if a value is a valid URL
        /// </summary>
        /// <param name="val">Value.</param>
        public static bool IsValidUrl(string val)
        {

            return Uri.IsWellFormedUriString(val, UriKind.RelativeOrAbsolute);
        }   

        #endregion validation methods
    }
}

