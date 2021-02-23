using System;
using System.Collections.Generic;
using System.Text;

namespace IMAP.Shared
{
    [Flags]
    public enum SearchCriterias
    {
        ALL,
        ANSWERED,
        BCC,
        BEFORE,
        BODY, 
        CC, 
        DELETED, 
        DRAFT,
        FLAGGED, 
        FROM, 
        HEADER, 
        KEYWORD,
        LARGER,
        NEW,
        NOT, 
        OLD,
        ON,
        OR,
        RECENT,
        SEEN, 
        SENTBEFORE,
        SENTON,
        SENTSINCE,
        SINCE, 
        SMALLER,
        SUBJECT,
        TEXT,
        TO,
        UID,
        UNANSWERED,
        UNDELETED,
        UNDRAFT,
        UNFLAGGED,
        UNKEYWORD,
        UNSEEN
    }
}
