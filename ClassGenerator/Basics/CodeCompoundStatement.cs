using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    //This class represents a statement that can contain other statements
   public class CodeCompoundStatement:CodeStatement
    {
        public List<CodeStatement> Statements { get; set; } // list of statements
        public CodeCompoundStatement():base()
        {
            Statements = new List<CodeStatement>();
        }
    }
}
