using MadWin.Core.Entities.SentMessages;


namespace MadWin.Core.Interfaces
{
    public interface ISmsRepository : IGenericRepository<Sms>
    {
        Task<bool> SendWelcomeSmsAsync(string cellPhone, string fullName, int smsCount);
        Task<bool> SendSMSOrderForCustomerAsync(string cellPhone, int orderId);
        Task<bool> SendSMSOrderForProductionAsync(string cellPhone, int orderId);
        Task<bool> SendSMSOrderForManagerAsync(string cellPhone, int orderId);

        Task SaveSmsAsync(string phoneNumber, string template, string message, string contact, int smsCount, string status, int? orderId = null, int? factorId = null);
    }
}
