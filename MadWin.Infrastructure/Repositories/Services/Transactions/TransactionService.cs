

using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Advices;
using MadWin.Core.Entities.Transactions;

namespace Shop2City.Core.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly MadWinDBContext _context;
        public TransactionService(MadWinDBContext context)
        {
            _context = context;   
        }

        public async Task AddAdvice(AdviceModel advice)
        {
            _context.Advices.Add(advice);
            await _context.SaveChangesAsync();
        }

        public async Task AddTransaction(TransactionModel transaction)
        {
            _context.Transactions.Add(transaction);
            await  _context.SaveChangesAsync();
        }
    }
}
