using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.ViewModel.VMObjects
{
    public class FormBase<TModel> : ViewModelBase where TModel : class
    {
        private TModel _item = null;
        public TModel Item
        {
            get => _item;
            set
            {
                if (Set(ref _item, value))
                {
                    EditableItem = _item;
                }
            }
        }

        private TModel _editableItem = null;
        public TModel EditableItem
        {
            get => _editableItem;
            set => Set(ref _editableItem, value);
        }

        private bool _isNew = false;
        public bool IsNew
        {
            get => _isNew;
            set => Set(ref _isNew, value);
        }
    }
}
