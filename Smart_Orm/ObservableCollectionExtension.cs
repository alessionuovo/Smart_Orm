using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Smart_Orm
{
    public class ObservableCollectionExtension:MarkupExtension, IList
    {
     
        public Type ValueType { get; set; }

        private IList _dictionary;
        private IList Dictionary
        {
            get
            {
                if (_dictionary == null)
                {
                    var type = typeof(ObservableCollection<>);
                    var dictType = type.MakeGenericType(ValueType);
                    _dictionary = (IList)Activator.CreateInstance(dictType);
                }
                return _dictionary;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsFixedSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Dictionary;
        }

        public void Add(object value)
        {
            if (!ValueType.IsAssignableFrom(value.GetType()))
                value = TypeDescriptor.GetConverter(ValueType).ConvertFrom(value);
            Dictionary.Add(value);
        }

        #region Other Interface Members
        public void Clear()
        {
            throw new NotSupportedException();
        }

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
