using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared
{
    [Flags]
    public enum MessageAttributes
    {
        NONE = 0,
        ANNOTATIONS = 1,
        BODY = 2,
        BODYSTRUCTURE = 4,
        ENVELOPE = 8,
        FLAGS = 16,
        INTERNALDATE = 32,
        SIZE = 64,
        UID = 128,
        HEADERS = 256,
        ALL = 512, 
        FAST = 1024,
        TEXT = 2048,
        FULL = 4096
    }
}
