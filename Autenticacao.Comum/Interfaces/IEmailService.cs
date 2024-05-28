namespace Autenticacao.Comum.Interfaces;

public interface IEmailService
{
    void EnviarEmail(string emailPara, string assunto, string corpo);
}