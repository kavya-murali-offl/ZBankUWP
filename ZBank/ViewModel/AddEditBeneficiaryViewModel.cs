using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.View;
using ZBankManagement.Entity.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBank.ViewModel
{
    public class AddEditBeneficiaryViewModel : ViewModelBase
    {
        private IView View { get; set; }
        private BeneficiaryBObj _selectedBeneficiary {  get; set; } 
        
        public BeneficiaryBObj SelectedBeneficiary
        {
            get { return _selectedBeneficiary; }
            set { _selectedBeneficiary = value; }
        }

        public AddEditBeneficiaryViewModel(IView view) 
        { 
            View = view;
        }
        
        public IEnumerable<BeneficiaryType> BeneficiaryTypes
        {
            get
            {
                return new List<BeneficiaryType>()
                {
                    BeneficiaryType.WITHIN_BANK,
                    BeneficiaryType.OTHER_BANK
                };
            }
        }

    }
}
