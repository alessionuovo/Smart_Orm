using System;

namespace Utilities
{
    /** \ingroup Bll
     <summary>
     Inherits exception
     Needed to store friendly message to user additionaly to Exception
     </summary>
    */
    public class Result:Exception
    {
        public string DisplayMessage { get; set; }
        public Result(string message, string friendlymessage):base(message)
        {
            DisplayMessage = friendlymessage;
        }
    }
}