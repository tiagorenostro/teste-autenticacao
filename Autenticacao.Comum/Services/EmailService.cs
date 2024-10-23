namespace Autenticacao.Comum.Services;

public class EmailService(Configuracao.Configuracao configuracao) : IEmailService
{
    public Task EnviarEmailAsync(string emailPara, string assunto, string corpo)
    {
        using var stmpClient = new SmtpClient(configuracao.Email.Host, configuracao.Email.Porta)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(configuracao.Email.Usuario, configuracao.Email.Senha)
        };
        
        stmpClient.SendAsync(new MailMessage(configuracao.Email.Usuario, emailPara, assunto, corpo), null);

        return Task.CompletedTask;
    }
}