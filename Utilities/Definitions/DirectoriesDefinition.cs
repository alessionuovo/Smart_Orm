namespace Utilities.Definitions
{
    /** \ingroup Bll
     <summary>
    Directories configuration class in the system.
     Implemented as Singleton.
         Part of \ref  Facade 
     </summary>
    */
    public class DirectoriesDefinition
    {
        private DirectoriesDefinition() { }
        private static DirectoriesDefinition _ddefinition;
        public static DirectoriesDefinition DDefinition { get { if (_ddefinition == null) _ddefinition = new DirectoriesDefinition(); return _ddefinition; } }
        /// <summary>
        /// Const upload directory
        /// </summary>
        public string TEMP_DIR = "TEMP";
        /// <summary>
        /// Temp upload directory
        /// </summary>
        public string REAL_DIR = "REAL";
    }
}