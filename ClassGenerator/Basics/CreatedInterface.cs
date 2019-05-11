using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
     /** \ingroup Bll
     <summary>
     Represents a generate c# interface. Part of \ref Builder 
     </summary>
    */
   public class CreatedInterface:CreatedType
    {
        public CreatedInterface(string directory, string name):base(directory,name)
        {
            this.directory = directory;
            this.name = name;
            TheType = new CodeTypeDeclaration(name);
            TheType.IsInterface = true;
        }
        /// <summary>
        /// Created declaration of event in interface and adds it to the type
        /// </summary>
        /// <param name="name">Name of event</param>
        /// <param name="type">Type of event</param>
        public void BuildEvent(string name, string type)
        {
            CodeSnippetTypeMember EventDeclarationSnippet = new CodeSnippetTypeMember(string.Format("event {0} {1};", type, name));
            TheType.Members.Add(EventDeclarationSnippet);
        }
       
        /// <summary>
        /// Creates method declaration in interface
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="lstParameters"></param>
        /// <returns></returns>
        public CodeMemberMethod BuildMethodSignature(string methodName, List<KeyValuePair<string, string>> lstParameters)
        {
            CodeMemberMethod cmmMethod = new CodeMemberMethod();
            foreach (KeyValuePair<string, string> kvpParameter in lstParameters)
            {
                cmmMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(kvpParameter.Key), kvpParameter.Value));
            }
            //cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("EventArgs"), "e"));
            cmmMethod.Name = methodName;

            return cmmMethod;
        }

        /// <summary>
        /// Create property declaration in interface 
        /// and adds it
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of property</param>
        internal void BuildProperty(string name, string type)
        {
            CodeSnippetTypeMember cstm = new CodeSnippetTypeMember(string.Format("{0} {1} {{get; set;}}", type, name));
            TheType.Members.Add(cstm);
        }
        public CodeMemberMethod BuildMethodSignature(string methodName, List<KeyValuePair<string, string>> lstParameters, string returnType)
        {
            CodeMemberMethod cmmMethod = new CodeMemberMethod();
            foreach (KeyValuePair<string, string> kvpParameter in lstParameters)
            {
                cmmMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(kvpParameter.Key), kvpParameter.Value));
            }
            //cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("EventArgs"), "e"));
            cmmMethod.Name = methodName;
            cmmMethod.ReturnType = new CodeTypeReference(returnType);
            return cmmMethod;
        }
    }
}
