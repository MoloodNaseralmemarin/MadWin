
using MadWin.Core.Entities.Advices;
using MadWin.Core.Entities.Transactions;

namespace Shop2City.Core.Services.Transactions
{
    public interface ITransactionService
    {
        Task AddTransaction(TransactionModel transaction);

        Task AddAdvice(AdviceModel transaction);
    }
}
