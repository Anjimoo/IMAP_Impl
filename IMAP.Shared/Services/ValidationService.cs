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

        public static bool ValidateUsername(string username)
        {

            var match = Regex.Match(username, @"^(?=[a-zA-Z])[-\w.]{0,23}([a-zA-Z\d]|(?<![-.])_)$", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return true;
            }

            return false;
        }

    }
}
