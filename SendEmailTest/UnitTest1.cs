using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using SendEmail;
using SendEmail.Services;
using Azure.Communication.Email;
using Azure;

namespace SendEmailTest
{
    public class EmailServiceTest : IEmailService
    {
        private string recipient;
        public EmailServiceTest(string recipient) {
            this.recipient = recipient;
        }

        public void SendEmail(string sender, string recipient, string subject, string htmlText, string text)
        {
            Assert.Equal(this.recipient, recipient);
        }
    }


    public class SendEmailTest
    {
        [Fact]
        async public Task SendEmailSuccessfully()
        {
            string email = "123@123";
            string name = "test.docx";
            byte[] content = [0, 1, 2, 3, 4, 5];

            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "email", email }
            };

            BlobUploadOptions options = new BlobUploadOptions();
            options.Metadata = metadata;

            BlobContainerClient blobContainerClient = new BlobContainerClient("UseDevelopmentStorage=true", "documents");
            await blobContainerClient.DeleteIfExistsAsync();
            await blobContainerClient.CreateAsync();

            BlobClient blobClient = new BlobClient("UseDevelopmentStorage=true", "documents", name);
            await blobClient.DeleteIfExistsAsync();
            await blobClient.UploadAsync(BinaryData.FromBytes(content), options);


          string connectionString =  Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", "UseDevelopmentStorage=true");

            Function1 function1 = new Function1(new LoggerFactory(), new EmailServiceTest(email));
            function1.Run("123", name);

            Environment.SetEnvironmentVariable("AzureWebJobsStorage", connectionString);


            await blobClient.DeleteIfExistsAsync();
            await blobContainerClient.DeleteIfExistsAsync();
        }
    }
}