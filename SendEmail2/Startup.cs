//using Microsoft.Azure.Functions.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
//using SendEmail.Services;
//
//[assembly: FunctionsStartup(typeof(SendEmail.Startup))]
//
//namespace SendEmail
//{
//    public class Startup : FunctionsStartup
//    {
//        public override void Configure(IFunctionsHostBuilder builder)
//        {
//            builder.Services.AddSingleton<IEmailService, EmailService>();
//        }
//    }
//}
