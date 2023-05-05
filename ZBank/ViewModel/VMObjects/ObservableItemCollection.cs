using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.ViewModel.VMObjects
{
    public class ObservableItemCollection<T> : ObservableCollection<T>
    {
        public ObservableItemCollection(IEnumerable<T> enumerableData) : base(enumerableData) { }
    }
}
