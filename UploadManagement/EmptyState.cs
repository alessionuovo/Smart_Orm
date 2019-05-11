using System;
using Utilities.Definitions;

namespace UploadManagement
{
    /// <summary>
    /// Part of \ref State Design Pattern.
    /// Deletes all files from target upload directory.
    /// </summary>
    internal class EmptyState : UploadState
    {
        public EmptyState()
        {
            this.Result = new ResultState();
        }
        public override void Execute()
        {
            SystemDefinitionsManager.DefinitionsManager.EmptyConstUploadDirectory();
            Result.Status = "Success";
        }
    }
}