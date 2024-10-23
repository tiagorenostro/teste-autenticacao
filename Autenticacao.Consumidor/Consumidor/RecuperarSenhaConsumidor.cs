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
            
            var resultadoTask = emailService.EnviarEmailAsync(envioRecuperarSenha.Email, Assunto, Corpo);

            if (!resultadoTask.IsCompletedSuccessfully)
                throw resultadoTask.Exception ?? new Exception("Ocorreu um erro ao enviar e-mail da recuperação da senha.");

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Ocorreu um erro ao enviar e-mail para {envioRecuperarSenha.Email} recuperar senha.");
            throw;
        }
    }
}