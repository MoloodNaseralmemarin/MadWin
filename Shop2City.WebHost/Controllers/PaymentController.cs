
using MadWin.Application.Services;
using MadWin.Core.Entities.Advices;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Entities.SentMessages;
using MadWin.Core.Entities.Transactions;
using MadWin.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop2City.Core.Services.Transactions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace Shop2City.WebHost.Controllers
{
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly IFactorService _factorService;
        private readonly IOrderService _orderService;
        private readonly ISmsSenderService _smsSenderService;
        private readonly IFactorDetailService _factorDetailService;

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IUserService userService, ITransactionService transactionService, IFactorService factorService, IOrderService orderService, ISmsSenderService smsSenderService, ILogger<PaymentController> logger, IFactorDetailService factorDetailService)
        {
            _userService = userService;
            _transactionService = transactionService;
            _factorService = factorService;
            _orderService = orderService;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(300)
            };
            _factorService = factorService;
            _smsSenderService = smsSenderService;
            _logger = logger;
            _factorDetailService = factorDetailService;
        }
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return Json(new { success = false, message = "کاربر نامعتبر" });
            #region بدست آوردن شماره همراه کاربر برای ارسال پیامک
            var cellPhone = await _userService.GetCellPhoneByUserIdAsync(int.Parse(userIdString));
            #endregion

            string callbackUrl = $"https://madwin.ir/Payment/Verify?source={request.Source}";
            string url = "https://sepehr.shaparak.ir:8081/V1/PeymentApi/GetToken";
           var paymentRequest = new
            {
                TerminalID = "98808771",
                Amount = request.SumOrder, // اینجا می‌توانید از request.SumOrder استفاده کنید
                InvoiceID = request.InvoiceId, // در صورت نیاز، مقدار فاکتور را از جدول بگیرید
                callbackURL = callbackUrl,
                payload = ""
            };

            //// سریالایز کردن داده‌های درخواست پرداخت
            string jsonData = JsonSerializer.Serialize(paymentRequest);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            //// ارسال درخواست برای دریافت توکن
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
              var jsonObject = JObject.Parse(responseData);
               var token = jsonObject["Accesstoken"]?.ToString();

               if (!string.IsNullOrEmpty(token))
               {
                    string redirectUrl = $"https://sepehr.shaparak.ir:8080/Pay?token={token}&terminalID=98808771&getMethod=0";

                    if (request.Source == "order")
                    {
                        var order = await _orderService.GetOrderByOrderIdAsync(request.InvoiceId);
                        #region مطمینم رفته به درگاه اینترنتی و همه چیز درسته پس هم isfinaly true کنه و هم ارسال SMS
                        await _orderService.UpdatePriceAndDeliveryAsync(request.deliveryId, request.InvoiceId);
                        await _orderService.UpdateIsFinalyOrderAsync(order);

                        // ارسال پیامک

                        #region send SMS To Customer

                        var smsCustomerSent = await _smsSenderService.SendSMSOrderForCustomerAsync(cellPhone, order.Id);

                        if (smsCustomerSent)
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای کاربرموفق بوده است.", cellPhone);
                        }
                        else
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای کاربر ناموفق بوده است.", cellPhone);

                        }
                        #endregion

                        #region send SMS To Admin

                        var smsAdminSent = await _smsSenderService.SendSMSOrderForManagerAsync("09180580270", order.Id);

                        if (smsAdminSent)
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای مدیر موفق بوده است.", cellPhone);
                        }
                        else
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای مدیر ناموفق بوده است.", cellPhone);

                        }
                        #endregion

                        #region Send SMS To Production

                        var smsProductionSent = await _smsSenderService.SendSMSOrderForProductionAsync("09182185223", order.Id);

                        if (smsProductionSent)
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای مدیر موفق بوده است.", cellPhone);
                        }
                        else
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای مدیر ناموفق بوده است.", cellPhone);

                        }
                        #endregion
                        #endregion
                    }
                    else if(request.Source=="factor")
                    {
                        var factor = await _factorService.GetFactorByFactorIdAsync(request.InvoiceId);
                        #region آپدیت نحوه ارسال
                        await _factorService.UpdatePriceAndDeliveryAsync(request.deliveryId, request.InvoiceId);
                        await _factorService.UpdateIsFinalyFactorAsync(factor);
                        #endregion
                        //ارسال پیامک
                        #region send SMS To Customer
                        var smsCustomerSent = await _smsSenderService.SendSMSFactorForCustomerAsync(cellPhone, factor.Id);

                        if (smsCustomerSent)
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای کاربرموفق بوده است.", cellPhone);
                        }
                        else
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای کاربر ناموفق بوده است.", cellPhone);

                        }
                        #endregion
                        #region Send SMS To Manager
                        var smsManagerSent = await _smsSenderService.SendSMSFactorManagerAsync(factor.Id);

                        if (smsCustomerSent)
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای کاربرموفق بوده است.", cellPhone);
                        }
                        else
                        {
                            // فقط لاگ کن، بدون اطلاع دادن به کاربر
                            _logger.LogWarning("ارسال پیامک برای کاربر ناموفق بوده است.", cellPhone);

                        }
                        #endregion

                    }




                    return Json(new { success = true, redirectUrl = redirectUrl });
                       }

                      return Json(new { success = false, message = "توکن از درگاه پرداخت دریافت نشد." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "خطا در دریافت توکن پرداخت." });
                    }
                }

                [HttpPost]
        public async Task<IActionResult> Verify()
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest("داده‌های فرم نادرست دریافت شده است.");
            }
            var source = Request.Query["source"].ToString();
            var amountStr = Request.Form["Amount"];
            var digitalReceipt = Request.Form["DigitalReceipt"].ToString();
            var datePaidStr = Request.Form["DatePaid"].ToString();
            var terminalId = Request.Form["TerminalId"].ToString();
            int invoiceId = int.Parse(Request.Form["InvoiceId"]);
            var cardNumber = Request.Form["CardNumber"].ToString();
            var payload = Request.Form["Payload"].ToString();
            var rrn = Request.Form["Rrn"].ToString();
            var respMsg = Request.Form["RespMsg"].ToString();
            var traceNumber = Request.Form["TraceNumber"].ToString();
            var respCode = Request.Form["RespCode"].ToString();
            var issuerBank = Request.Form["IssuerBank"].ToString();
            // ارسال درخواست Advice
            var url = "https://sepehr.shaparak.ir:8081/V1/PeymentApi/Advice";
            var advice = new
            {
                digitalreceipt = digitalReceipt,
                Tid = terminalId
            };

            var jsonData = JsonSerializer.Serialize(advice);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);  // عملیات غیرهمزمان برای ارسال درخواست
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(responseData);
                var adviceModel = new AdviceModel
                {
                    Status = jsonObject["Status"]?.ToString(),
                    ReturnId = jsonObject["ReturnId"]?.ToString(),
                    Message = jsonObject["Message"]?.ToString(),
                };
                await _transactionService.AddAdvice(adviceModel);

                var transactionModel = new TransactionModel
                {
                    DigitalReceipt=digitalReceipt,
                    Amount=amountStr,
                    CardNumber=cardNumber,
                    DatePaid=datePaidStr,
                    InvoiceId=invoiceId,
                    IssuerBank=issuerBank,
                    Payload=payload,
                    RespCode=respCode,
                    RespMsg=respMsg,
                    Rrn=rrn,
                    TerminalId=terminalId,
                    TraceNumber=traceNumber
                };
                await _transactionService.AddTransaction(transactionModel);
            }

            if (source == "order")
            {
                return RedirectToAction("ShowOrderForUser", "Orders", new { area = "UserPanel", orderId = invoiceId });
            }
            else if (source == "factor")
            {
                return RedirectToAction("ShowFactorForUser", "Factors", new { area = "UserPanel", factorId = invoiceId });
            }
        
            return BadRequest("پارامتر source نامعتبر است.");
        }

        public class PaymentRequest
        {
            public int SumOrder { get; set; }
            public int InvoiceId { get; set; }
            public string Source { get; set; }

            public int deliveryId { get; set; }
        }
    }
}
                                                                                                                                                                                        