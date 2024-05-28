namespace Autenticacao.Consumidor.Consumidor;

public class RecuperarSenhaConsumidor(ILogger<RecuperarSenhaConsumidor> logger, IEmailService emailService) : IConsumer<RecuperarSenha>
{
    private const string Assunto = "Recuperação de Senha";
    private const string Corpo = "Acesse o link http://localhost:5014/swagger/index.html para recuperar a senha.";
    
    public Task Consume(ConsumeContext<RecuperarSenha> context)
    {
        var envioRecuperarSenha = context.Message;
        
        try
        {
            if (string.IsNullOrWhiteSpace(envioRecuperarSenha.Email))
                return Task.CompletedTask;
            
            emailService.EnviarEmail(envioRecuperarSenha.Email, Assunto, Corpo);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Ocorreu um erro ao enviar e-mail para {envioRecuperarSenha.Email} recuperar senha.");
            throw;
        }
    }
}