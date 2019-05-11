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
     This class is basic class for all generated types. Part of \ref Builder 
     Inherits CodeCompileUnit
     </summary>
    */
    public class CreatedType:CodeCompileUnit
    {
        protected string directory;
        protected string name;

        public CreatedType(string directory, string name)
        {
            this.directory = directory;
            this.name = name;
        }

        /// <summary>
        /// Type that was generated
        /// </summary>
        public CodeTypeDeclaration TheType { get; set; }
        public virtual void BuildMethods()
        {

        }

        /// <summary>
        /// Adds using directive in the beginning of created type
        /// </summary>
        /// <param name="importLibrary">Library to import</param>
        public void AddImport(string importLibrary)
        {
            CodeNamespace cn = new CodeNamespace();
            cn.Imports.Add(new CodeNamespaceImport(importLibrary));
            this.Namespaces.Add(cn);
        }
        public virtual void BuildPropertiesAndFields()
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>name of the generated type</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Creates generic type with possible constraints
        /// </summary>
        /// <param name="lstGenericClassDefinitions">list of GenericClassParameters-struct with generic type name and constraints</param>
        public void CreateGenericType(List<GenericClassParameters> lstGenericClassDefinitions)
        {
            List<string> lstTypeParams = new List<string>();
            foreach (GenericClassParameters GenericClassDefinition in lstGenericClassDefinitions)
            {

                CodeTypeReference ctrGenericTypeReference = new CodeTypeReference(GenericClassDefinition.GenericType);
                CodeTypeReference ctrBaseTypeReference;
                if (!string.IsNullOrEmpty(GenericClassDefinition.BaseType))
                {
                    if (GenericClassDefinition.IsBaseTypeGeneric)
                    {
                        ctrBaseTypeReference = new CodeTypeReference(GenericClassDefinition.BaseType, ctrGenericTypeReference);
                    }
                    else ctrBaseTypeReference = new CodeTypeReference(GenericClassDefinition.BaseType);
                    TheType.BaseTypes.Add(ctrBaseTypeReference);
                }

                CodeTypeParameter ctpGenericTypeParameter = new CodeTypeParameter(GenericClassDefinition.GenericType);
              
               
                    if (GenericClassDefinition.Constraint.Count > 0)
                    {
                        foreach (string constraint in GenericClassDefinition.Constraint)
                        {
                            CodeTypeReference ctrGenericConstraintReference = new CodeTypeReference(constraint);
                            ctpGenericTypeParameter.Constraints.Add(ctrGenericConstraintReference);
                        }
                    }
                    if (!lstTypeParams.Contains(GenericClassDefinition.GenericType))
                        TheType.TypeParameters.Add(ctpGenericTypeParameter);
                   
                
                lstTypeParams.Add(GenericClassDefinition.GenericType);
            }
        }

        public void CreateGenericTypeMultiDimension(List<GenericClassParameters> lstGenericClassDefinitions)
        {
            List<string> lstTypeParams = new List<string>();
            foreach (GenericClassParameters GenericClassDefinition in lstGenericClassDefinitions)
            {
                CodeTypeReference ctrBaseTypeReference = new CodeTypeReference(GenericClassDefinition.BaseType);

                foreach (string genType in GenericClassDefinition.GenericTypes)
                {
                    CodeTypeReference ctrGenericTypeReference = new CodeTypeReference(genType);





                    if (GenericClassDefinition.IsBaseTypeGeneric && ctrBaseTypeReference != null)
                    {
                        ctrBaseTypeReference.TypeArguments.Add(ctrGenericTypeReference);
                    }
                    CodeTypeParameter ctpGenericTypeParameter = new CodeTypeParameter(genType);


                    if (GenericClassDefinition.Constraint.Count > 0)
                    {
                        foreach (string constraint in GenericClassDefinition.Constraint)
                        {
                            CodeTypeReference ctrGenericConstraintReference = new CodeTypeReference(constraint);
                            ctpGenericTypeParameter.Constraints.Add(ctrGenericConstraintReference);
                        }
                    }
                  //  if (!lstTypeParams.Contains(genType))
                    //    TheType.TypeParameters.Add(ctpGenericTypeParameter);




                    lstTypeParams.Add(GenericClassDefinition.GenericType);
                }
                TheType.BaseTypes.Add(ctrBaseTypeReference);
            }
            
        
        }
        public virtual void CreateAutomaticProperty(string name, string type)
        {
            CodeSnippetTypeMember PropertySnippet = new CodeSnippetTypeMember();
            PropertySnippet.Comments.Add(new CodeCommentStatement("this is integer property", true));
            PropertySnippet.Text = string.Format(" {0} {1} {{ get; set; }}",  type, name);
            TheType.Members.Add(PropertySnippet);
        }
        public virtual void CreateAutomaticReadOnlyProperty(string name, string type)
        {
            CodeSnippetTypeMember PropertySnippet = new CodeSnippetTypeMember();
            PropertySnippet.Comments.Add(new CodeCommentStatement("this is integer property", true));
            PropertySnippet.Text = string.Format(" {0} {1} {{ get;  }}", type, name);
            TheType.Members.Add(PropertySnippet);
        }
        public CodeMemberMethod BuildMethodSignature(string v1, List<KeyValuePair<string, string>> list, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
