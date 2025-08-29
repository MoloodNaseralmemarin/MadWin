
using MadWin.Core.Entities.Orders;
using MadWin.Core.Entities.SentMessages;
using MadWin.Core.Interfaces;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MadWin.Infrastructure.Repositories
{
    public class SmsRepository : GenericRepository<Sms>, ISmsRepository
    {
        private readonly MadWinDBContext _context;

        public SmsRepository(MadWinDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sms>> GetRecentSmsAsync(int count)
        {
            return await _context.SentMessages
                .OrderByDescending(s => s.CreateDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sms>> GetFailedSmsAsync()
        {
            return await _context.SentMessages
                .Where(s => s.Status == "Failed")
                .ToListAsync();
        }

        public Task<bool> SendWelcomeSmsAsync(string cellPhone, string fullName, int smsCount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendSMSOrderForCustomerAsync(string cellPhone, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendSMSOrderForProductionAsync(string cellPhone, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendSMSOrderForManagerAsync(string cellPhone, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task SaveSmsAsync(string phoneNumber, string template, string message, string contact, int smsCount, string status, int? orderId = null, int? factorId = null)
        {
            throw new NotImplementedException();
        }
    }

}
