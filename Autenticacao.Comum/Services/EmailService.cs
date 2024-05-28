namespace Autenticacao.Comum.Services;

public class EmailService(Configuracao.Configuracao configuracao) : IEmailService
{
    public void EnviarEmail(string emailPara, string assunto, string corpo)
    {
        using var stmpClient = new SmtpClient(configuracao.Email.Host, configuracao.Email.Porta)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(configuracao.Email.Usuario, configuracao.Email.Senha)
        };
        
        stmpClient.Send(new MailMessage(configuracao.Email.Usuario, emailPara, assunto, corpo));
    }
}