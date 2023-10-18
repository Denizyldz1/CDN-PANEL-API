namespace AppouseProjet.API.MailServices
{
    public interface IMailService
    {
        Task SendEmailAsync(MailModel model);
    }
}
