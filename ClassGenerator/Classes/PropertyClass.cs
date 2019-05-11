﻿using System;
using System.CodeDom;
using System.Collections.Generic;
namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Represents a generated class property
    Property represents field of database table.
    this class implements INotifyPropertyChanged.
         Also part of \ref Builder 
    </summary>
    */
    public class PropertyClass : CreatedClass
   {
       public PropertyClass(string directory, string name) : base(directory, name)
       {
           
           
       }

       protected override void BuildDefaultConstructorBody()
       {
           base.BuildDefaultConstructorBody();

       }
       
       /// <summary>
       /// Build constructor with propertyValue argument
       /// and the constructor converts value from string to corresponding property type
       /// using IConvertible interface(Convert.ChangeType)
       /// </summary>
       public override void BuildOtherConstructors()
       {
           CodeConstructor codeConstructor = new CodeConstructor();
           codeConstructor.Attributes = MemberAttributes.Public;
           codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "propertyValue"));
           CodeExpression ChangeTypeInvokationExpression = GetMethodInvokationExpression("Convert", "ChangeType", new List<CodeExpression>() { new CodeVariableReferenceExpression("propertyValue"), new CodeTypeOfExpression(new CodeTypeReference("T")) });
           CodeStatement ValueObjectAssignmentStatement = GetAssignment("System.Object", "pValue", ChangeTypeInvokationExpression);
           codeConstructor.Statements.Add(ValueObjectAssignmentStatement);
           CodeStatement ValueAssignmentStatement = GetCastWithAssignmentDeclare("T", "Value", "pValue");
           codeConstructor.Statements.Add(ValueAssignmentStatement);
           TheType.Members.Add(codeConstructor);
       }
       public override void BuildPropertiesAndFields()
       {
           base.BuildPropertiesAndFields();
           BuildEvent("TablePropertyChanged", "PropertyChangedEventHandler", MemberAttributes.Public);
       }

       /// <summary>
       /// Create NotifyPropertyChanged event handler
       /// and invoke delegate if is not null
       /// </summary>
       public override void BuildMethods()
       {
           base.BuildMethods();
           CodeMemberMethod PropertyChangeMethod = BuildMethodSignature("NotifyPropertyChanged", MemberAttributes.Family, new List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("System.String", "propertyName")});
           CodeStatement PropertyChangedInvokationStatement = GetEventHandlerInvokation("PropertyChanged", "PropertyChangedEventArgs", new List<string>() { "propertyName"} );
           PropertyChangeMethod.Statements.Add(PropertyChangedInvokationStatement);
           TheType.Members.Add(PropertyChangeMethod);
       }
       public override void BuildEventHandlers()
       {
         /*  base.BuildEventHandlers();
           CodeMemberMethod cmm = BuildMethodSignature("NotifyPropertyChanged", MemberAttributes.Family, new List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("System.String", "propertyName") });
           CodeStatement cer = GetEventHandlerInvokation("PropertyChanged", "PropertyChangedEventArgs", new List<string>() { "propertyName" });
           cmm.Statements.Add(cer);
           TheType.Members.Add(cmm);*/
}


    }
}