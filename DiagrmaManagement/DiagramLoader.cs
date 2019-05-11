using FileManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Definitions;
/// <summary>
/// This namespace manages the changes that occur in diagram, 
/// by saving them in memory and by managing their permanent save in files.
/// This is done via \ref Command
/// </summary>
namespace DiagrmaManagement
{
    //This class manages load of existing diagram
   public class DiagramLoader
    {
        string format="xml";
        /// <summary>
        /// Result from checking for diagram files in directory
        /// or no files, or files are ok or files that cannot represent database objects
        /// </summary>
        public enum DiagramResult { HAS_FILES, NO_FILES, ONLY_MALFORMED_FILES}
        /// <summary>
        /// Tries to load diagram
        /// so to create object of classes bound to diagram control.
        /// </summary>
        /// <returns>Result of loading</returns>
        public DiagramResult LoadDiagram()
        {
            
            List<string> files = SystemDefinitionsManager.DefinitionsManager.GetDiagramFileNames("xml");
            if (files.Count == 0)
                return DiagramResult.NO_FILES;
            bool overallresult = false;
            foreach (string file in files)
            {

                bool result=DbDefinitionsManager.DbManager.CreateDiagram(file, format);
                overallresult = result || overallresult;
            }
            if (overallresult)
             return DiagramResult.HAS_FILES;
            return DiagramResult.ONLY_MALFORMED_FILES;
        }
    }
}
