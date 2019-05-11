using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    internal struct MemberFieldDefinition
    {
        public string MemberName { get; set; }

    }
    /** \ingroup Bll
    <summary>
    Represents generate type that is class. Part of \ref Builder 
    </summary>
        */
   public class CreatedClass:CreatedType
   {
      
       protected CodeConstructor defaultConstructor;
       
       public CreatedClass(string directory, string name, string type):base(directory,name)
       {
           this.directory = directory;
           this.name = name;
           TheType = new CodeTypeDeclaration(name);
           init(type);
       }

       private void init(string type)
       {
           TheType = new CodeTypeDeclaration(name);
           TheType.IsClass = true;
       }

        

        public CreatedClass(string directory, string name):base(directory,name)
       {
          
           
           init("class");
       }

       /// <summary>
       /// This method may be overriden in classes that extend this class
       /// in order to provide constructors with parameters
       /// </summary>
       public virtual void BuildOtherConstructors()
       {
          
       }

       

       
       /// <summary>
       /// Creates a constructor without parameters
       /// </summary>
       public void BuildDefaultConstructor()
       {
           defaultConstructor = new CodeConstructor();
           defaultConstructor.Attributes = MemberAttributes.Public;
           BuildDefaultConstructorBody();
           // Adds the constructor to the Members collection of the BaseType.
           this.TheType.Members.Add(defaultConstructor);
       }

       /// <summary>
       /// This method is used to create methods that serve as event handlers
       /// </summary>
       public virtual void BuildEventHandlers()
       {

       }

      /// <summary>
      /// In order to work with default contructor only override this function 
      /// </summary>
       protected virtual void BuildDefaultConstructorBody()
       {

       }

       

       #region Utility methods
       /// <summary>
       /// Creates event invokation structure like this:
       /// <code>
       /// if (eventDelegate!=null)
       ///   eventDelegate(a, args);
       /// </code>
       /// and the EventArgs param has default constructor
       /// </summary>
       /// <param name="eventPropertyName">Name of the delegate</param>
       /// <param name="eventArgs">Type of EventArgs argument(EventArgs or one of his descendants)</param>
       /// <returns>Statement that represents check that eventDelegate in defined and invokes it </returns>
       public CodeStatement GetEventHandlerInvokation(string eventPropertyName,  string eventArgs)
       {

           CodeExpression notNullConditionExpression = GetConditionCompareToNull(eventPropertyName);
           CodeExpressionStatement invokeEventStatement = GetInvokationStatement(eventPropertyName, eventArgs);
           CodeStatement[] trueStatements = {
               invokeEventStatement
       };
           CodeStatement[] falseStatements = { new CodeCommentStatement("Do this is false") };
           CodeStatement InvokeDefinedDelegateStatement = new CodeConditionStatement(notNullConditionExpression, trueStatements);
           return InvokeDefinedDelegateStatement;
       }

       /// <summary>
       /// Creates event invokation structure like this:
       /// <code>
       /// if (eventDelegate!=null)
       ///   eventDelegate(a, args);
       /// </code>
       /// and the EventArgs param has default constructor
       /// </summary>
       /// <param name="eventPropertyName">Name of the delegate</param>
       /// <param name="eventArgs">Type of EventArgs argument(EventArgs or one of his descendants)</param>
         /// <param name="list">List of string that represents parameters of eventArgs constructor arguments</param>
           /// <returns>Statement that represents check that eventDelegate in defined and invokes it </returns>
       protected CodeStatement GetEventHandlerInvokation(string eventPropertyName, string eventArgs, List<string> list)
       {
           CodeExpression ce = GetConditionCompareToNull(eventPropertyName);
           CodeExpressionStatement expressionStatement = GetInvokationStatement(eventPropertyName, eventArgs, list);
           CodeStatement[] trueStatements = {
               expressionStatement
       };
           CodeStatement[] falseStatements = { new CodeCommentStatement("Do this is false") };
           CodeStatement cer = new CodeConditionStatement(ce, trueStatements);
           return cer;
       }

     
       /// <summary>
       /// Create this c# structure:
       /// property!=null
       /// </summary>
       /// <param name="propertyName">Name of property to compare</param>
       /// <returns>Expression that represents compare of element to null(in equality comparison)</returns>
       public  CodeExpression GetConditionCompareToNull(string propertyName)
       {
           CodeExpression ceLeft = new CodeVariableReferenceExpression(propertyName);
           CodeExpression ceRight = new CodePrimitiveExpression(null);
           CodeExpression ConditionExpression = new CodeBinaryOperatorExpression(ceLeft, CodeBinaryOperatorType.IdentityInequality, ceRight);
           return ConditionExpression;
       }

       /// <summary>
       /// Creates if that checks one condition and has only the true part:
       /// Example:
       /// <code>
       ///  if (variable=="5")
       ///  {
       ///    Some stuff to do
       ///  }
       /// </code>
       /// </summary>
       /// <param name="variableName">Name of varible to check</param>
       /// <param name="type">Type of comparison</param>
       /// <param name="comparisonValue">To what string value to compare</param>
       /// <param name="trueStatements">what to do if the expression is true</param>
       /// <returns>Statement that represents if with one condition and has only the true part</returns>
       public CodeStatement GetUnoConditionIf(string variableName, CodeBinaryOperatorType type, string comparisonValue, List<CodeStatement> trueStatements)
       {
           CodeExpression ceLeft = new CodeVariableReferenceExpression(variableName);
           CodeExpression ceRight;
           if (comparisonValue=="null")
           {
               ceRight = new CodePrimitiveExpression(null);
           }
           else if (comparisonValue!="true" && comparisonValue!="false")

              ceRight = new CodeVariableReferenceExpression(comparisonValue);
           else 
           {
               bool boolComparisonValue;
               bool.TryParse(comparisonValue, out boolComparisonValue);
               ceRight = new CodePrimitiveExpression(boolComparisonValue);


           }
           CodeExpression ConditionExpression = new CodeBinaryOperatorExpression(ceLeft, type, ceRight);
           CodeStatement IfStatement = new CodeConditionStatement(ConditionExpression, trueStatements.ToArray());
           return IfStatement;
       }

       /// <summary>
       /// Creates if that checks one condition and has only the true part:
       /// Example:
       /// <code>
       ///  if (variable=="5")
       ///  {
       ///    Some stuff to do
       ///  }
       /// </code>
       /// </summary>
       /// <param name="variableName">Name of varible to check</param>
       /// <param name="type">Type of comparison</param>
       /// <param name="comparisonValue">To what string value to compare</param>
       /// <param name="IsVariable">Boolean- if it is true it says that you compare to variable if false comparison is to string
       /// (Compare this two options: a==b and a=="b")
       /// </param>
       /// <param name="trueStatements">what to do if the expression is true</param>
       /// <returns>Statement that represents if with one condition and has only the true part</returns>
       public CodeStatement GetUnoConditionIf(string variableName, CodeBinaryOperatorType type, string comparisonValue, bool IsVariable, List<CodeStatement> trueStatements)
       {
           CodeExpression ceLeft = new CodeVariableReferenceExpression(variableName);

           CodeExpression ceRight;
           if (comparisonValue != "true" && comparisonValue != "false")
           {
               if (IsVariable)
                   ceRight = new CodeVariableReferenceExpression(comparisonValue);
               else ceRight = new CodePrimitiveExpression(comparisonValue);
           }
           else
           {
               bool booleanValue;
               bool.TryParse(comparisonValue, out booleanValue);
               ceRight = new CodePrimitiveExpression(booleanValue);


           }
               CodeExpression ConditionExpression = new CodeBinaryOperatorExpression(ceLeft, type, ceRight);
           CodeStatement IfStatement = new CodeConditionStatement(ConditionExpression, trueStatements.ToArray());
           return IfStatement;
       }

       /// <summary>
       /// Creates if that checks one condition and has true and false parts:
       /// Example:
       /// <code>
       ///  if (variable=="5")
       ///  {
       ///    Some stuff to do
       ///  }
       ///  else 
       ///  {
       ///   Some stuff to do
       ///  }
       /// </code>
       /// </summary>
       /// <param name="variableName">Name of varible to check</param>
       /// <param name="type">Type of comparison</param>
       /// <param name="comparisonValue">To what string value to compare</param>
      
       /// <param name="trueStatements">what to do if the expression is true</param>
       /// <returns>Statement that represents if with one condition and has only the true part</returns>
       public CodeStatement GetUnoConditionIf(string variableName, CodeBinaryOperatorType type, string comparisonValue, List<CodeStatement> trueStatements, List<CodeStatement> falseStatements)
       {
           CodeExpression ceLeft = new CodeVariableReferenceExpression(variableName);
           CodeExpression ceRight;
           if (comparisonValue != "null")
               ceRight = new CodeVariableReferenceExpression(comparisonValue);
           else ceRight = new CodePrimitiveExpression(null);
           CodeExpression ConditionExpression = new CodeBinaryOperatorExpression(ceLeft, type, ceRight);
           CodeStatement IfStatement = new CodeConditionStatement(ConditionExpression, trueStatements.ToArray(), falseStatements.ToArray());
           return IfStatement;
       }

       /// <summary>
       /// Creates if that checks one condition and has true and false parts:
       /// Example:
       /// <code>
       ///  if (variable=="5")
       ///  {
       ///    Some stuff to do
       ///  }
       ///  else 
       ///  {
       ///   Some stuff to do
       ///  }
       /// </code>
       /// </summary>
       /// <param name="variableName">Name of varible to check</param>
       /// <param name="type">Type of comparison</param>
       /// <param name="comparisonValue">To what string value to compare</param>
       /// <param name="IsVariable">Boolean- if it is true it says that you compare to variable if false comparison is to string
       /// (Compare this two options: a==b and a=="b")
       /// </param>
       /// <param name="trueStatements">what to do if the expression is true</param>
       /// <param name="falseStatements">what to do if the expression is false</param> 
       /// <returns>Statement that represents if with one condition and has only the true part</returns>
       public CodeStatement GetUnoConditionIf(string variableName, CodeBinaryOperatorType type, string comparisonValue, bool IsVariable, List<CodeStatement> trueStatements, List<CodeStatement> falseStatements)
       {
           CodeExpression ceLeft = new CodeVariableReferenceExpression(variableName);
           CodeExpression ceRight;
           if (IsVariable)
               ceRight = new CodeVariableReferenceExpression(comparisonValue);
           else ceRight = new CodePrimitiveExpression(comparisonValue);
           CodeExpression ConditionExpression = new CodeBinaryOperatorExpression(ceLeft, type, ceRight);
           CodeStatement IfStatement = new CodeConditionStatement(ConditionExpression, trueStatements.ToArray(), falseStatements.ToArray());
           return IfStatement;
       }

       /// <summary>
       /// Generate cast with assignment
       /// in this structure:
       /// a=(int)b;
       /// </summary>
       /// <param name="targetType">The type to which to cast</param>
       /// <param name="variableName">Name of variable to assign to</param>
       /// <param name="expression">Expression to assign</param>
       /// <returns>Statement that represents cast with assignment</returns>
       public CodeStatement GetCastWithAssignment(string targetType, string variableName,string expression )
       {
           CodeCastExpression castExpression = new CodeCastExpression(
             // targetType parameter indicating the target type of the cast.
           targetType,
           // The CodeExpression to cast, here an Int32 value of 1000.
          new CodeVariableReferenceExpression(expression));
           CodeStatement Assignment = GetAssignment(targetType, variableName, castExpression);
           return Assignment;
       }
        public CodeStatement GetCastWithAssignmentDeclare(string targetType, string variableName, string expression)
        {
            CodeCastExpression castExpression = new CodeCastExpression(
            // targetType parameter indicating the target type of the cast.
            targetType,
           // The CodeExpression to cast, here an Int32 value of 1000.
           new CodeVariableReferenceExpression(expression));
            CodeStatement Assignment = GetAssignmentWithExpression( variableName, castExpression);
            return Assignment;
        }
        /// <summary>
        /// Creates assignment
        /// </summary>
        /// <param name="targetType">Type of the variable to which to assign</param>
        /// <param name="variableName">Name of variable</param>
        /// <param name="castExpression">Expression to assign</param>
        /// <returns>Statement that represents assignment</returns>
        protected CodeStatement GetAssignment(string targetType, string variableName, CodeExpression castExpression)
       {

           return GetDeclareAndAssign(targetType, variableName, castExpression);
       }
       protected CodeStatement GetAssignment(string variableName, KeyValuePair<string,bool> value)
       {
           CodeAssignStatement stAssignment;
           if (value.Key != "true" && value.Key != "false")
           {
               if (value.Value)
                   stAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(variableName), new CodeVariableReferenceExpression(value.Key));
               else stAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(variableName), new CodePrimitiveExpression(value.Key));
           }
           else
           {
               bool b;
               bool.TryParse(value.Key, out b);
               stAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(variableName), new CodePrimitiveExpression(b));
           }
           return stAssignment;
       }
        protected CodeStatement GetAssignmentWithExpression(string variableName, CodeExpression value)
        {
            CodeAssignStatement stAssignment;
           
                stAssignment = new CodeAssignStatement(new CodeVariableReferenceExpression(variableName), value);
            
            return stAssignment;
        }
        /// <summary>
        /// Create declaration like this
        /// : int a;
        /// </summary>
        /// <param name="name">Name of variable to declare</param>
        /// <param name="type">Type to declare</param>
        /// <returns></returns>
        public CodeStatement GetDeclaration(string name, string type)
       {
           CodeVariableDeclarationStatement variableDeclaration = new CodeVariableDeclarationStatement(
   // Type of the variable to declare.
   type,
    // Name of the variable to declare.
    name);
           return variableDeclaration;
       }

       /// <summary>
       /// Declaration and assignment in one statement
       /// </summary>
       /// <param name="targetType">Type of variable</param>
       /// <param name="variableName">Name of variable</param>
       /// <param name="castExpression">Expression to assign</param>
       /// <returns></returns>
       private CodeStatement GetDeclareAndAssign(string targetType, string variableName, CodeExpression castExpression)
       {
          
           CodeVariableDeclarationStatement variableDeclaration = new CodeVariableDeclarationStatement(
    // Type of the variable to declare.
   targetType,
    // Name of the variable to declare.
    variableName,
    // Optional initExpression parameter initializes the variable.
    castExpression);
           return variableDeclaration;
       }

       /// <summary>
       /// Simple for loop
       /// </summary>
       /// <param name="forIndex">Nmae of index(common is i)</param>
       /// <param name="kvpValueRange">The key is smallest value of index, the value is the largest, the comparison is below largest</param>
       /// <param name="step">Step</param>
       /// <param name="statements">what to do in loop</param>
       /// <returns></returns>
       public CodeCompoundStatement GetSimpleForInteger(string forIndex, KeyValuePair<int,string> kvpValueRange, int step, List<CodeStatement> statements )
       {
           CodeCompoundStatement CompoundStatement = new CodeCompoundStatement();
           CodeVariableDeclarationStatement IndexDeclarationStatement = new CodeVariableDeclarationStatement(typeof(int), forIndex, new CodePrimitiveExpression(kvpValueRange.Key));
           CompoundStatement.Statements.Add(IndexDeclarationStatement);
           // Creates a for loop that sets testInt to 0 and continues incrementing testInt by 1 each loop until testInt is not less than 10.
           CodeIterationStatement forLoop = new CodeIterationStatement(
               // initStatement parameter for pre-loop initialization.
               new CodeAssignStatement(new CodeVariableReferenceExpression(forIndex), new CodePrimitiveExpression(kvpValueRange.Key)),
               // testExpression parameter to test for continuation condition.
               new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(forIndex),
                   CodeBinaryOperatorType.LessThan, new CodeVariableReferenceExpression(kvpValueRange.Value.ToString())),
               // incrementStatement parameter indicates statement to execute after each iteration.
               new CodeAssignStatement(new CodeVariableReferenceExpression(forIndex), new CodeBinaryOperatorExpression(
                   new CodeVariableReferenceExpression(forIndex), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(step))),
               // statements parameter contains the statements to execute during each interation of the loop.
               // Each loop iteration the value of the integer is output using the Console.WriteLine method.
              statements.ToArray());
           CompoundStatement.Statements.Add(forLoop);
           return CompoundStatement;
       }

       // Receives tw parameters Invokes delegate function 
       //
       private static CodeExpressionStatement GetInvokationStatement(string eventPropertyName, string eventArgs)
       {
           CodeDelegateInvokeExpression DelegateInvoke = new CodeDelegateInvokeExpression(new CodeEventReferenceExpression(new CodeThisReferenceExpression(), eventPropertyName),
                       new CodeExpression[] { new CodeThisReferenceExpression(), new CodeObjectCreateExpression(eventArgs) });
          
           CodeExpressionStatement invokationStatement;
           invokationStatement = new CodeExpressionStatement(DelegateInvoke);
           return invokationStatement;
       }


       private CodeExpressionStatement GetInvokationStatement(string eventPropertyName, string eventArgs, List<string> list)
       {
           List<CodeExpression> lst = new List<CodeExpression>();
           foreach(var elem in list)
           {
               lst.Add(new CodeVariableReferenceExpression(elem));
           }
           CodeDelegateInvokeExpression invoke1 = new CodeDelegateInvokeExpression(new CodeEventReferenceExpression(new CodeThisReferenceExpression(), eventPropertyName),
                      new CodeExpression[] { new CodeThisReferenceExpression(), new CodeObjectCreateExpression(eventArgs, lst.ToArray()) });
           CodeStatement css = new CodeStatement();
           CodeExpressionStatement expressionStatement;
           expressionStatement = new CodeExpressionStatement(invoke1);
           return expressionStatement;
       }

       // Variation of create field
       // receive name, type and permission-public, private, internal
       //Creates field and adds it to members of class
       /// <summary>
       ///  //Creates field and adds it to members of class
       /// </summary>
       /// <param name="name">Name of field</param>
       /// <param name="type">Type of field</param>
       /// <param name="permission">Permission</param>
       public void BuildField(string name, string type, MemberAttributes permission)
       {
           CodeMemberField CreatedField = new CodeMemberField();
           CreatedField.Attributes = permission;
           CreatedField.Name = name;
           CreatedField.Type = new CodeTypeReference(type);
           TheType.Members.Add(CreatedField);
       }
        public void BuildField(string name, string type, MemberAttributes permission, string init_val)
        {
            CodeMemberField CreatedField = new CodeMemberField();
            CreatedField.Attributes = permission;
            CreatedField.Name = name;
            CreatedField.Type = new CodeTypeReference(type);
            CreatedField.InitExpression = new CodeVariableReferenceExpression(init_val);
            TheType.Members.Add(CreatedField);
        }
        /// <summary>
        /// Creates event and add it to the class
        /// and uses default constructor of class that extends EventArgs
        /// (that is used)
        /// </summary>
        /// <param name="name">Name of Event</param>
        /// <param name="type">Type of Event</param>
        public void BuildEvent(string name, string type)
       {
           CodeMemberEvent CreatedEvent = new CodeMemberEvent();
           CreatedEvent.Name = name;

           CreatedEvent.Type = new CodeTypeReference(type);
           TheType.Members.Add(CreatedEvent);
       }

       /// <summary>
       /// Creates event and add it to the class
       /// and uses  default constructor  of class that extends EventArgs
       /// (that is used)
       /// </summary>
       /// <param name="name">Name of Event</param>
       /// <param name="type">Type of Event</param>
       /// <param name="attr">Permissions-public, protected, private, internal</param>
       public void BuildEvent(string name, string type, MemberAttributes attr)
       {
           CodeMemberEvent CreatedEvent = new CodeMemberEvent();
           CreatedEvent.Name = name;
           CreatedEvent.Attributes = attr;
           CreatedEvent.Type = new CodeTypeReference(type);
           TheType.Members.Add(CreatedEvent);
       }

       /* Creates event with public modifier explicit if needed
          Parameter specifyType - if true than make event public
          else default access modifier
       */
    public void BuildEvent(string name, string type, bool specifyType)
        {
            CodeMemberEvent CreatedEvent = new CodeMemberEvent();
            CreatedEvent.Name = name;
            if (specifyType)
                CreatedEvent.Attributes = MemberAttributes.Public;
            CreatedEvent.Type = new CodeTypeReference(type);
            TheType.Members.Add(CreatedEvent);
        }

        /// <summary>
        /// This method creates automatic property
        /// Example: <code>public int a {get; set;}</code>
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of property</param>
        public override void CreateAutomaticProperty(string name, string type)
        {
            CreateAutomaticProperty(name, type, "public");
        }

        /// <summary>
        /// This method creates automatic property with specified access modifier
        /// Example: <code>public int a {get; set;}</code>
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of property</param>
        /// <param name="permission">Permission-private, public , internal</param>
        public void CreateAutomaticProperty(string name, string type, string permission)
        {
            CodeSnippetTypeMember PropertySnippet = new CodeSnippetTypeMember();
            PropertySnippet.Comments.Add(new CodeCommentStatement("this is integer property", true));
            PropertySnippet.Text = string.Format("{0} {1} {2} {{ get; set; }}", permission, type, name);
            TheType.Members.Add(PropertySnippet);
        }

        public void CreateAutomaticProperty(string name, string type, string permission, string setpremission)
        {
            CodeSnippetTypeMember PropertySnippet = new CodeSnippetTypeMember();
            PropertySnippet.Comments.Add(new CodeCommentStatement("this is integer property", true));
            PropertySnippet.Text = string.Format("{0} {1} {2} {{ get; {3} set; }}", permission, type, name, setpremission);
            TheType.Members.Add(PropertySnippet);
        }


        /* Creates regular property
           Example: public int A {get {retrun a;} set {a=value;})
           private int a;
        */
        protected void BuildProperty(string propertyName, string propertyType, string fieldName)
        {
            CodeMemberField PropertyField = new CodeMemberField(propertyType, fieldName);
            CodeMemberProperty CreatedProperty = new CodeMemberProperty();
            CreatedProperty.Attributes = MemberAttributes.Public;
            CreatedProperty.Type = new CodeTypeReference(propertyType);
            CreatedProperty.Name = propertyName;
            CreatedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
            CreatedProperty.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
            TheType.Members.Add(CreatedProperty);
            TheType.Members.Add(PropertyField);
        }
        public void BuildIndexerProperty(string propertyName, string propertyType, string fieldName, string setpremission, List<string> fields)
        {
            CodeMemberField PropertyField = new CodeMemberField(propertyType, fieldName);
            CodeMemberProperty CreatedProperty = new CodeMemberProperty();
            CreatedProperty.Attributes = MemberAttributes.Public;
            CreatedProperty.Type = new CodeTypeReference(propertyType);
            CreatedProperty.Name = propertyName;
            CreatedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));



            CodeSnippetTypeMember CreatePropertySnippet = new CodeSnippetTypeMember();
            CreatePropertySnippet.Text = string.Format("public {0} {1} {{ get {{return {2};  }} {3} set {{   ", propertyType, CreatedProperty.Name, PropertyField.Name, setpremission);
            foreach(string field in fields)
            {
                CreatePropertySnippet.Text += field + "= value;";
            }
            CreatePropertySnippet.Text += "}}";
            TheType.Members.Add(CreatePropertySnippet);
            TheType.Members.Add(PropertyField);
        }
        protected void BuildStaticProperty(string propertyName, string propertyType, string fieldName)
        {
            CodeMemberField PropertyField = new CodeMemberField(propertyType, fieldName);
            CodeMemberProperty CreatedProperty = new CodeMemberProperty();
            CreatedProperty.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            CreatedProperty.Type = new CodeTypeReference(propertyType);
            CreatedProperty.Name = propertyName;
            CreatedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
            CreatedProperty.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
            TheType.Members.Add(CreatedProperty);
            TheType.Members.Add(PropertyField);
        }
        public void BuildProperty(string propertyName, string propertyType, string fieldName, string setPermission)
        {
            CodeMemberField PropertyField = new CodeMemberField(propertyType, fieldName);
            CodeMemberProperty CreatedProperty = new CodeMemberProperty();
            CreatedProperty.Attributes = MemberAttributes.Public;
            CreatedProperty.Type = new CodeTypeReference(propertyType);
            CreatedProperty.Name = propertyName;
            CreatedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
           
            
            
            CodeSnippetTypeMember CreatePropertySnippet = new CodeSnippetTypeMember();
            CreatePropertySnippet.Text = string.Format("public {0} {1} {{ get {{return {2};  }} {3} set {{ {2}=value; }} }}  ", propertyType, CreatedProperty.Name, PropertyField.Name, setPermission);
            TheType.Members.Add(CreatePropertySnippet);
            TheType.Members.Add(PropertyField);
        }
        internal void BuildNotifyableProperty(string propertyName, string propertyType, string notifyEvent, string notifyparam, string fieldName)
        {
            CodeMemberField PropertyField = new CodeMemberField(propertyType, fieldName);
            CodeMemberProperty CreatedProperty = new CodeMemberProperty();
            CreatedProperty.Type = new CodeTypeReference(propertyType);
            CreatedProperty.Attributes = MemberAttributes.Public;
            CreatedProperty.Name = propertyName;
            CreatedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
            CodeStatement PropertySetStatement = GetMethodInvokationStatement("this", notifyEvent, false, new List<string>() { notifyparam });
            CreatedProperty.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
            CreatedProperty.SetStatements.Add(PropertySetStatement);
            TheType.Members.Add(CreatedProperty);
            TheType.Members.Add(PropertyField);
        }

        /// <summary>
        /// Creates method signature without return type(returns void)
        /// Example: <code>public void a(string h)</code>
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <param name="permission">Access Modifier-private, public</param>
        /// <param name="lstParameters">List of key value pairs - the key is type, the value is name</param>
        /// <returns>Object that represents c# method</returns>
        public CodeMemberMethod BuildMethodSignature(string methodName, MemberAttributes permission, List<KeyValuePair<string,string>> lstParameters)
        {
            CodeMemberMethod cmmMethod = new CodeMemberMethod();
            foreach (KeyValuePair<string, string> kvpParameter in lstParameters)
            {
                cmmMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(kvpParameter.Key), kvpParameter.Value));
            }
            //cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("EventArgs"), "e"));
            cmmMethod.Name = methodName;
            cmmMethod.Attributes = permission;
            return cmmMethod;
        }

        /// <summary>
        /// Creates method signature with return type
        /// Example: <code>public int a(string h)</code>
        /// </summary>
        /// <param name="methodName">Name of method</param>
        /// <param name="permission">Access Modifier-private, public</param>
        /// <param name="lstParameters">List of key value pairs - the key is type, the value is name</param>
        /// <param name="returntype"> Return type of the method</param>
         /// <returns>Object that represents c# method</returns>
        public CodeMemberMethod BuildMethodSignature(string methodName, MemberAttributes permission, List<KeyValuePair<string, string>> lstParameters, string returntype)
        {
            CodeMemberMethod cmmMethod = new CodeMemberMethod();
            foreach (KeyValuePair<string, string> kvpParameter in lstParameters)
            {
                cmmMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(kvpParameter.Key), kvpParameter.Value));
            }
            //cmm.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("EventArgs"), "e"));
            cmmMethod.Name = methodName;
            cmmMethod.Attributes = permission;
            cmmMethod.ReturnType = new CodeTypeReference("System.String");
            return cmmMethod;
        }
      
        /// <summary>
        /// Creates call to method
        /// Example: <code>A(5)</code>
        /// </summary>
        /// <param name="targetObject">The owner of the method-can be variable or this</param>
        /// <param name="method">Name of methods</param>
        /// <param name="lstParameters">Parameters-list of variables to use in a call</param>
        /// <returns>Statement that represents call to method</returns>
        public CodeStatement GetMethodInvokationStatement(string targetObject, string method, List<string> lstParameters)
        {
            List<CodeExpression> lstParams = new List<CodeExpression>();
            foreach(string parameter in lstParameters)
            {
                if (parameter != "this")
                    lstParams.Add(new CodeTypeReferenceExpression(new CodeTypeReference(parameter)));
                else if (parameter == "null")
                    lstParams.Add(new CodePrimitiveExpression(null));
                else lstParams.Add(new CodeThisReferenceExpression());
            }
            CodeExpression ceObjectExpression;
            if (targetObject != "this")
                ceObjectExpression = new CodeTypeReferenceExpression(new CodeTypeReference(targetObject));
            else ceObjectExpression = new CodeThisReferenceExpression();
            CodeExpression invokeExpression = new CodeMethodInvokeExpression(
                 ceObjectExpression,
                 method,lstParams.ToArray());

            // Creates a statement using a code expression.
            CodeExpressionStatement expressionStatement;
            expressionStatement = new CodeExpressionStatement(invokeExpression);
            return expressionStatement;
        }

        /// <summary>
        /// Creates call to method
        /// Example: <code>A(5)</code>
        /// </summary>
        /// <param name="targetObject">The owner of the method-can be variable or this</param>
        /// <param name="method">Name of methods</param>
        /// <param name="isVariable">indicates that all parameters are variable(compare a and "a") or all are strings</param>
        /// <param name="lstParameters">List of parameters</param>
        /// <returns>Statement that represents call to method</returns>
        public CodeStatement GetMethodInvokationStatement(string targetObject, string method, bool isVariable, List<string> lstParameters)
        {
            List<CodeExpression> lstParams = new List<CodeExpression>();
            foreach (string parameter in lstParameters)
            {
                if (parameter != "this")
                {
                    if (isVariable)
                        lstParams.Add(new CodeTypeReferenceExpression(new CodeTypeReference(parameter)));
                    else lstParams.Add(new CodePrimitiveExpression(parameter));
                }
                else lstParams.Add(new CodeThisReferenceExpression());
            }
            CodeExpression ceObjectExpression;
            if (targetObject != "this")
                ceObjectExpression = new CodeTypeReferenceExpression(new CodeTypeReference(targetObject));
            else ceObjectExpression = new CodeThisReferenceExpression();
            CodeExpression invokeExpression = new CodeMethodInvokeExpression(
                 ceObjectExpression,
                 method, lstParams.ToArray());

            // Creates a statement using a code expression.
            CodeExpressionStatement expressionStatement;
            expressionStatement = new CodeExpressionStatement(invokeExpression);
            return expressionStatement;
        }

        /// <summary>
        /// Creates call to method
        /// Example: <code>A(5)</code>
        /// </summary>
        /// <param name="targetObject">The owner of the method-can be variable or this</param>
        /// <param name="method">Name of methods</param>
    
        /// <param name="lstParameters">List of parameters</param>
        /// <returns>Expression that represents call to method(needed when call to method is part of assignment)</returns>
        public CodeExpression GetMethodInvokationExpression(string targetObject, string method, List<CodeExpression> lstParameters)
        {
            List<CodeExpression> lstParams = new List<CodeExpression>();
            
            CodeExpression CallingObjectExpression;
            if (targetObject != "this")
            {
                CallingObjectExpression = new CodeTypeReferenceExpression(new CodeTypeReference(targetObject));
            }
            else CallingObjectExpression = new CodeThisReferenceExpression();
            CodeExpression invokeExpression = new CodeMethodInvokeExpression(
                CallingObjectExpression,
                 method, lstParameters.ToArray());

            // Creates a statement using a code expression.
            CodeExpressionStatement expressionStatement;
            expressionStatement = new CodeExpressionStatement(invokeExpression);
            return invokeExpression;
        }

        /// <summary>
        /// Generate expression that represents call to a method
        /// </summary>
        /// <param name="targetObject">Object that initiates a call ( for example: in string.Format it is string)</param>
        /// <param name="method">Method name to call</param>
        /// <param name="lstParameters">List of parameters-list of keyvalue pairs where key is name and value is bool that indicates if a name is variable or this is text of string parameter(compare a and "a")</param>
        /// <returns>Expression that contains call to method</returns>
        public CodeExpression GetMethodInvokationExpression(string targetObject, string method, List<KeyValuePair<string,bool>> lstParameters)
        {
            List<CodeExpression> lstParams = new List<CodeExpression>();
            foreach (KeyValuePair<string,bool>  parameter in lstParameters)
            {
                if (parameter.Key != "this")
                {
                    CodeExpression ParameterExpression;
                    if (parameter.Value)
                        ParameterExpression = new CodeTypeReferenceExpression(new CodeTypeReference(parameter.Key));
                    else ParameterExpression = new CodePrimitiveExpression(parameter.Key);
                    lstParams.Add(ParameterExpression);
                }
                else lstParams.Add(new CodeThisReferenceExpression());
            }
            CodeExpression CallingObjectExpression;
            if (targetObject != "this")
            {
                CallingObjectExpression = new CodeTypeReferenceExpression(new CodeTypeReference(targetObject));
            }
            else CallingObjectExpression = new CodeThisReferenceExpression();
            CodeExpression invokeExpression = new CodeMethodInvokeExpression(
                CallingObjectExpression,
                 method, lstParams.ToArray());

            // Creates a statement using a code expression.
            CodeExpressionStatement expressionStatement;
            expressionStatement = new CodeExpressionStatement(invokeExpression);
            return invokeExpression;
        }


        /// <summary>
        /// Generate expression that represents Create Object(new keyword)
        /// </summary>
        /// <param name="objectType">Type of object to create</param>
        /// <param name="variablename">Name of variable</param>
        /// <param name="parameters">names of variables that serves as parameters in object's constructor</param>
        /// <returns>Statement that represents object create</returns>
        public CodeStatement GetObjectCreateStatement(string objectType, string variablename, List<string> parameters)
        {
            List<CodeExpression> lst = new List<CodeExpression>();
            foreach(string paramValue in parameters)
            {
                lst.Add(new CodePrimitiveExpression(paramValue));
            }
            CodeExpression CreateObjectExpression = new CodeObjectCreateExpression(new CodeTypeReference(string.Format(objectType)), lst.ToArray());
            CodeExpression thisExpression = new CodeThisReferenceExpression();
            CodePropertyReferenceExpression CreateObjectPropertyExpression = new CodePropertyReferenceExpression(thisExpression, string.Format(variablename));
            CodeStatement CreateObjectPropertyAssignStatement = new CodeAssignStatement(CreateObjectPropertyExpression, CreateObjectExpression);
            return CreateObjectPropertyAssignStatement;

        }

        /// <summary>
        /// Generate expression that represents Create Object(new keyword)
        /// </summary>
        /// <param name="objectType">Type of object to create</param>
        /// <param name="variablename">Name of variable</param>
        /// <param name="isLocal"> boolean that indicates if the creates object meber of this class or not(to know if to add this or not comapre this.a and a)</param>
        /// <param name="parameters">names of variables that serves as parameters in object's constructor</param>
        /// <returns>Statement that represents object create</returns>
        public CodeStatement GetObjectCreateStatement(string objectType, string variablename, bool isLocal, List<string> parameters)
        {
            List<CodeExpression> lst = new List<CodeExpression>();
            foreach (string paramValue in parameters)
            {
                lst.Add(new CodePrimitiveExpression(paramValue));
            }
            CodeExpression CreateObjectExpression = new CodeObjectCreateExpression(new CodeTypeReference(string.Format(objectType)), lst.ToArray());
            CodeExpression thisExpression = new CodeThisReferenceExpression();
            CodeExpression CreateObjectVariableExpression;
            if (isLocal)
                CreateObjectVariableExpression = new CodeVariableReferenceExpression(string.Format(variablename));
            else CreateObjectVariableExpression = new CodePropertyReferenceExpression(thisExpression, string.Format(variablename));
            CodeStatement CreateObjectVariableAssignStatement = new CodeAssignStatement(CreateObjectVariableExpression, CreateObjectExpression);
            return CreateObjectVariableAssignStatement;

        }

        /// <summary>
        /// Generate expression that represents Create Object(new keyword)
        /// for generic type
        /// </summary>
        /// <param name="objectType">Type of object to create</param>
        /// <param name="variablename">Name of variable</param>
        /// <param name="parameters">names of variables that serves as parameters in object's constructor</param>
       ///<param name="genericelement">Name of generic variable to create(commonly it is t to generate for example Property with generic type t</param>
            /// <returns>Statement that represents object create</returns>
        public CodeStatement GetObjectCreateStatementGeneric(string objectType, string genericelement, string variablename, List<string> parameters)
        {
            List<CodeExpression> lst = new List<CodeExpression>();
            foreach (string paramValue in parameters)
            {
                lst.Add(new CodePrimitiveExpression(paramValue));
            }
            CodeExpression CreateObjectExpression = new CodeObjectCreateExpression(new CodeTypeReference(string.Format("{0}<{1}>",objectType, genericelement)), lst.ToArray());
            CodeExpression thisExpression = new CodeThisReferenceExpression();
            CodePropertyReferenceExpression PropertyExpression = new CodePropertyReferenceExpression(thisExpression, string.Format(variablename));
            CodeStatement PropertyAssignStatement = new CodeAssignStatement(PropertyExpression, CreateObjectExpression);
            return PropertyAssignStatement;

        }
        public CodeStatement GetObjectCreateStatement(string objectType, string variablename)
        {
            CodeExpression ObjectCreateStatement = new CodeObjectCreateExpression(new CodeTypeReference(string.Format(objectType)));
            CodeExpression thisExpression = new CodeThisReferenceExpression();
            CodePropertyReferenceExpression PropertyExpression = new CodePropertyReferenceExpression(thisExpression, string.Format(variablename));
            CodeStatement AssignStatement = new CodeAssignStatement(PropertyExpression, ObjectCreateStatement);
            return AssignStatement;
        }

        /// <summary>
        /// Generate expression that represents Create Object(new keyword)
        /// with constructor without parameters
        /// </summary>
        /// <param name="objectType">Type of object to create</param>
        /// <param name="variablename">Name of variable</param>
        /// <param name="isLocal"> boolean that indicates if the creates object meber of this class or not(to know if to add this or not comapre this.a and a)</param>
        /// <returns>Statement that represents object create</returns>
        public CodeStatement GetObjectCreateStatement(string objectType, string variablename, bool isLocalorStatic)
        {
            CodeExpression CreateObjectExpression = new CodeObjectCreateExpression(new CodeTypeReference(string.Format(objectType)));
            CodeExpression thisExpression = new CodeThisReferenceExpression();
            CodeExpression VariableExpression;
            if (isLocalorStatic)
                VariableExpression = new CodeVariableReferenceExpression(string.Format(variablename));
            else VariableExpression = new CodePropertyReferenceExpression(thisExpression, string.Format(variablename));
            CodeStatement VariableAssignStatement = new CodeAssignStatement(VariableExpression, CreateObjectExpression);
            return VariableAssignStatement;
        }

        /// <summary>
        /// Create Delegate Define (the += operation of event delegate)
        /// </summary>
        /// <param name="eventHandlerType">Type of Event Delegate</param>
        /// <param name="variablename">Name of variable</param>
        /// <param name="methodName">nameOf method</param>
        /// <param name="eventName">name of event</param>
        /// <param name="isClassVariable">Variable in class or from other class</param>
        /// <returns>Statement with delegate creation</returns>
        public CodeStatement GetDelegateCreateStatement(string eventHandlerType, string variablename, string methodName, string eventName, bool isClassVariable)
        {
            CodeExpression thisExpression = new CodeThisReferenceExpression();
            CodeExpression DelegeteVariableExpression;
            if (isClassVariable)
                DelegeteVariableExpression = new CodePropertyReferenceExpression(thisExpression, string.Format(variablename));
            else DelegeteVariableExpression = new CodeVariableReferenceExpression(string.Format(variablename));
            CodeDelegateCreateExpression DelegateCreateExpression = new CodeDelegateCreateExpression(
                new CodeTypeReference(eventHandlerType), new CodeThisReferenceExpression(), string.Format(methodName));
                // Attaches an EventHandler delegate pointing to TestMethod to the TestEvent event.
                CodeAttachEventStatement attachEventStatement = new CodeAttachEventStatement(DelegeteVariableExpression, eventName, DelegateCreateExpression);
            return attachEventStatement;
        }
        public void CreateSingletonAccessor(string propertyName, string fieldName,  string className)
        {
            CodeMemberField PropertyField = new CodeMemberField(className, fieldName);
            PropertyField.Attributes = MemberAttributes.Static;
            CodeMemberProperty CreatedProperty = new CodeMemberProperty();
            CreatedProperty.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            CreatedProperty.Type = new CodeTypeReference(className);
            CreatedProperty.Name = propertyName;
            CodeStatement csIf = GetUnoConditionIf(fieldName, CodeBinaryOperatorType.IdentityEquality, "null", new List<CodeStatement>() { GetObjectCreateStatement(className, fieldName, true) });
            CreatedProperty.GetStatements.Add(csIf);
            CreatedProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression( fieldName)));
            CreatedProperty.SetStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(fieldName), new CodePropertySetValueReferenceExpression()));
            TheType.Members.Add(CreatedProperty);
            TheType.Members.Add(PropertyField);
        }
        #endregion
    }
}
