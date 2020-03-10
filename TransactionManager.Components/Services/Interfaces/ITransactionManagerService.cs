using TransactionManager.Components.Domain.Enums;
using TransactionManager.Components.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransactionManager.Components.Services.Interfaces
{
    public interface ITransactionManagerService
    {
        Task ImportTransactions(UploadFileRequest request);
        Task<List<Models.Response.Transaction>> GetTransactions(Status status);
        Task<List<Models.Response.Transaction>> GetTransactions(string startDate, string endDate);
        Task<List<Models.Response.Transaction>> GetTransactions(string currencyCode);
    }
}
