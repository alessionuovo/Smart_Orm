
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
namespace GeneratedDatabase
{
public class Generate {
public string path="I:\\Smart_Orm\\Smart_Orm\\Smart_Orm\\Generate\\";
public ObservableCollection<First> First_Values;
public ObservableCollection<Second> Second_Values;
public Generate()
{
First_Values=new ObservableCollection<First>();
First_Values.CollectionChanged+=First_CollectionChanged;
XDocument xd=XDocument.Load("I:\\Smart_Orm\\Smart_Orm\\Smart_Orm\\Generate\\First.xml");
XElement xe=xd.Root.Data;
if (xe!=null){
foreach(XElement xee in xe.Elements("Element"))
{
First elem=new First();
 var type = elem.GetType();
foreach(XElement xeProperty in xee.Elements("Property"))
{
string name=xeProperty.Attribute("Name");
string value=xeProperty.Attribute("Value");
 var property = type.GetProperty(name);
property.SetValue(elem, value);
}
}
}

Second_Values=new ObservableCollection<Second>();
Second_Values.CollectionChanged+=Second_CollectionChanged;
XDocument xd=XDocument.Load("I:\\Smart_Orm\\Smart_Orm\\Smart_Orm\\Generate\\Second.xml");
XElement xe=xd.Root.Data;
if (xe!=null){
foreach(XElement xee in xe.Elements("Element"))
{
Second elem=new Second();
 var type = elem.GetType();
foreach(XElement xeProperty in xee.Elements("Property"))
{
string name=xeProperty.Attribute("Name");
string value=xeProperty.Attribute("Value");
 var property = type.GetProperty(name);
property.SetValue(elem, value);
}
}
}

}
private void First_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
{
}
private void Second_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
{
}
}
}
