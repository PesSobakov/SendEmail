//using Azure.Communication.Email;
//using Microsoft.Extensions.Configuration;
//using Azure;
//
//namespace SendEmail.Services
//{
//    public class EmailService:IEmailService
//    {
//        private readonly string _emailConnectionString;
//
//        public EmailService(IConfiguration configuration)
//        {
//            _emailConnectionString = configuration.GetValue<string>("COMMUNICATION_SERVICES_CONNECTION_STRING");
//        }
//
//        public void SendEmail(string sender, string recipient, string subject, string htmlText, string text)
//        {
//            var emailClient = new EmailClient(_emailConnectionString);
//
//            EmailSendOperation emailSendOperation = emailClient.Send(
//                WaitUntil.Completed,
//                senderAddress: sender,
//                recipientAddress: recipient,
//                subject: subject,
//                htmlContent: htmlText,
//                plainTextContent: text);
//        }
//    }
//}
