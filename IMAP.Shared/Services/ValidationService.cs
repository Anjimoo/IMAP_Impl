using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IMAP.Shared.Services
{
    public static class ValidationService
    {
        /// <summary>
        /// Validates that the user name is in a correct format.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool ValidateUsername(string username)
        {

            var match = Regex.Match(username, @"^(?=[a-zA-Z])[-\w.]{0,23}([a-zA-Z\d]|(?<![-.])_)$", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates that the password is in a correct format. To be used ONLY on the client side.
        /// </summary>
        /// <returns></returns>
        public static bool ValidatePassword(string password)
        {
            /***********************************/
            //TODO: Finish writing this function.
            /***********************************/

            return true;
        }

        /// <summary>
        /// Validates that the Mailbox name is in a correct format.
        /// </summary>
        /// <returns></returns>
        public static bool ValidateMailboxName(string mailbox)
        {
            /***********************************/
            //TODO: Finish writing this function.
            /***********************************/

            return true;
        }
    }
}
