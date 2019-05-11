using DiagramContent;
using DiagramManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiagramManagement
{
    /** \ingroup Bll
     <summary>
     This class is part of command design pattern
     Also it is implemented as singleton
     and its objective to manage the changes 
     
     that occur in system
     </summary> 
    /// Contains:
    /// <list type="bullet">
    /// <item>
    /// <term>Method Add</term>
    /// <description>Adds the object that was changed
    /// as one of three commands:
    /// <list type="bullet">
    /// <item>
    /// <term>AddCommand</term>
    /// <description>The change was that element was added to system</description>
    /// <term>UpdateCommand</term>
    /// <description>The change was that element was updated in the system</description>
    /// </item>
    /// <term>DeleteCommand</term>
    /// <description>The change was that element was deleted from the system</description>
    /// </list>
    /// </description>
    /// </item>
    /// <item><term>Method Execute</term>
    /// <description>Stores all the changes to permanent place(before all the changes are in memory)</description>
    /// </item>
    /// 
    /// </list>
        Part of \ref Command 
    */
    public class ChangeManager
    {
        private ChangeManager()
        {
            HasChanges = false;
        }
        //singleton implementation
        private static ChangeManager _changeManager; //represents the instance of change manager
        public static ChangeManager Manager { get { if (_changeManager == null) _changeManager = new ChangeManager(); return _changeManager; } }

        public bool HasChanges { get; set; }

        /// <summary>
        /// Represents the list of changes that occur in system.
        /// Changes are stored by their name(name can be anythinq needed by system).
        /// </summary>
        public SortedList<string, ICommand> commands = new SortedList<string, ICommand>();

        //Stores all changes of the system to permanent place
        public void Execute()
        {
            foreach(KeyValuePair<string,ICommand> kvpCommand in commands)
            {
                ICommand command = kvpCommand.Value;
                command.Execute();
            }
            commands.Clear();
            HasChanges = false;
        }

        //Accepts element to add
        //and adds the last change to changes tracking system
        public void Add<DataElementType>(DataElementType dataElement, State state)
        {
            HasChanges = true;
            string name;
            bool checkTypeValidity = TryGetNameProperty(dataElement, out name);
            if (!checkTypeValidity)
                throw new Exception("Cannot store element without a name");
            ICommand command=null;
            bool wasCommandCreated = true;
            
            switch (state)
            {

                case State.Add: 
                    command = new AddCommand<DataElementType>(dataElement);
                    break;
                case State.Update: bool found = false;
                    foreach(KeyValuePair<string,ICommand> kvpCommand in commands)
                    {
                       
                       
                        if (kvpCommand.Key==name )
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        command = new UpdateCommand<DataElementType>(dataElement);
                    }
                    else wasCommandCreated = false;
                    break;
                case State.Delete:
                    command = new DeleteCommand<DataElementType>(dataElement);
                    if (commands.ContainsKey(name))
                        commands.Remove(name);
                        break;
                default: command = new UpdateCommand<DataElementType>(dataElement);
                    break;
            }
            if (wasCommandCreated && command != null)
            {
                
                  

                this.commands.Add(name, command);
            }
            
        }

        //Trying to get name property of the element, because
        //we cannot add element without the name.
        protected bool TryGetNameProperty<DataElementType>(DataElementType element, out string name)
        {
            PropertyInfo p=element.GetType().GetProperty("Name") ;
            if (p != null)
                name = p.GetValue(element).ToString();
            else name = string.Empty;
            return p != null;
        }

      
       
    }
}
