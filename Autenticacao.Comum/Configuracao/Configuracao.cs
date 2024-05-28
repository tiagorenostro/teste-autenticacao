namespace Autenticacao.Comum.Configuracao;

public class Configuracao
{
    public string ConnectionString { get; set; }
    public ConfiguracaoRabbitMQ RabbitMQ { get; set; }
    public ConfiguracaoEmail Email { get; set; }
}