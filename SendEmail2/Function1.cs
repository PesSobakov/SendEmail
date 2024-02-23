using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Communication.Email;
//using SendEmail.Services;

namespace SendEmail
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([BlobTrigger("documents/{name}", Connection = "AzureWebJobsStorage")] string myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name}");

            BlobClient blobClient = new BlobClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "documents", name);
            Uri sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
            var properties = blobClient.GetProperties();
            var prop = properties.Value;
            string email;
            prop.Metadata.TryGetValue("email", out email);

            
               string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
              var emailClient = new EmailClient(connectionString);
            
              EmailSendOperation emailSendOperation = emailClient.Send(
                  WaitUntil.Completed,
                  senderAddress: "DoNotReply@aa6c7eff-7b5f-4803-9048-c457e93b3476.azurecomm.net",
                  recipientAddress: email,
                  subject: "File uploaded",
                  htmlContent: $"<html><p>The file is successfully uploaded</p><p><a href=\"{sasUri}\">{sasUri}</a></p></html>",
                  plainTextContent: $"The file is successfully uploaded {sasUri}");

        }
    }

    /*
      public class Function1
     {
         private readonly ILogger _logger;
         private readonly IEmailService _emailService;

         public Function1(ILoggerFactory loggerFactory, IEmailService emailService)
         {
             _logger = loggerFactory.CreateLogger<Function1>();
             _emailService = emailService;
         }

         [Function("Function1")]
         public void Run([BlobTrigger("documents/{name}", Connection = "AzureWebJobsStorage")] string myBlob, string name)
         {
             _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name}");

             BlobClient blobClient = new BlobClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "documents", name);
             Uri sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
             var properties = blobClient.GetProperties();
             var prop = properties.Value;
             string email;
             prop.Metadata.TryGetValue("email", out email);

             _emailService.SendEmail(
                 "DoNotReply@aa6c7eff-7b5f-4803-9048-c457e93b3476.azurecomm.net",
                 email,
                 "File uploaded",
                 $"<html><p>The file is successfully uploaded</p><p><a href=\"{sasUri}\">{sasUri}</a></p></html>",
                 $"The file is successfully uploaded {sasUri}");

             // Этот код извлекает строку подключения из переменной среды.
           //  string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
           //  var emailClient = new EmailClient(connectionString);
           //
           //  EmailSendOperation emailSendOperation = emailClient.Send(
           //      WaitUntil.Completed,
           //      senderAddress: "DoNotReply@aa6c7eff-7b5f-4803-9048-c457e93b3476.azurecomm.net",
           //      recipientAddress: email,
           //      subject: "File uploaded",
           //      htmlContent: $"<html><p>The file is successfully uploaded</p><p><a href=\"{sasUri}\">{sasUri}</a></p></html>",
           //      plainTextContent: $"The file is successfully uploaded {sasUri}");

         }
     }
    */
}
