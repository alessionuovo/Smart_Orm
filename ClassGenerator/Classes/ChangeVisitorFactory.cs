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
    public class ChangeVisitorFactory : CreatedClass
   {
       public ChangeVisitorFactory(string directory, string name) : base(directory, name)
       {

            CreateGenericType(new List<GenericClassParameters>() { new GenericClassParameters() { GenericType = "T",  Constraint = new List<string>() { "ITable", "new()" } } });
            TheType.Attributes = MemberAttributes.Abstract ;
        }

       protected override void BuildDefaultConstructorBody()
       {
          

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
           
       }

       /// <summary>
       /// Create NotifyPropertyChanged event handler
       /// and invoke delegate if is not null
       /// </summary>
       public override void BuildMethods()
       {
           base.BuildMethods();
           CodeMemberMethod PropertyChangeMethod = BuildMethodSignature("BuildVisitor", MemberAttributes.Public | MemberAttributes.Abstract, new List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("System.String", "propertyName")});
         
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