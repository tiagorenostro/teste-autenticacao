namespace Autenticacao.Comum.Interfaces;

public interface IEmailService
{
    Task EnviarEmail(string emailPara, string assunto, string corpo);
}