using System.CodeDom;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    This class represents a class ConnectionProperty(inherits Property class)
    that is representation of foreign key and relationship.
       Also part of \ref Builder 
    Inherits CreatedClass
    </summary>
   */
    public class ConnectionPropertyClass : CreatedClass
    {
        public ConnectionPropertyClass(string directory, string name) : base(directory, name)
        {
        }

        protected override void BuildDefaultConstructorBody()
        {
            base.BuildDefaultConstructorBody();

        }
        /// <summary>
        /// The class has constructor with parameter propertyValue
        /// which calls the constructor of the base with the value
        /// </summary>
        public override void BuildOtherConstructors()
        {
            CodeConstructor codeConstructor = new CodeConstructor();
            codeConstructor.Attributes = MemberAttributes.Public;
            codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "propertyValue"));
            codeConstructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("propertyValue"));
            TheType.Members.Add(codeConstructor);
        }
    }
}