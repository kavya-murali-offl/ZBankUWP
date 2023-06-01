using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
        public class DropDownItem
        {
            public DropDownItem(string text, object value)
            {
                Text = text;
                Value = value;
            }
            public string Text { get; set; }
            public object Value { get; set; }
        }
}
