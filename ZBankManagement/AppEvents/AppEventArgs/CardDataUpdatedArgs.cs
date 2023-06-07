using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity.BusinessObjects;

namespace ZBank.AppEvents.AppEventArgs
{
    public class CardDataUpdatedArgs
    {
        public IEnumerable<CardBObj> CardsList { get; set; }   
    }

}
