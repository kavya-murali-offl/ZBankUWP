using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.AppEvents.AppEventArgs
{
    public class CardPageDataUpdatedArgs
    {
        public IEnumerable<Card> CardsList { get; set; }   
    }

}
