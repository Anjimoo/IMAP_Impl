using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_Client.Services
{
    public static class TaggingService
    {
        private static int tagNumber = 0; //Initial tag number.

        private static string _Tag = "A0000"; //Initial tag.

        public static string Tag
        {
            get
            {
                LastTag = _Tag;
                NextTag();
                return _Tag;
            }
            private set
            {
                _Tag = "A" + value;
            }
        }

        public static string LastTag {get;set;}

        private static void NextTag()
        {
            tagNumber++;
            Tag = tagNumber.ToString().PadLeft(4, '0');
        }

    }
}
