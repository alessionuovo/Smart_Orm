using System.CodeDom;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    Represents generated ITable interface
    This interface only defines one event
    TableElementChanged-that listens to changes in table records.
       Also part of \ref Builder 
    </summary>
   */
    internal class ITableInterface : CreatedInterface
    {
        public ITableInterface(string directory, string name, string type) : base(directory, name)
        {
            this.directory = directory;
            BuildEvent("TableElementChanged", "EventHandler");
           // TheType.BaseTypes.Add("INotifyP")
        }

    }
}