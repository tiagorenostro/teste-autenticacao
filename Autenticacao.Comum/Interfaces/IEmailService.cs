namespace Autenticacao.Comum.Interfaces;

public interface IEmailService
{
    Task EnviarEmailAsync(string emailPara, string assunto, string corpo);
}