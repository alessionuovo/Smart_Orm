using System;
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
    public class FileManagerFactory : CreatedClass
   {
       public FileManagerFactory(string directory, string name) : base(directory, name)
       {
           
           
       }

       protected override void BuildDefaultConstructorBody()
       {
           //base.BuildDefaultConstructorBody();

       }
       
       /// <summary>
       /// Build constructor with propertyValue argument
       /// and the constructor converts value from string to corresponding property type
       /// using IConvertible interface(Convert.ChangeType)
       /// </summary>
       public override void BuildOtherConstructors()
       {
           
       }
       public override void BuildPropertiesAndFields()
       {
           base.BuildPropertiesAndFields();
         
       }

       /// <summary>
       /// Create NotifyPropertyChanged event handler
       /// and invoke delegate if is not null
       /// </summary>
       public override void BuildMethods()
       {
           base.BuildMethods();
           CodeMemberMethod PropertyChangeMethod = BuildMethodSignature("BuildVisitor", MemberAttributes.Public | MemberAttributes.Override, new List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("string", "format")}, "IChangeVisitor<T>");
            CodeStatement PropertyChangedInvokationStatement = new CodeMethodReturnStatement(new CodeObjectCreateExpression("FileManager<T>", new CodePrimitiveExpression("XML")));
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