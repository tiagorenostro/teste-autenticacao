namespace Autenticacao.Consumidor.Consumidor;

public class EnviarCodigoAtivacaoConsumidor(ILogger<EnviarCodigoAtivacaoConsumidor> logger, IEmailService emailService) : IConsumer<EnviarCodigoAtivacao>
{
    private const string Assunto = "Código Ativação";
    private const string Corpo = "Seu código de ativação é {0}";
    
    public Task Consume(ConsumeContext<EnviarCodigoAtivacao> context)
    {
        var envioCodigoAtivacao = context.Message;
        
        try
        {
            if (string.IsNullOrWhiteSpace(envioCodigoAtivacao.Email))
                return Task.CompletedTask;

            emailService.EnviarEmail(envioCodigoAtivacao.Email, Assunto, string.Format(Corpo, envioCodigoAtivacao.CodigoAtivacao));

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Ocorreu um erro ao enviar e-mail para {envioCodigoAtivacao.Email} do código de ativação.");
            throw;
        }
    }
}

