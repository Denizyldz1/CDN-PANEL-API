using MimeKit;
using MailKit.Net.Smtp;

namespace AppouseProjet.API.MailServices
{
    public class MailService : IMailService
    {
        public Task SendEmailAsync(MailModel model)
        {
            // MimeMessage nesnesini oluşturuyoruz.
            MimeMessage mimeMessage = new ();

            // Gönderici e-posta adresi ve ismi ile MailboxAddress nesnesini oluşturuyoruz.
            var mailboxAddressFrom = new MailboxAddress("Deniz Yıldız", "denizmailservice@gmail.com");
            // Oluşturduğumuz gönderici adresini MimeMessage nesnesine ekliyoruz.
            mimeMessage.From.Add(mailboxAddressFrom);

            // Alıcı e-posta adresi ve ismi ile MailboxAddress nesnesini oluşturuyoruz.
            var mailboxAddressTo = new MailboxAddress(model.To, model.Email);
            // Oluşturduğumuz alıcı adresini MimeMessage nesnesine ekliyoruz.
            mimeMessage.To.Add(mailboxAddressTo);

            // E-posta gövdesini oluşturan BodyBuilder nesnesini oluşturuyoruz.
            var bodyBuilder = new BodyBuilder();

            // HTML formatındaki e-posta gövdesini BodyBuilder nesnesine ekliyoruz.
            bodyBuilder.HtmlBody = model.Body;
            // Oluşturduğumuz e-posta gövdesini MimeMessage nesnesine ekliyoruz.
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            // E-posta konusunu ayarlıyoruz.
            mimeMessage.Subject = model.Subject;

            // SMTP sunucusuna bağlanmak için SmtpClient nesnesini oluşturuyoruz.
            var smtpClient = new SmtpClient();

            // SMTP sunucusuna bağlantı kuruyoruz (smtp.gmail.com adresine, 587 numaralı port ile güvenli olmayan bağlantı ile).
            smtpClient.Connect("smtp.gmail.com", 587, false);

            // SMTP sunucusuna kimlik doğrulaması yaparak bağlanıyoruz.
            smtpClient.Authenticate("denizmailservice@gmail.com", "Deniz1234.");

            // Oluşturduğumuz MimeMessage nesnesini SMTP sunucusuna gönderiyoruz.
            smtpClient.Send(mimeMessage);

            // SMTP sunucusuyla bağlantıyı sonlandırıyoruz.
            smtpClient.Disconnect(true);

            // E-posta gönderme işlemi tamamlandı, tamamlandı bilgisini döndürüyoruz.
            return Task.CompletedTask;

        }
    }
}
