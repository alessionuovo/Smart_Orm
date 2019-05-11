using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Utilities
{
    /** \ingroup Dal
    @{
     <summary>
     class XmlHelper-helps to search in xml
     </summary>
    */
    public static class XmlHelper
    {
        /// <summary>
        /// Safely(without exceptions get attribute
        /// </summary>
        /// <param name="element">in which XElement</param>
        /// <param name="name">Name of attribute</param>
        /// <returns>Value of attribute</returns>
        public static string GetAttributeValue(this XElement element, string name)
        {
            XAttribute attr = element.Attribute(name);
            if (attr == null) return string.Empty;
            string value = attr.Value;
            return value;
        }
    }
    /* @} */
}
