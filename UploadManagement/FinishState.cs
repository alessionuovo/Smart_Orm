using System;

namespace UploadManagement
{
    /** \ingroup Bll
     <summary>
     Finish State-no job all ended successfully.
        Part of \ref State Design Pattern
     </summary>
    */
    public class FinishState : UploadState
    {
        public FinishState()
        {
            
            this.Result = new ResultState();
            Result.Status = "Success";
        }
        public override void Execute()
        {
          
        }
    }
}