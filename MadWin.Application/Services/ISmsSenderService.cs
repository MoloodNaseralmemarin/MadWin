namespace MadWin.Application.Services
{
    public interface ISmsSenderService
    {
        Task<bool> SendWelcomeSmsAsync(string cellPhone, string fullName, int smsCount);
        Task<bool> SendSMSOrderForCustomerAsync(string cellPhone, int orderId);
        Task<bool> SendSMSOrderForProductionAsync(string cellPhone, int orderId);
        Task<bool> SendSMSOrderForManagerAsync(string cellPhone, int orderId);

        Task<bool> SendSMSFactorForCustomerAsync(string cellPhone, int factorId);

        Task<bool> SendSMSFactorManagerAsync(int factorId);

        Task<bool> SendSMSForgotPasswordForCustomer(string cellPhone,string password);



        Task SaveSmsAsync(string phoneNumber, string template, string message, string contact, int smsCount, string status, int? orderId = null, int? factorId = null);
        }
    }

