using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Utilities
{
    /** \ingroup Bll
     <summary>
     Helps to make searches in wpf controls
     </summary>
        */
    public static class WpfUIUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ChildControl">Some Control Type which is searched</typeparam>
        /// <param name="DependencyObj">Object in which to search</param>
        /// <param name="name">Name of Control to search</param>
        /// <returns>Control with specified name or null if not exist</returns>
        public static ChildControl FindVisualChildByName<ChildControl>(DependencyObject DependencyObj, string name)
     where ChildControl : FrameworkElement
        {
            int k = VisualTreeHelper.GetChildrenCount(DependencyObj); ;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl && (Child as ChildControl).Name==name)
                {
                    return (ChildControl)Child;
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChildByName<ChildControl>(Child, name);

                    if (ChildOfChild != null && ChildOfChild.Name==name)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }
        /// </summary>
        /// <typeparam name="ChildControl">Some Control Type which is searched</typeparam>
        /// <param name="DependencyObj">Object in which to search</param>
      
        /// <returns>Control with specified name or null if not exist</returns>
        /// </summary>
        public static ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj)
     where ChildControl : DependencyObject
        {
            int k = VisualTreeHelper.GetChildrenCount(DependencyObj); ;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    return (ChildControl)Child;
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }
        /*  public static IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
          {
              if (obj != null)
              {
                  if (obj is T)
                      yield return obj as T;

                  foreach (DependencyObject child in VisualTreeHelper.GetChildren(obj).OfType<DependencyObject>())
                      foreach (T c in FindLogicalChildren<T>(child))
                          yield return c;
              }
          }*/
    /*public static IEnumerable<ChildControl> FindVisualChildren<ChildControl>(DependencyObject DependencyObj)
   where ChildControl : DependencyObject
    {
        //return new List<ChildControl>();
        int k = VisualTreeHelper.GetChildrenCount(DependencyObj); ;
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
        {
            DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

            if (Child != null && Child is ChildControl)
            {
               yield return (ChildControl)Child;
            }
            else
            {
                IEnumerable<ChildControl> ChildOfChild = FindVisualChildren<ChildControl>(Child).ToList();

                if (ChildOfChild != null)
                {
                    yield return ChildOfChild;
                }
            }
        }

    }*/

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type to search</typeparam>
    /// <param name="depObj">in what object(in its children)-this is for itemscontrol (search in template)</param>
    /// <returns>Collection of elements of specified type</returns>
    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Searches in children of StackPanel, and other container elements not in template
        /// </summary>
        /// <typeparam name="ChildControl">Type to search</typeparam>
        /// <param name="DependencyObj">in which control</param>
        /// <returns>Control of specified type in children of searched control</returns>
        public static ChildControl  FindLogicalChild<ChildControl>(DependencyObject DependencyObj) where ChildControl : DependencyObject
        {
            if (DependencyObj != null)
                foreach (object logicalChild in LogicalTreeHelper.GetChildren(DependencyObj))
                {
                    DependencyObject logChild = logicalChild as DependencyObject;
                    DependencyObject child=FindLogicalChild<ChildControl>(logChild);

            if (child != null && child is ChildControl)
                {
                    return (ChildControl)child;
                }
                
            }
            return null;
        }
    }
}
