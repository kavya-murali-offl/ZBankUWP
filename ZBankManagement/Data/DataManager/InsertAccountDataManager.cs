using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using ZBank.DatabaseHandler;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;
using ZBankManagement.Entity.DTOs;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace ZBankManagement.DataManager
{
    class InsertAccountDataManager : IInsertAccountDataManager
    {

        public InsertAccountDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        private async Task<byte[]> GetBytesFromFile(StorageFile file)
        {
            var stream = await file.OpenStreamForReadAsync();
            byte[] bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public async Task InsertAccount(InsertAccountRequest request, IUseCaseCallback<InsertAccountResponse> callback)
        {
            try
            {
                IEnumerable<KYCDocuments> documents = new List<KYCDocuments>(); 
                foreach(var doc in request.Documents)
                {
                    byte[] fileBytes = await GetBytesFromFile(doc);
                    KYCDocuments kycDoc = new KYCDocuments()
                    {
                        ID = Guid.NewGuid().ToString(),
                        File = fileBytes,
                        FileName = doc.Name,
                        UploadedOn = DateTime.Now,
                    };
                }


                await DBHandler.InsertAccount(request.AccountToInsert, documents);
               InsertAccountResponse response = new InsertAccountResponse
               {
                        IsSuccess = true,
                        InsertedAccount = request.AccountToInsert
               };
               callback.OnSuccess(response);
              
            }
            catch(Exception err)
            {
                ZBankException error = new ZBankException();
                error.Message = err.Message;
                error.Type = ErrorType.DATABASE_ERROR;
                callback.OnFailure(error);
            }
        }

    }
}



