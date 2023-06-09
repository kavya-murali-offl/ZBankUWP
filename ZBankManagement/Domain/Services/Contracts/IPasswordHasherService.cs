﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.ZBankManagement.Services.Contracts
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password, string salt);
        string GenerateSalt();
    }
}
