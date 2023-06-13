﻿using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.Interface
{
    public interface IInsertAccountDataManager
    {
        Task InsertAccount(InsertAccountRequest request, IUseCaseCallback<InsertAccountResponse> callback);
       

    }
}