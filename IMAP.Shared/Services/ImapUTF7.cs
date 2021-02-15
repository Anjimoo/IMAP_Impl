using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IMAP.Shared.Services
{
    /*
     * Implementation is based upon https://github.com/jstedfast/MailKit/blob/master/MailKit/Net/Imap/ 
     * since it's the most complete solution for encoding UTF7 to match IMAP requirements.
    */
    public static class ImapUTF7
    {

        public static string Encode(string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder nonAscii = new StringBuilder();
            char ch;
            for (int i = 0; i < str.Length; i++)
            {
                ch = str[i];
                if (ch < 0x7f && ch > 0x1f)
                {
                    if (nonAscii.Length > 0)
                    {
                        byte[] encodedAscii = Encoding.BigEndianUnicode.GetBytes(nonAscii.ToString());
                        string encodedAsciiStr = Convert.ToBase64String(encodedAscii);
                        encodedAsciiStr = encodedAsciiStr.Replace('/', ',').TrimEnd('=');
                        stringBuilder.Append("&" + encodedAsciiStr + "-");
                        nonAscii.Clear();
                    }
                    if (ch == 0x26)
                        stringBuilder.Append("&-");
                    else
                        stringBuilder.Append(ch);
                }
                else
                {
                    nonAscii.Append(ch);
                }
            }
            if (nonAscii.Length > 0)
            {
                byte[] encodedAscii = Encoding.BigEndianUnicode.GetBytes(nonAscii.ToString());
                string encodedAsciiStr = Convert.ToBase64String(encodedAscii);
                encodedAsciiStr = encodedAsciiStr.Replace('/', ',').TrimEnd('=');
                stringBuilder.Append("&" + encodedAsciiStr + "-");
            }
            return stringBuilder.ToString();

        }

    }
}
