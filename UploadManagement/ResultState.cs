using System.Collections.Generic;
using Utilities;

namespace UploadManagement
{

    public class ResultState
    {
        public string Status { get; set; }
        public SortedList<string,string> error {get; set;}
        public ResultState()
        {
            error = new SortedList<string, string>();
        }
    }
}